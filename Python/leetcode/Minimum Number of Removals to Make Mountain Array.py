import unittest as ut
from typing import List
from bisect import bisect_left


class Solution(ut.TestCase):
    def minimumMountainRemovals(self, nums: List[int]) -> int:
        def lengthOfLIS(self, nums: list[int]) -> int:
            sub: list[int] = []

            for n in nums:
                i = bisect_left(sub, n)

                if i == len(sub):
                    sub.append(n)
                else:
                    sub[i] = n

            return len(sub)

        return 0

    def test_case_1(self):
        self.assertEqual(0, self.minimumMountainRemovals([1, 3, 1]))

    def test_case_2(self):
        self.assertEqual(3, self.minimumMountainRemovals([2, 1, 1, 5, 6, 2, 3, 1]))


ut.main()
