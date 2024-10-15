import unittest


class MinHeap:
    def __init__(self, default: list[int] = []):
        self._arr = default

    def heapify(self):
        pass

    def insert(self, value: int):
        self._arr.append(value)

    def pop(self):
        pass

    def getMin(self):
        return self._arr[0]


class Solution(unittest.TestCase):
    def minGroups(self, intervals: list[list[int]]) -> int:
        return 0

    def test_example_1(self):
        self.assertEqual(3, self.minGroups([[5, 10], [6, 8], [1, 5], [2, 3], [1, 10]]))

    def test_example_2(self):
        self.assertEqual(1, self.minGroups([[1, 3], [5, 6], [8, 10], [11, 13]]))


unittest.main()
