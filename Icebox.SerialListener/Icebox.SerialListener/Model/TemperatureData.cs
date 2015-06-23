using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icebox.SerialListener.Model
{
    public class TemperatureData
    {
        public DateTime time { get; set; }
        public int sensorValue { get; set; }
        public float voltage { get; set; }
        public float tempC { get; set; }
        public float tempF { get; set; }
    }
}
