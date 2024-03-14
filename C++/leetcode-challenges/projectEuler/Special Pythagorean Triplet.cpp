#include <cmath>
#include <cstdio>
#include <string>

int solve(int target) {
  for (int a = 1; a < target - 1; a++) {
    for (int b = a + 1; b < target; b++) {
      double c = sqrt(a * a + b * b);

      double sum = a + b + c;

      if (sum == target) {
        printf("%i, %i, %f \n", a, b, c);
        return a * b * c;
      }
    }
  }

  return 0;
}

int main(int argc, char *argv[]) {

  int ans = solve(1000);
  std::string test = ans == 31875000 ? "Passed" : "Failed";

  printf("%i, (%s)", ans, test.c_str());
  return 0;
}
