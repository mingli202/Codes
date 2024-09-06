class Solution:
    def strStr(self, haystack: str, needle: str) -> int:
        for i in range(len(haystack) - len(needle) + 1):
            if haystack[i : i + len(needle)] == needle:
                return i

        return -1

    def searchInsert(self, nums: list[int], target: int) -> int:
        return 0

    def missingRolls(self, rolls: list[int], mean: int, n: int) -> list[int]:
        debt = mean * (len(rolls) + n) - sum(rolls)
        avg_roll = debt // n
        remainder = debt % n

        if avg_roll > 6 or avg_roll < 1:
            return []

        result = [avg_roll] * n
        max_give = 6 - avg_roll

        if remainder > max_give * n:
            return []

        for i in range(len(result)):
            if remainder <= 0:
                break

            give_count = min(max_give, remainder)
            remainder -= give_count
            result[i] += give_count

        return result


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


if __name__ == "__main__":
    s = Solution()

    test(0, s.strStr("sadbutsad", "sad"))
    test(-1, s.strStr("leetcode", "leeto"))
    test(2, s.strStr("hello", "ll"))
    test(0, s.strStr("a", "a"))
    test(0, s.strStr("aaa", "aaa"))
    test(4, s.strStr("mississippi", "issip"))
