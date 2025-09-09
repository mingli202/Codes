import unittest
from typing import List
import bisect


class Solution(unittest.TestCase):
    def minimumTotalDistance(self, robot: List[int], factory: List[List[int]]) -> int:
        dist = 0
        factory.sort(key=lambda x: x[0])

        for r in robot:
            closest = bisect.bisect_left(factory, r, key=lambda x: x[0])

            d = -1

            if factory[closest][1] != 0:
                d = abs(factory[closest][0] - r)

            c = closest

            i = 1
            while i == 1 or d == -1:
                if (
                    closest - i >= 0
                    and factory[closest - i][1] != 0
                    and (
                        d == -1
                        or abs(factory[closest - i][0] - r) < d
                        and factory[closest - i][1] > factory[closest][1]
                    )
                ):
                    c = closest - i
                    d = abs(factory[closest - i][0] - r)

                if (
                    closest + i < len(factory)
                    and factory[closest + i][1] != 0
                    and (
                        d == -1
                        or abs(factory[closest - i][0] - r) < d
                        and factory[closest + i][1] > factory[closest][1]
                    )
                ):
                    c = closest + i
                    d = abs(factory[closest + i][0] - r)

                i += 1

            closest = c
            factory[closest][1] -= 1
            dist += d

        return dist

    def test_case_1(self):
        self.assertEqual(4, self.minimumTotalDistance([0, 4, 6], [[2, 2], [6, 2]]))

    def test_case_2(self):
        self.assertEqual(2, self.minimumTotalDistance([1, -1], [[-2, 1], [2, 1]]))

    def test_case_3(self):
        self.assertEqual(
            6,
            self.minimumTotalDistance(
                [9, 11, 99, 101],
                [[10, 1], [7, 1], [14, 1], [100, 1], [96, 1], [103, 1]],
            ),
        )


unittest.main()
