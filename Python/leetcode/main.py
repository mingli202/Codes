class Solution:
    def solve(self, nums: list[int], val: int) -> int:
        left = 0
        right = len(nums)

        while left < right:
            if nums[left] == val:
                while right == len(nums) or nums[right] == val and left < right:
                    right -= 1

                [nums[left], nums[right]] = [nums[right], nums[left]]

            left += 1

        return left


OKGREEN = "\033[92m"
FAIL = "\033[91m"
ENDC = "\033[0m"


def test(lhs, rhs):
    if lhs == rhs:
        print(f"{OKGREEN}Ok{ENDC}")

    else:
        print(f"{FAIL}Fail{ENDC}")
        print("lhs:", lhs)
        print("rhs:", rhs)


def t(a, k, b):
    if len(a) != k:
        print(f"{FAIL}Fail{ENDC}")
        print("lhs:", a)
        print("rhs:", b)
        print("k:", k)
        return

    for i in range(k):
        if a[i] != b[i]:
            print(f"{FAIL}Fail{ENDC}")
            print("lhs:", a)
            print("rhs:", b)
            print("k:", k)
            return

    print(f"{OKGREEN}Ok{ENDC}")


if __name__ == "__main__":
    s = Solution()

    answer = []
    li = [2]
    r = s.solve(nums=li, val=2)

    li = li[0:r]
    li.sort()
    answer.sort()
    t(answer, r, li)
