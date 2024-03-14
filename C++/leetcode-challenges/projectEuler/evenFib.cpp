#include <array>
#include <cmath>
#include <format>
#include <iostream>
#include <map>
#include <stdbool.h>
#include <string>
#include <vector>

// time: O(n)
// space: O(n)
int evenFib(int n) {
  std::vector<int> table = {1, 0};
  int sum = 0;

  for (int i = 0; table[i] <= n; i++) {
    if (table[i] % 2 == 0)
      sum += table[i];

    table.push_back(0);

    table[i + 1] += table[i];

    table[i + 2] += table[i];
  }

  return sum;
}

int main() {
  int answer = evenFib(4000000);
  std::string test = answer == 4613732 ? "passed" : "failed";

  printf("%i (%s)", answer, test.c_str());

  return 0;
}