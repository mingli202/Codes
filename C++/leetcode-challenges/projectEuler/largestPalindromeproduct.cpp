#include <array>
#include <cmath>
#include <format>
#include <iostream>
#include <map>
#include <stdbool.h>
#include <string>
#include <vector>

int largestPalindromeProduct() {
  int toReturn = 0;

  auto checkPalindrome = [](std::string number) -> bool {
    int left = 0;
    int right = number.size() - 1;

    while (left < right) {
      if (number[left] != number[right]) {
        return false;
      }

      left++;
      right--;
    }

    return true;
  };

  for (int i = 1000; i >= 900; i--) {
    for (int k = 1000; k >= 900; k--) {
      int product = i * k;

      if (checkPalindrome(std::to_string(product)) && product > toReturn)
        toReturn = product;
    }
  }

  return toReturn;
}

int main() {
  int answer = largestPalindromeProduct();
  std::string test = answer == 906609 ? "passed" : "failed";

  printf("%i (%s)", answer, test.c_str());

  return 0;
}