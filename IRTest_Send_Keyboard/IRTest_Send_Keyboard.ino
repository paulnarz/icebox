#include <IRremote.h>
#include <IRremoteInt.h>

IRsend irsend;

int incomingByte = 0;
byte incomingBuffer[4];
unsigned long incomingCode;

void setup() {
  Serial.begin(9600);
}

void loop() {
  while (Serial.available() > 0) {
    incomingByte = Serial.read();
    if (incomingByte == 'L') {
      Serial.readBytes(incomingBuffer, 4);
      incomingCode = ReadUInt32FromBuffer(incomingBuffer, 4, 0);
      //Serial.print("Received: L");
      //Serial.print(incomingCode);
      //Serial.println();
      irsend.sendNEC(incomingCode, 32);      
    }
  }
}

unsigned long ReadUInt32FromBuffer(byte buffer[], int bytes, int offset)
{
  unsigned long value = 0;
  for (int i = 0; i < bytes; i++)
  {
    //Serial.print("i: ");
    //Serial.print(i);
    //Serial.print(" buffer[i + offset]: ");
    //Serial.print(buffer[i + offset]);    
    //Serial.print(" << ");
    //Serial.print(((unsigned long)buffer[i + offset]) << (i * 8));        
    //Serial.println();
    
    value += ((unsigned long)buffer[i + offset]) << (i * 8);
  }
  return value;
}
