from vector import *
from matrix import *
from decimal import Decimal
from copy import deepcopy


def dotProduct(u: Vector, v: Vector) -> Decimal:
    if u.space != v.space:
        raise Exception("Vectors are not of the same vector space")

    sum = 0
    for i, u_n in enumerate(u.vector):
        sum += u_n * v.vector[i]

    return Decimal(sum)


def crossProduct(u: Vector, v: Vector) -> Vector:
    if u.space != 3 or v.space != 3:
        raise Exception("Vectors are not in R^3")

    def fn(i: int) -> Decimal:
        index = 0 if i > 1 else i + 1

        subU = deepcopy(u.vector)
        subV = deepcopy(v.vector)

        subU.pop(index)
        subV.pop(index)

        M = Matrix([[subU[k], subV[k]] for k in range(2)])

        return M.determinant()

    toReturn = [fn(i) for i in range(3)]

    return Vector(toReturn)


def vectorOperation(u: Vector, v: Vector, operation: str) -> Vector:
    if u.space != v.space:
        raise Exception("Vectors are not of the same vector space")

    length = u.space

    placeholder = []
    if operation == "+":
        placeholder = [u.vector[i] + v.vector[i] for i in range(length)]
    elif operation == "-":
        placeholder = [u.vector[i] - v.vector[i] for i in range(length)]
    else:
        raise Exception("invalid operation")

    return Vector(placeholder)


def projection(ofVectorA: Vector, ontoVectorB: Vector) -> Vector:
    u = ofVectorA
    v = ontoVectorB
    scalar = Decimal(dotProduct(u, v)) / Decimal(dotProduct(v, v))
    proj = Vector([i * scalar for i in v.vector])

    return proj


def perpendicular(ofVectorA: Vector, ontoVectorB: Vector) -> Vector:
    u = ofVectorA
    v = ontoVectorB

    proj = projection(u, v)
    perp = Vector([u.vector[index] - value for index,
                  value in enumerate(proj.vector)])

    return perp


def matrixMultiMatrix(matrixA: Matrix, withMatrixB: Matrix) -> Matrix:
    A = matrixA
    B = withMatrixB

    if A.cols != B.rows:
        raise Exception("not multiplicable")

    tmp = [
        [
            sum([A.matrix[i][l]*B.matrix[l][k]
                 for l in range(A.cols)]) for k in range(B.cols)
        ] for i in range(A.rows)
    ]

    return Matrix(tmp)


def matrixMultiVector(matrixA: Matrix, withVectorB: Vector) -> Vector:
    A = matrixA
    B = withVectorB

    if A.cols != B.space:
        raise Exception("not multiplicable")

    tmp = [
        sum([A.matrix[i][k] * B.vector[k] for k in range(A.cols)]) for i in range(A.rows)
    ]

    return Vector(tmp)


def solve(matrixA: Matrix, augmentedVectorB: Vector) -> Vector | None:
    A = matrixA
    b = augmentedVectorB

    if A.cols != b.space:
        raise Exception("Cannot augment matrix A with vector b")
