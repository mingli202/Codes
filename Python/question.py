import math


def p(n: int, memo: dict = {}) -> float:
    if n in memo:
        return memo[n]

    if n == 2:
        return 2 * math.sqrt(2)

    memo[n] = p(n - 1) * math.sqrt(
        2 / (1 + math.sqrt(1 - math.pow(p(n - 1) / math.pow(2, n - 1), 2)))
    )
    return memo[n]


for i in range(3, 35 + 1):
    print(f"n = {i} ->", p(i))
