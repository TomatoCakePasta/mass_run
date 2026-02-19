import processing.serial.*;

Serial port;

boolean DEBUG = false;
int portIdx = 2;
int baud = 115200;

float value = 0;     // ← float対応
//float maxRPM = 200;  // 想定最大RPM（必要に応じて変更）
float maxRPM = 5;

float[] graph;
int graphWidth = 800;

void setup() {
  size(1200, 600);
  graph = new float[graphWidth];

  String[] ports = Serial.list();
  println("=== Serial Ports ===");
  for (int i = 0; i < ports.length; i++) {
    println(i + ": " + ports[i]);
  }

  if (!DEBUG) {
    port = new Serial(this, ports[portIdx], baud);
    port.bufferUntil('\n');
  }
}

void draw() {
  background(20);

  updateGraph();
  drawGrid();
  drawGraph();
  drawText();
}

void updateGraph() {
  for (int i = 0; i < graph.length - 1; i++) {
    graph[i] = graph[i + 1];
  }
  graph[graph.length - 1] = value;
}

void drawGraph() {
  stroke(0, 255, 100);
  strokeWeight(2);
  noFill();

  beginShape();
  for (int i = 0; i < graph.length; i++) {

    // ▼▼ ここが拡大ポイント ▼▼
    float normalized = graph[i] / maxRPM;
    float scaled = sqrt(normalized);  // 小さい値を拡大

    float y = map(scaled, 0, 1, height - 50, 50);
    vertex(i + 100, y);
  }
  endShape();
}

void drawGrid() {
  stroke(60);

  for (int i = 0; i <= 10; i++) {
    float y = map(i, 0, 10, height - 50, 50);
    line(100, y, width - 50, y);
  }
}

void drawText() {
  fill(255);
  textSize(16);
  text("RPM: " + nf(value, 0, 2), 100, 30);
}

void serialEvent(Serial p) {
  try {
    String input = p.readStringUntil('\n');

    if (input != null) {
      input = trim(input);

      if (input.matches("\\d+\\.?\\d*")) {
        value = float(input);   // ← float変換
      }
    }

  } catch (Exception e) {
    println("parse error");
  }
}
