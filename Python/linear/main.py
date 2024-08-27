from vector import *
from matrix import *
from functions import *
from decimal import Decimal


def main():
    A = Matrix([[1, 2, -2], [2, 3, -1], [0, -1, 3], [-1, 2, -10]])

    print(A.REF())
    print(A.RREF())


if __name__ == "__main__":
    main()
