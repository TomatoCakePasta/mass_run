const int encoderPin = 2;
const int coinPin = 5;
int lastCoinState = LOW;

volatile long totalCount = 0;

const int PPR = 600;
const int baseN = 10;
const int baseD = 5;

unsigned long lastTime = 0;
unsigned long lastCoinTime = 0;
long lastCount = 0;

int lastOutput = 0;
int keepCnt = 3;
int sendMillis = 1000;

// ==== DEBUG MODE ===
bool DEBUG = false;
int debugData[] = {0, 1, 2, 3, 2, 1};

void setup() {
  Serial.begin(115200);
  pinMode(encoderPin, INPUT_PULLUP);
  pinMode(coinPin, INPUT_PULLUP);
  attachInterrupt(digitalPinToInterrupt(encoderPin), pulse, RISING);
}

void loop() {

  if (DEBUG) {
    int size = sizeof(debugData) / sizeof(debugData[0]);
    for (int i = 0; i < size; i++) {
      Serial.println(debugData[i]);
      delay(4000);
    }
    return;
  }

  if (millis() - lastTime >= sendMillis) {

    noInterrupts();
    long currentCount = totalCount;
    interrupts();

    long diff = currentCount - lastCount;
    lastCount = currentCount;

    float rotations = (float)diff / PPR;

    Serial.println(rotations);

    lastTime = millis();
  }

  // coin selector
  int state = digitalRead(coinPin);

  if (lastCoinState == HIGH && state == LOW) {
    // chattering
    if (millis() - lastCoinTime > 200) {
      Serial.println("T");
      lastCoinTime = millis();
    }
  }

  lastCoinState = state;
}

void pulse() {
  totalCount++;
}