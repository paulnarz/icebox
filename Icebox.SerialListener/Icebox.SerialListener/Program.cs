using Icebox.SerialListener.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Icebox.SerialListener
{
    class Program
    {
        static bool _continue;
        static SerialPort _serialPort;
        static string _outputPath = "./../../../../logs/tempreader.txt";
        static Dictionary<ConsoleKey, uint> _keyMappings;

        static void Main(string[] args)
        {
            _outputPath = Path.GetFullPath(_outputPath);
            Console.WriteLine(_outputPath);

            _keyMappings = new Dictionary<ConsoleKey, uint>();
            _keyMappings.Add(ConsoleKey.Enter, Convert.ToUInt32("0x10AF8877", 16));
            _keyMappings.Add(ConsoleKey.UpArrow, Convert.ToUInt32("0x10AF708F", 16));
            _keyMappings.Add(ConsoleKey.DownArrow, Convert.ToUInt32("0x10AFB04F", 16));

            foreach (var mapping in _keyMappings)
            {
                Console.WriteLine("{0}\t{1}\t{1}", mapping.Key, mapping.Value);
            }

            string portName = "COM3";
            int baudRate = 9600;

            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;

            Console.WriteLine("Available Ports:");
            foreach (string s in SerialPort.GetPortNames())
            {
                Console.WriteLine("   {0}", s);
            }

            _serialPort = new SerialPort(portName, baudRate);
            Console.WriteLine(" PortName: {0}", _serialPort.PortName);
            Console.WriteLine(" BaudRate: {0}", _serialPort.BaudRate);

            Thread readThread = new Thread(ReadRaw);
            _serialPort.Open();
            _continue = true;
            readThread.Start();

            Console.WriteLine("ESC to exit");

            while (_continue)
            {
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Escape)
                {
                    _continue = false;
                }
                else if (_keyMappings.ContainsKey(key.Key))
                {
                    var code = _keyMappings[key.Key];
                    Console.WriteLine(code);
                    var buffer = new byte[5];
                    buffer[0] = (byte)'L';
                    WriteUInt32ToBuffer(buffer, code, 4, 1);
                    _serialPort.Write(buffer, 0, 5);
                }
            }

            readThread.Join();
            _serialPort.Close();
        }

        private static void WriteUInt32ToBuffer(byte[] buffer, uint value, int bytes, int offset)
        {
            for (int i = 0; i < bytes; i++)
            {
                buffer[i + offset] = (byte)((value & (0xFFU << (i * 8))) >> (i * 8));
            }
        }

        private static uint ReadUInt32FromBuffer(byte[] buffer, int bytes, int offset)
        {
            uint value = 0;
            for (int i = 0; i < bytes; i++)
            {
                value += ((uint)buffer[i + offset]) << (i * 8);
            }
            return value;
        }

        public static void ReadRaw()
        {
            while (_continue)
            {
                try
                {
                    var header = (char)_serialPort.ReadByte();
                    Console.Write(header);
                    if (header == 'T')
                    {
                        var data = new TemperatureData();
                        data.time = DateTime.Now;
                        data.sensorValue = _serialPort.ReadByte();
                        Console.Write(data.sensorValue);
                        data.voltage = (data.sensorValue / 1024.0f) * 5.0f;
                        data.tempC = (data.voltage - 0.5f) * 100f;
                        data.tempF = (data.tempC * 9f / 5f) + 32f;

                        LogToFile(data);
                        Console.WriteLine();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        public static void ReadJson()
        {
            while (_continue)
            {
                try
                {
                    string message = _serialPort.ReadLine();
                    Console.WriteLine(message);
                    var data = JsonConvert.DeserializeObject<TemperatureData>(message);
                    data.time = DateTime.Now;
                    LogToFile(data);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        public static void LogToFile(TemperatureData data)
        {
            var sb = new StringBuilder();
            sb.Append(data.time);
            sb.Append("\t");
            sb.Append(data.sensorValue);
            sb.Append("\t");
            sb.Append(data.voltage);
            sb.Append("\t");
            sb.Append(data.tempC);
            sb.Append("\t");
            sb.Append(data.tempF);
            sb.Append("\t");
            sb.AppendLine();

            EnsureFile(_outputPath);
            File.AppendAllText(_outputPath, sb.ToString());
        }

        public static void EnsureFile(string fileName)
        {
            string path = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (File.Exists(fileName))
                return;

            var sb = new StringBuilder();

            sb.Append("DateTime");
            sb.Append("\t");
            sb.Append("sensorValue");
            sb.Append("\t");
            sb.Append("voltage");
            sb.Append("\t");
            sb.Append("tempC");
            sb.Append("\t");
            sb.Append("tempF");
            sb.Append("\t");
            sb.AppendLine();

            File.WriteAllText(_outputPath, sb.ToString());
        }
    }
}
