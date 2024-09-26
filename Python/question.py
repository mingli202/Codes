import math
from decimal import Decimal


def p(n: int) -> float:
    if n == 2:
        return 2 * math.sqrt(2)

    return math.pow(2, n) * math.sqrt(
        2 * (1 - math.sqrt(1 - math.pow(p(n - 1) / math.pow(2, n), 2)))
    )


def p_decimal(n: int) -> Decimal:
    if n == 2:
        return 2 * Decimal.sqrt(2)

    return Decimal.__pow__(2, n)


for i in range(3, 35 + 1):
    print(f"n = {i} ->", p(i))
