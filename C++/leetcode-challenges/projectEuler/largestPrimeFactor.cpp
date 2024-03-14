#include <array>
#include <cmath>
#include <format>
#include <iostream>
#include <map>
#include <stdbool.h>
#include <string>
#include <vector>

int largestPrimeFactor(long n) {

  /**
   * 24;
   * i * n
   * 2 * 12
   * 2 * 6
   * 2 * 3
   * 3 * 1
   */

  long last = 2;

  // time: O(nlog(n))
  for (int i = 2; i < n; i++) {
    while (n % i == 0)
      n = n / i;

    last = n;
  }

  return last;
}

int main() {

  int answer = largestPrimeFactor(600851475143);
  std::string test = answer == 6857 ? "passed" : "failed";
  printf("%i (%s)", answer, test.c_str());

  return 0;
}