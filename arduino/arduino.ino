const int coin = 4;
volatile bool coinFlag = false;

// Rotary encoder
const int pinA = 2;
const int pinB = 3;

const int MAX = 1000;
const int decremenmt = 25;

static const int encode_table[] =
{0, -1, 1, 0,
 1,  0, 0,-1,
-1,  0, 0, 1,
 0,  1,-1, 0};

volatile int value = 0;          // 0〜100用
volatile long rawCount = 0;

unsigned long lastMoveTime = 0;
const unsigned long timeout = 300;  // 無操作で0に戻る時間(ms)

void setup()
{
  Serial.begin(9600);

  pinMode(coin, INPUT_PULLUP);
  pinMode(pinA, INPUT_PULLUP);
  pinMode(pinB, INPUT_PULLUP);

  attachInterrupt(digitalPinToInterrupt(pinA), encode, CHANGE);
  attachInterrupt(digitalPinToInterrupt(pinB), encode, CHANGE);
}

void loop()
{
  if (digitalRead(coin) != HIGH) {
    coinFlag = false;
    Serial.println("T");
    delay(1000);
  }

  // 無操作なら0に戻す
  if (millis() - lastMoveTime > timeout) {
    value -= decremenmt;
    
    if (value < 0) {
      value = 0;
    }
  }

  if (value > 0) {
    Serial.println(value);
  }

  delay(100);
}


void encode()
{
  static int last_state = 0;

  int sig1 = digitalRead(pinA);
  int sig2 = digitalRead(pinB);

  int state = sig1 | (sig2 << 1);
  int sum = state | (last_state << 2);

  if (last_state != state) {

    int delta = encode_table[sum];

    // 正方向のみ加算
    if (delta > 0) {
      rawCount += delta;

      // スケール調整（600P/Rなどの場合）
      value = rawCount / 10;   // ←感度調整

      if (value > MAX) {
        value = MAX;
      }

      lastMoveTime = millis();
    }

    last_state = state;
  }
}