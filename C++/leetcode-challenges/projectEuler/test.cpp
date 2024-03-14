#include <cstdio>
#include <cstdlib>

int main(int argc, char *argv[]) {
  int size = 5;
  int *arr = (int *)malloc(size * sizeof(int));

  for (int i = 0; i < size; i++) {
    printf("%i\n", arr[i]);
  }

  free(arr);

  return 0;
}
