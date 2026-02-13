import processing.serial.*;

Serial port;

float value = 0;     // 0-1000
float speed = 0;
float phase = 0;

float SCALE = 3.0;   // 棒人間の大きさ

int treeCount = 10;
float[] treeX = new float[treeCount];

boolean DEBUG = false;  // trueでマウス操作
int portIdx = 2;        // 自分の環境に合わせる
int baud = 9600;

void setup() {
  size(1600, 800);

  // 木をランダム配置
  for (int i = 0; i < treeCount; i++) {
    treeX[i] = random(width);
  }

  String[] ports = Serial.list();
  println("=== Serial Ports ===");
  for (int i = 0; i < ports.length; i++) {
    println(i + ": " + ports[i]);
  }

  if (!DEBUG) {
    port = new Serial(this, ports[portIdx], baud);
    port.bufferUntil('\n');   // ← 超重要
  }
}

void draw() {
  background(200, 230, 255);

  // ===== DEBUGモード =====
  if (DEBUG) {
    value = map(mouseX, 0, width, 0, 1000);
    value = constrain(value, 0, 1000);
  }

  // ===== スピード計算 =====
  speed = map(value, 0, 1000, 0, 20);

  // 足のアニメーション
  phase += speed * 0.08;

  // ===== 木を右→左へ =====
  for (int i = 0; i < treeCount; i++) {
    treeX[i] -= speed;

    if (treeX[i] < -100) {
      treeX[i] = width + random(200);
    }

    drawTree(treeX[i], height/2 + 80);
  }

  // ===== 棒人間（中央固定）=====
  drawRunner(width/2, height/2, phase, SCALE);

  // ===== 表示 =====
  fill(0);
  textSize(24);
  text("value: " + int(value), 40, 50);
  text("speed: " + nf(speed, 1, 2), 40, 80);
}

void serialEvent(Serial p) {
  try {
    String input = p.readStringUntil('\n');

    if (input != null) {
      input = trim(input);

      if (input.matches("\\d+")) {   // 数字だけ受け取る
        value = int(input);
        println("recv: " + value);
      }
    }

  } catch (Exception e) {
    println("parse error");
  }
}

void drawRunner(float x, float y, float p, float s) {
  pushMatrix();
  translate(x, y);
  scale(s);

  stroke(0);
  strokeWeight(3 / s);
  fill(0);

  // 頭
  ellipse(0, -40, 20, 20);

  // 体
  line(0, -30, 0, 0);

  // 腕
  float armSwing = sin(p) * 20;
  line(0, -20, armSwing, -5);
  line(0, -20, -armSwing, -5);

  // 足
  float legSwing = sin(p) * 25;
  line(0, 0, legSwing, 30);
  line(0, 0, -legSwing, 30);

  popMatrix();
}

void drawTree(float x, float y) {
  pushMatrix();
  translate(x, y);

  // 幹
  fill(120, 70, 20);
  rect(-10, 0, 20, 60);

  // 葉
  fill(30, 160, 60);
  triangle(-80, 0, 0, -120, 80, 0);

  popMatrix();
}
