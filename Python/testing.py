import numpy as np
from matplotlib import pyplot as plt
from sys import argv, exit


def divisors(n):
    l = []
    for i in range(1, n + 1):
        if n % i == 0:
            l.append(i)
    return l


def is_prime(n):
    if len(divisors(n)) == 2:
        return True
    else:
        return False


def prime_until(n):
    l = [1]
    for i in range(1, n + 1):
        if is_prime(i):
            l.append(i)
    return l


def palindrom():
    z = 0
    l = []
    for i in range(100, 1000):
        for j in range(100, 1000):
            z = i * j
            z = str(z)
            t = len(z)
            if z[0] == z[t - 1]:
                if z[1] == z[t - 2]:
                    if z[2] == z[t - 3]:
                        l.append(int(z))
    print(max(l))


def nth_prime(num):
    prime = [2, 3]
    n = 3
    k = 2

    while True:
        for i in range(num):
            if n % prime[i] == 0:
                break
            if i == len(prime) - 1:
                prime.append(n)
                k += 1
                break
        n += 1
        if k == num:
            break
    print(prime[num - 1])


def draw():
    x = np.linspace(-np.pi, np.pi, 1000)
    y = np.sin(x)

    plt.plot(x, y)
    plt.show()


def cliThings():
    if len(argv) == 1:
        print("Missing command line argument")
        exit(1)

    for i in argv[1:]:
        print(i)

    exit(0)


def numbers():
    number = [4, 7, 5, 2, 2, 1, 5, 8, 0, 7, 5, 3]

    n = 0
    if n in number:
        print("Found")
        exit(0)
    else:
        exit(1)


if __name__ == "__main__":
    numbers()
