import 'dart:math';

void main(List<String> args) {
  final m = {};
  m["helo"] = "hello";
  print(m);
}

enum Mode { ten, twenty }

class Point implements Operations {
  double x, y;

  Point(this.x, this.y);

  @override
  String toString() {
    return "(x, y) = (${x}, ${y})";
  }

  Point.origin()
      : x = 0,
        y = 0 {
    print("this is the origin");
  }

  factory Point.invert(double x, double y) {
    return Point(-1 / x, -1 / y);
  }

  Point operator +(Point other) => Point(x + other.x, y + other.y);
  Point operator -(Point other) => Point(x - other.x, y - other.y);

  @override
  bool operator ==(Object other) {
    return other is Point && x == other.x && y == other.y;
  }

  @override
  int get hashCode => Object.hash(x, y);

  @override
  double distanceTo(Point b) {
    return sqrt(pow(x - b.x, 2) + pow(y - b.y, 2));
  }
}

abstract class Operations {
  double distanceTo(Point b);
}
