import unittest
from typing import List
import bisect


class Solution(unittest.TestCase):
    def minimumTotalDistance(self, robot: List[int], factory: List[List[int]]) -> int:
        def getNearest(
            factory: list[list[int]], robot: int, d: int, closest: int
        ) -> tuple[int, int]:
            c = closest
            i = 1
            while i == 1 or d == -1:
                for k in range(-1, 1, 2):
                    if (
                        closest + i * k >= 0
                        and factory[closest + i * k][1] != 0
                        and (
                            d == -1
                            or abs(factory[closest + i * k][0] - robot) < d
                            and factory[closest + i * k][1] > factory[closest][1]
                        )
                    ):
                        c = closest + i * k
                        d = abs(factory[closest + i * k][0] - robot)

                i += 1

            return d, c

        dist = 0
        factory.sort(key=lambda x: x[0])

        hashmap = {}

        for r in robot:
            closest = bisect.bisect_left(factory, r, key=lambda x: x[0])

            d, closest = getNearest(
                factory,
                r,
                abs(factory[closest][0] - r) if factory[closest][1] >= 0 else -1,
                closest,
            )

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


if __name__ == "__main__":
    unittest.main()
unittest.main()
