#include <iostream>
#include <stdbool.h>
#include <string>
#include <vector>

int nthPrime(int n) {
  int nth = 1;
  std::vector<int> primes = {2};

  for (int i = 2; nth <= n; i++) {
    bool isPrime = true;

    for (int prime : primes) {
      if (i % prime == 0) {
        isPrime = false;
        break;
      }
    }

    if (isPrime) {
      primes.push_back(i);
      nth++;
    }
  }

  return primes[n - 1];
}

int main() {

  int answer = nthPrime(10001);
  std::string test = answer == 104743 ? "passed" : "failed";
  std::cout << answer << '\n' << test << '\n'; // passed

  return 0;
}
