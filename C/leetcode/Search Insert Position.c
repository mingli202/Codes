#include <stdio.h>

int searchInsert(int *nums, int numsSize, int target) {
  int low = 0;
  int high = numsSize - 1;

  if (target > nums[high]) {
    return high + 1;
  }

  while (low <= high) {
    int mid = (low + high) / 2;

    if (nums[mid] == target) {
      return mid;
    }

    if (target < nums[mid]) {
      high = mid - 1;
    } else {
      low = mid + 1;
    }
  }

  return low;
}

int main(void) {
  int a[] = {1, 3, 5, 6};
  printf("expected: %i, got: %i\n", 2, searchInsert(a, 4, 5));

  int b[] = {1, 3, 5, 6};
  printf("expected: %i, got: %i\n", 1, searchInsert(b, 4, 2));

  int c[] = {1, 3, 5, 6};
  printf("expected: %i, got: %i\n", 4, searchInsert(c, 4, 7));

  return 0;
}
