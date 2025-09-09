import unittest


class MiniHeap:
    def __init__(self, a: int, b: int, c: int):
        self._arr: list[tuple[int, str]] = [(a, "a"), (b, "b"), (c, "c")]

        if self._arr[0][0] < self._arr[1][0]:
            (self._arr[0], self._arr[1]) = (self._arr[1], self._arr[0])

        if self._arr[0][0] < self._arr[2][0]:
            (self._arr[0], self._arr[2]) = (self._arr[2], self._arr[0])

    def heapify(self):
        largest = 0

        if self._arr[largest][0] < self._arr[1][0]:
            largest = 1

        if self._arr[largest][0] < self._arr[2][0]:
            largest = 2

        if largest != 0:
            (self._arr[largest], self._arr[0]) = (self._arr[0], self._arr[largest])

    def decrement(self):
        # why are python tuples are immutable
        count, letter = self._arr[0]
        self._arr[0] = (count - 1, letter)

        self.heapify()

    def get_max(self) -> str:
        return self._arr[0][1]

    def get_second_and_decrement(self) -> str | None:
        second = 1

        if self._arr[2][0] > self._arr[second][0]:
            second = 2

        count, letter = self._arr[second]

        if count == 0:
            return None

        self._arr[second] = (count - 1, letter)

        return letter


class Solution(unittest.TestCase):
    def longestDiverseString(self, a: int, b: int, c: int) -> str:
        heap = MiniHeap(a, b, c)
        s = ""

        last: str = ""
        count = 0

        for i in range(a + b + c):
            ch = heap.get_max()

            if ch == last and count == 2:
                _c = heap.get_second_and_decrement()

                if _c:
                    ch = _c
                    count = 0
                else:
                    break

            else:
                heap.decrement()
                count += 1

            last = ch
            s += ch

        return s

    def test_example_1(self):
        self.assertEqual("ccbccacc", self.longestDiverseString(1, 1, 7))

    def test_example_2(self):
        self.assertEqual("aabaa", self.longestDiverseString(7, 1, 0))


unittest.main()
