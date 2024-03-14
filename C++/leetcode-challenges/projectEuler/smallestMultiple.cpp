#include <array>
#include <cmath>
#include <format>
#include <iostream>
#include <map>
#include <stdbool.h>
#include <string>
#include <vector>

// time: O(n^2)
// space: O(n)
// takes less than 0.1 seconds
long smallestMultiple(const int n) {
  // array of factors
  std::vector<int> table = {1};

  // initial value
  long product = 1;

  // every number can be factored with primes
  // loop through 1 to 20 to find all common divisors
  for (int i = 1; i <= n; i++) {
    int num = i;
    // divide by all its exiting divisors
    for (int k : table) {
      // check if k is a divisor of num
      if (num % k == 0)
        num /= k;
    }

    // if num can't be made from existing divisors
    // then is has a remaining prime factor
    // add to prime divisors
    if (num != 1) {
      table.push_back(num);
      product *= num;
    }
  }

  // number from 1 to 20 can be made from some combinasion of these factors
  // [1, 2, 3, 2, 5, 7, 2, 3, 11, 13, 2, 17, 19]
  printf("[");
  for (int i = 0; i < table.size(); i++) {
    if (i != 0) {
      printf(",");
    }
    printf("%i", table[i]);
  }
  printf("]\n");

  return product;
}

int main() {
  long answer = smallestMultiple(20);
  std::string test = answer == 232792560 ? "passed" : "failed"; // passed

  printf("%li (%s)", answer, test.c_str());

  return 0;
}