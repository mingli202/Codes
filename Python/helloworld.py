import math

n = 3


def gen(i) -> list[int]:
    k = 2**n

    out = []

    while k > 1:
        out.append(math.floor(i % k / (k / 2)))
        k /= 2

    return out


out = [gen(i) for i in range(0, 2**n)]


def f(x: int, y: int, z: int) -> int:
    return int(x and (not z or not y))


for x, y, z in out:
    print(f(x, y, z))


a = {i for i in range(-4, 8)}
b = {i for i in range(3, 12)}

c = a
print(c)
print(len(c))

n = 12

for k in range(0, n + 1):
    print(f"{k}!/({n}!*{n - k}!)")
