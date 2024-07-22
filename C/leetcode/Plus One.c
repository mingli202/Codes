#include <stdio.h>
#include <stdlib.h>

/**
 * Note: The returned array must be malloced, assume caller calls free().
 */
int *plusOne(int *digits, int digitsSize, int *returnSize) {
  int carry = 1;
  for (int i = digitsSize - 1; i >= 0; i--) {
    digits[i] += carry;

    if (digits[i] == 10) {
      digits[i] = 0;
      carry = 1;
    } else {
      *returnSize = digitsSize;
      return digits;
    }
  }

  *returnSize = digitsSize + 1;
  int *addedOne = (int *)malloc(sizeof(int) * (*returnSize));
  addedOne[0] = 1;

  for (int i = digitsSize - 1; i >= 0; i--) {
    addedOne[i + 1] = digits[i];
  }

  return addedOne;
}

void test(int *nums, int size) {
  for (int i = 0; i < size; i++) {
    printf("%i ", nums[i]);
  }
  printf("\n");
}

int main(void) {
  int input[] = {1, 2, 3};
  int size = 3;
  test(plusOne(input, 3, &size), 3);

  int in2[] = {9, 9};
  test(plusOne(in2, 2, &size), 3);

  return 0;
}
