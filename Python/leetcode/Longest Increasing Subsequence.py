import unittest as ut
from bisect import bisect_left


class Solution(ut.TestCase):
    def lengthOfLIS(self, nums: list[int]) -> int:
        sub: list[int] = []

        for n in nums:
            i = bisect_left(sub, n)

            if i == len(sub):
                sub.append(n)
            else:
                sub[i] = n

        return len(sub)

    def test_case_1(self):
        self.assertEqual(4, self.lengthOfLIS([10, 9, 2, 5, 3, 7, 101, 18]))

    def test_case_2(self):
        self.assertEqual(4, self.lengthOfLIS([0, 1, 0, 3, 2, 3]))

    def test_case_3(self):
        self.assertEqual(1, self.lengthOfLIS([7, 7, 7, 7, 7, 7, 7]))


ut.main()
