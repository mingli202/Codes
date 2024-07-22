#include <stdio.h>

void hello() {
  printf("Hello world\n");
  printf("This is what functions are about\n");
}

long long factorial(int n) {
  if (n == 0) {
    return 1;
  }

  long prod = 1;
  for (int i = 1; i <= n; i++) {
    prod *= i;
  }
  return prod;
}

int main(int argc, char *argv[]) {
  printf("Hello world");

  return 0;
}
