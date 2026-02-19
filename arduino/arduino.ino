const int encoderPin = 2;

volatile long totalCount = 0;

const int PPR = 600;
const int baseN = 10;
const int baseD = 5;

unsigned long lastTime = 0;
long lastCount = 0;

int lastOutput = 0;
int keepCnt = 3;

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

  if (millis() - lastTime >= 1000) {

    noInterrupts();
    long currentCount = totalCount;
    interrupts();

    long diff = currentCount - lastCount;
    lastCount = currentCount;

    float rotations = (float)diff / PPR;

    int output;

    if (rotations >= baseN + (baseD * 2)) {
      output = 3;
    }
    else if (rotations >= baseN + baseD) {
      output = 1;
    }
    else {
      output = 0;
    }

    // === Important ===
    // If it jumps more than two steps, the previous value is retained.
    if ((abs(output - lastOutput) >= 2) && (output < lastOutput) && keepCnt > 0) {
      output = lastOutput;
      keepCnt--;
    }
    else {
      keepCnt = 3;
    }

    lastOutput = output;

    // Serial.print("rot/s: ");
    // Serial.println(rotations);

    int ret = 0;

    // super dash
    if (rotations > 30) {
      ret = 3;
    }
    // dash
    else if (rotations > 20) {
      ret = 2;
    }
    // walk
    else if (rotations > 10) {
      ret = 1;
    }
    Serial.println(ret);

    // Serial.print("  output: ");
    // Serial.println(output);

    lastTime = millis();
  }
}

void pulse() {
  totalCount++;
}