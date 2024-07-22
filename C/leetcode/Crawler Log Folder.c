#include <stdio.h>
#include <string.h>

int minOperations(char **logs, int logsSize) {
  int deep = 0;

  for (int i = 0; i < logsSize; i++) {
    if (strcmp(logs[i], "./") == 0) {
      continue;
    }
    if (strcmp(logs[i], "../") == 0) {
      if (deep > 0) {
        deep--;
      }
    } else {
      deep++;
    }
  }

  return deep;
}

int main(void) {
  char *a[] = {"d1/", "d2/", "../", "d21/", "./"};
  printf("expected: %i; got: %i\n", 2, minOperations(a, 5));

  char *b[] = {"d1/", "d2/", "./", "d3/", "../", "d31/"};
  printf("expected: %i; got: %i\n", 3, minOperations(b, 6));

  char *c[] = {"d1/", "../", "../", "../"};
  printf("expected: %i; got: %i\n", 0, minOperations(c, 4));

  return 0;
}
