#include <cstdio>
#include <stdbool.h>
#include <vector>

double findMedianSortedArrays(std::vector<int> &nums1,
                              std::vector<int> &nums2) {

  int maxLen = nums1.size() + nums2.size();

  int a = 0, b = 0, val1 = 0, val2 = 0;

  char isEven = maxLen % 2;

  for (int i = 0; i <= maxLen / 2; i++) {
    if (a == nums1.size() || (b != nums2.size() && nums2[b] <= nums1[a])) {

      if (i == maxLen / 2)
        val2 = nums2[b];

      if (i == maxLen / 2 - 1)
        val1 = nums2[b];

      b++;
    } else if (b == nums2.size() ||
               (a != nums1.size() && nums1[a] <= nums2[b])) {

      if (i == maxLen / 2)
        val2 = nums1[a];

      if (i == maxLen / 2 - 1)
        val1 = nums1[a];

      a++;
    }
  }
  return isEven == 0 ? (val1 + val2) / 2.00 : val2;
}

int main() {
  // test case
  std::vector<int> a1 = {1, 3};
  std::vector<int> a2 = {2, 5};
  printf("%f", findMedianSortedArrays(a1, a2));

  return 0;
}
