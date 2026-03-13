const int encoderPin = 2;

volatile long totalCount = 0;

const int PPR = 600;
const int baseN = 10;
const int baseD = 5;

unsigned long lastTime = 0;
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
}

void pulse() {
  totalCount++;
}