import numpy as np
from copy import deepcopy
from decimal import Decimal


class Matrix:
    def __init__(self, matrix=[[0]]) -> None:
        self.matrix = matrix

        if not all([len(v) == len(matrix[0]) for v in self.matrix]):
            raise Exception("Your matrix is invalid")

        self.rows = len(self.matrix)
        self.cols = len(self.matrix[0])

    def determinant(self):
        if self.rows != self.cols:
            raise Exception("It is not a square matrix")

        def det(matrix) -> Decimal:
            if (len(matrix) == 1):
                return matrix[0][0]

            prod = 0
            for index, v in enumerate(matrix.pop(0)):
                mCopy = deepcopy(matrix)
                for arr in mCopy:
                    arr.pop(index)

                prod += v * det(mCopy) * (-1) ** (index)

            return Decimal(prod)

        return det(self.matrix)

    def REF(self):
        M: list[list] = deepcopy(self.matrix)

        row = 0
        col = 0
        counter = 0
        while row < self.rows and col < self.cols:
            # if row swap every row then all start with 0
            if counter == self.rows - row:
                counter = 0
                col += 1
                continue

            # make sure row doesn't start with a 0
            # if zero then row swap it to the end
            if M[row][col] == 0:
                swappedRow = M.pop(row)
                M.append(swappedRow)
                counter += 1
                continue

            # reduce row to have leading 1
            M[row] = [(v / M[row][col]) for v in M[row]]

            # row reduce rows below it
            for i in range(row + 1, self.rows):
                scalar = (M[i][col] / M[row][col])
                M[i] = [M[i][k] - (M[row][k] * scalar)
                        for k in range(self.cols)]
            row += 1
            col += 1
            counter = 0

        return M

    def RREF(self):
        M: list[list] = self.REF()

        row = self.rows - 1
        col = self.cols - 1

        while row > 0 and col > 0:
            if M[row][col] != 1:
                if col == 1:
                    col = self.cols - 1
                    row -= 1
                else:
                    col -= 1

                continue

            # substract from rows above it
            for i in range(0, row):
                scalar = M[i][col]
                M[i] = [M[i][k] - (M[row][k] * scalar)
                        for k in range(self.cols)]

            row -= 1
            col -= 1

        return M
