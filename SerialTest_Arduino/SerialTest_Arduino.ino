const int buttonPin = 4;
int buttonState = 0;

int flag = 0;

int rotaryPin = A0;
int rotaryValue = 0;

void setup() {
    Serial.begin(9600); //シリアルポートを9600bpsで開く
    while (!Serial);    // 準備が終わるのを待つ

    pinMode(buttonPin, INPUT);
    pinMode(rotaryPin, INPUT);
}


void loop()
{
    buttonState = digitalRead(buttonPin);
    rotaryValue = analogRead(rotaryPin);

    if(flag== 0 && buttonState == HIGH){
    flag = 1;
    Serial.print("S");
    Serial.println();
    }
    if(flag== 1 && buttonState == LOW){
    flag = 0;
    }


    Serial.print(rotaryValue);
    Serial.println();
    delay(50);
}
