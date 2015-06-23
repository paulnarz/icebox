#include <IRremote.h>
#include <IRremoteInt.h>

const int RECV_PIN = 2;

IRrecv irrecv(RECV_PIN);

IRsend irsend;

decode_results results;

void setup()
{
  Serial.begin(9600);
  irrecv.enableIRIn(); // Start the receiver
  irrecv.blink13(true);
  Serial.print("TIMER_PWM_PIN: ");
  Serial.print(TIMER_PWM_PIN);
  Serial.println();
  Serial.println();  
  
  Serial.print("decode_type");
  Serial.print("\t");
  Serial.print("value");
  Serial.print("\t");
  Serial.print("valueHEX");
  Serial.print("\t");
  Serial.print("bits");
  Serial.println();
}

void loop() {
  if (irrecv.decode(&results)) {
    if (results.decode_type == NEC) {
      Serial.print("NEC");
    } else if (results.decode_type == SONY) {
      Serial.print("SONY");
    } else if (results.decode_type == RC5) {
      Serial.print("RC5");
    } else if (results.decode_type == RC6) {
      Serial.print("RC6");
    } else if (results.decode_type == UNKNOWN) {
      Serial.print("UNKNOWN");
    } else {
      Serial.print(results.decode_type);
    }
    Serial.print("\t");
    Serial.print(results.value);
    Serial.print("\t");
    Serial.print(results.value, HEX);
    Serial.print("\t");
    Serial.print(results.bits);
    Serial.println();
    irrecv.resume(); // Receive the next value
  }
}

