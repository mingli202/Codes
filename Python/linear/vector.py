import numpy as np
from decimal import Decimal


class Vector:
    def __init__(self, vector=[0]) -> None:
        self.vector = vector
        self.space = len(self.vector)

    def length(self):
        return Decimal(np.sqrt(sum([x**2 for x in self.vector])))
