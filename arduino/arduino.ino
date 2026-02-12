const int coin = 2;
volatile bool coinFlag = false;

void setup() {
  pinMode(coin, INPUT_PULLUP);

  Serial.begin(9600);
  delay(500);
  Serial.println("START");

  attachInterrupt(digitalPinToInterrupt(coin), onCoin, FALLING);
}

void loop() {
  if (coinFlag) {
    coinFlag = false;
    Serial.println(1);
  }
}

void onCoin() {
  coinFlag = true;
}