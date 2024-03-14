#include <array>
#include <cmath>
#include <format>
#include <iostream>
#include <map>
#include <stdbool.h>
#include <string>
#include <vector>

// time O(n)
long multipleOf3Or5(int n) {
  long sum = 0;

  for (int i = 0; i < n; i++) {
    if (i % 3 == 0 || i % 5 == 0) {
      sum += i;
    }
  }

  return sum;
}

int main() {

  printf("%li\n", multipleOf3Or5(10)); // 23

  long answer = multipleOf3Or5(1000);
  std::string test = answer == 233168 ? "passed" : "failed";

  printf("%li  (%s)\n", multipleOf3Or5(1000), test.c_str()); // 233168

  return 0;
}