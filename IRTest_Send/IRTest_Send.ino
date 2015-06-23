#include <IRremote.h>
#include <IRremoteInt.h>

IRsend irsend;

void setup() {
}

void loop() {
    irsend.sendNEC(0x10AF708Ful, 32); //up
    delay(500);
    irsend.sendNEC(0x10AFB04Ful, 32); //down
    delay(1500);
}
