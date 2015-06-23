# icebox

This project hacks together various technologies in an attempt to control an air conditioner from the internet.

### Icebox.SerialListener
A windows console app that connects to the Arduino serial port.
It logs all serial messages from the Arduino to the console.
It works with the TemperatureReader sketch.
It logs temperature readings from the Arduino to a file.

### IRTest_Receive
Test sketch for decoding IR codes.

### IRTest_Send
Test sketch for sending IR codes to a device.

### TemperatureReader
Arduino sketch for periodically logging values from a temperature sensor.

### **NOTE**
The Arudino sketches depend on the [Arduino-IRremote](https://github.com/shirriff/Arduino-IRremote) library.
In order to get this library to work, you might have to remove the old version of the library that comes packaged with the Arduino IDE. To do this, go to the Arduino install folder, in the libraries folder, remove the RobotIRremote folder.