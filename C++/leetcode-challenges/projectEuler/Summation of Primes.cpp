#include <cmath>
#include <string>
#include <vector>

// brute force optimized
long solve(int target) {
  std::vector<int> primes = {2};
  long sum = 2;

  for (int n = 3; n < target; n++) {
    bool isPrime = true;

    for (int i = 0; primes[i] <= sqrt(n); i++) {
      if (n % primes[i] == 0) {
        isPrime = false;
        break;
      }
    }

    if (!isPrime) {
      continue;
    }

    primes.push_back(n);
    sum += n;
  }

  return sum;
}

int main(int argc, char *argv[]) {

  printf("%li\n", solve(10)); // 2 + 3 + 5 + 7 = 17
  printf("%li\n", solve(100));

  long answer = solve(2000000);
  std::string test = answer == 142913828922 ? "passed" : "failed";

  printf("%li (%s)\n", answer, test.c_str());

  return 0;
}
