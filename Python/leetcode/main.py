class Solution:
    def strStr(self, haystack: str, needle: str) -> int:
        for i in range(len(haystack) - len(needle) + 1):
            if haystack[i : i + len(needle)] == needle:
                return i

        return -1

    def searchInsert(self, nums: List[int], target: int) -> int:
        pass


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
