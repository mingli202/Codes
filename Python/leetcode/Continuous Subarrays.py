import unittest


class Solution(unittest.TestCase):
    def continuousSubarrays(self, nums: list[int]) -> int:
        hashmap: dict[int, int] = {nums[0]: 1}

        left = 0
        right = 0

        n = 0

        smallest = nums[0]
        largest = nums[0]

        while right < len(nums):
            if nums[right] < largest - 2 or nums[right] > smallest + 2:
                n += right - left

                hashmap[nums[left]] -= 1
                if hashmap[nums[left]] == 0:
                    del hashmap[nums[left]]

                left += 1

                largest = max(hashmap.keys())
                smallest = min(hashmap.keys())

            else:
                right += 1

                if right >= len(nums):
                    break

                largest = max(largest, nums[right])
                smallest = min(smallest, nums[right])

                if nums[right] not in hashmap:
                    hashmap[nums[right]] = 0

                hashmap[nums[right]] += 1

        n += (right - left) * (right - left + 1) // 2

        return n

    def test_case_1(self):
        self.assertEqual(self.continuousSubarrays([5, 4, 2, 4]), 8)

    def test_case_2(self):
        self.assertEqual(self.continuousSubarrays([1, 2, 3]), 6)


unittest.main()

"""

max = 4
min = 2

hashmap: {
    2: 1
}

n = 3 + 2 + 1

5 4 5 2 4
      L
          R

"""
