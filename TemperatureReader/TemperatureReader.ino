struct Light {
  int pin;
  int value;
  float temp;
};

const int sensorPin = A0;
const int lightLen = 0;
const unsigned long interval = 5UL * 60UL * 1000UL; // 5 mins
//const unsigned long interval = 1UL * 1000UL; // 1 sec
long previousMillis = 0;

Light lights[] = {
  { 2, LOW, 90.0 },
  { 3, HIGH, 95.0 },
  { 4, LOW, 100.0 },
};

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  for (int i = 0; i < lightLen; i++) {
    pinMode(lights[i].pin, OUTPUT);
  }
}

void loop() {
  unsigned long currentMillis = millis();
  if (currentMillis - previousMillis <= interval)
    return;  
  previousMillis = currentMillis;
  
  int sensorValue = analogRead(sensorPin);
  float voltage = (sensorValue / 1024.0) * 5.0;
  float tempC = (voltage - 0.5) * 100;
  float tempF = (tempC * 9 / 5) + 32;
  
  for (int i = 0; i < lightLen; i++) {
    if (tempF > lights[i].temp)
      lights[i].value = HIGH;
    else
      lights[i].value = LOW;
  }

  for (int i = 0; i < lightLen; i++) {
    digitalWrite(lights[i].pin, lights[i].value);
  }

  logRaw(sensorValue, voltage, tempC, tempF);
}

void logRaw(int sensorValue, float voltage, float tempC, float tempF)
{
  Serial.write("T");
  Serial.write(sensorValue);
}

void logJson(int sensorValue, float voltage, float tempC, float tempF)
{
  Serial.print("{");
  Serial.print("sensorValue:");  
  Serial.print(sensorValue);
  Serial.print(",");
  Serial.print("voltage:");  
  Serial.print(voltage, 8);
  Serial.print(",");
  Serial.print("tempC:");  
  Serial.print(tempC, 8);
  Serial.print(",");
  Serial.print("tempF:");  
  Serial.print(tempF, 8);
  Serial.print("}");
  Serial.println();
}
