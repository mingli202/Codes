import unittest


class Solution(unittest.TestCase):
    def fullJustify(self, words: list[str], maxWidth: int) -> list[str]:
        arr = []

        start = 0
        length = 0
        for i, word in enumerate(words):
            if length + len(word) + i - start > maxWidth:
                s = ""

                if i - start > 1:
                    avg_space = (maxWidth - length) // (i - start - 1)
                    rem = (maxWidth - length) % (i - start - 1)

                    for k in range(start, i - 1):
                        s += words[k] + " " * avg_space + ("", " ")[rem > 0]
                        rem -= 1
                    s += words[i - 1]
                else:
                    s = words[i - 1] + " " * (maxWidth - len(words[i - 1]))

                arr.append(s)

                length = 0
                start = i

            length += len(word)

        s = " ".join(words[start:])

        arr.append(s + " " * (maxWidth - len(s)))

        return arr

    def test_case_1(self):
        self.assertEqual(
            ["This    is    an", "example  of text", "justification.  "],
            self.fullJustify(
                ["This", "is", "an", "example", "of", "text", "justification."], 16
            ),
        )

    def test_case_2(self):
        self.assertEqual(
            ["What   must   be", "acknowledgment  ", "shall be        "],
            self.fullJustify(
                words=["What", "must", "be", "acknowledgment", "shall", "be"],
                maxWidth=16,
            ),
        )

    def test_case_3(self):
        self.assertEqual(
            [
                "Science  is  what we",
                "understand      well",
                "enough to explain to",
                "a  computer.  Art is",
                "everything  else  we",
                "do                  ",
            ],
            self.fullJustify(
                words=[
                    "Science",
                    "is",
                    "what",
                    "we",
                    "understand",
                    "well",
                    "enough",
                    "to",
                    "explain",
                    "to",
                    "a",
                    "computer.",
                    "Art",
                    "is",
                    "everything",
                    "else",
                    "we",
                    "do",
                ],
                maxWidth=20,
            ),
        )


unittest.main()
