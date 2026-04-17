using TesterLib;

namespace Polynomials;

#pragma warning disable CA1822, IDE0051
class ExamplesPolynomial
{
    // Your tests go here!
    bool TestMonomialConstructor(Tester t)
    {
        return t.CheckConstructorExceptionType(typeof(ArgumentException), typeof(Monomial), 1, -1)
            && t.CheckConstructorNoException(typeof(Monomial), 1, 2);
    }

    bool TestPolynomialConstructor(Tester t)
    {
        var monomials = FromArray([(1, 2), (2, 3), (3, 4)]);
        var nonUniqueExponents = FromArray([(1, 3), (2, 2), (3, 4), (5, 3)]);

        return t.CheckConstructorExceptionType(
                typeof(ArgumentException),
                typeof(Polynomial),
                nonUniqueExponents
            ) && t.CheckConstructorNoException(typeof(Polynomial), monomials);
    }

    bool TestMonomialEvaluate(Tester t)
    {
        var monomial = new Monomial(2, 3);
        return t.CheckExpect(monomial.Evaluate(2), (int)(2 * Math.Pow(2, 3)));
    }

    bool TestPolynomialEvaluate(Tester t)
    {
        var monomials = FromArray([(1, 2), (2, 3), (3, 4)]);
        var polynomial = new Polynomial(monomials);
        return t.CheckExpect(
            polynomial.Evaluate(2),
            (int)(1 * Math.Pow(2, 2) + 2 * Math.Pow(2, 3) + 3 * Math.Pow(2, 4))
        );
    }

    bool TestNormalize(Tester t)
    {
        var monomials = FromArray([(4, 2), (1, 4), (0, 12), (5, 1), (6, 8), (0, 345)]).Normalize();
        var expected = FromArray([(5, 1), (4, 2), (1, 4), (6, 8)]);
        return t.CheckExpect(monomials, expected);
    }

    bool TestAnyAll(Tester t)
    {
        var monomials = FromArray([(1, 2), (2, 3), (3, 4)]);
        var empty = new EmptyLoMonomial();

        return t.CheckExpect(monomials.Any(m => m.CompareTo(new Monomial(0, 3)) == 0), true)
            && t.CheckExpect(monomials.Any(m => m.SameCoefficient(99)), false)
            && t.CheckExpect(monomials.All(m => m.CompareTo(new Monomial(0, 1)) > 0), true)
            && t.CheckExpect(monomials.All(m => m.SameCoefficient(1)), false)
            && t.CheckExpect(empty.Any(_ => true), false)
            && t.CheckExpect(empty.All(_ => false), true);
    }

    bool TestHasUniqueExponents(Tester t)
    {
        var unique = FromArray([(1, 1), (2, 3), (3, 5)]);
        var duplicates = FromArray([(1, 1), (2, 3), (3, 1)]);
        var empty = new EmptyLoMonomial();

        return t.CheckExpect(unique.HasUniqueExponents(), true)
            && t.CheckExpect(duplicates.HasUniqueExponents(), false)
            && t.CheckExpect(empty.HasUniqueExponents(), true);
    }

    bool TestInsertInSorted(Tester t)
    {
        var baseList = FromArray([(2, 2), (4, 4)]);

        var insertMiddle = baseList.InsertInSorted(new Monomial(3, 3));
        var expectedMiddle = FromArray([(2, 2), (3, 3), (4, 4)]);

        var insertHead = baseList.InsertInSorted(new Monomial(1, 1));
        var expectedHead = FromArray([(1, 1), (2, 2), (4, 4)]);

        var insertTail = baseList.InsertInSorted(new Monomial(5, 5));
        var expectedTail = FromArray([(2, 2), (4, 4), (5, 5)]);

        var insertEmpty = new EmptyLoMonomial().InsertInSorted(new Monomial(7, 7));
        var expectedEmpty = FromArray([(7, 7)]);

        return t.CheckExpect(insertMiddle, expectedMiddle)
            && t.CheckExpect(insertHead, expectedHead)
            && t.CheckExpect(insertTail, expectedTail)
            && t.CheckExpect(insertEmpty, expectedEmpty);
    }

    bool TestAdd1(Tester t)
    {
        var monomials = FromArray([(1, 2), (2, 3), (3, 4)]).Add(new Monomial(10, 3));
        var expected = FromArray([(1, 2), (12, 3), (3, 4)]);
        return t.CheckExpect(monomials, expected);
    }

    bool TestAdd2(Tester t)
    {
        var monomials = FromArray([(1, 2), (2, 3), (3, 4)]).Add(new Monomial(10, 5));
        var expected = FromArray([(1, 2), (2, 3), (3, 4), (10, 5)]);
        return t.CheckExpect(monomials, expected);
    }

    bool TestAddAll(Tester t)
    {
        var monomials1 = FromArray([(1, 2), (3, 4), (1, 10)]);
        var monomials2 = FromArray([(10, 4), (20, 3), (1, 1)]);
        var expected = FromArray([(1, 1), (1, 2), (20, 3), (13, 4), (1, 10)]);
        return t.CheckExpect(monomials1.AddAll(monomials2).Normalize(), expected.Normalize());
    }

    bool TestAddEmpty(Tester t)
    {
        var empty = new EmptyLoMonomial();
        var expected = FromArray([(5, 2)]);
        return t.CheckExpect(empty.Add(new Monomial(5, 2)), expected);
    }

    bool TestListEquals(Tester t)
    {
        var list1 = FromArray([(1, 1), (2, 2)]);
        var list2 = FromArray([(1, 1), (2, 2)]);
        var list3 = FromArray([(1, 1), (2, 3)]);
        var listDifferentOrder = FromArray([(2, 2), (1, 1)]);
        var empty1 = new EmptyLoMonomial();
        var empty2 = new EmptyLoMonomial();

        return t.CheckExpect(list1.Equals(list2), true)
            && t.CheckExpect(list1.Equals(list3), false)
            && t.CheckExpect(list1.Equals(listDifferentOrder), false)
            && t.CheckExpect(list1.Equals(empty1), false)
            && t.CheckExpect(empty1.Equals(empty2), true);
    }

    bool TestMultiplyEmpty(Tester t)
    {
        var empty = new EmptyLoMonomial();
        var monomials = FromArray([(2, 1), (1, 0)]);
        var monomial = new Monomial(3, 2);

        return t.CheckExpect(empty.Multiply(monomial), new EmptyLoMonomial())
            && t.CheckExpect(empty.MultiplyAll(monomials), new EmptyLoMonomial())
            && t.CheckExpect(monomials.MultiplyAll(empty), new EmptyLoMonomial())
            && t.CheckExpect(empty.Evaluate(5), 0)
            && t.CheckExpect(monomials.Evaluate(2), 2 * (int)Math.Pow(2, 1) + 1);
    }

    bool TestPolynomialAdd(Tester t)
    {
        var polynomial = new Polynomial(FromArray([(1, 2), (2, 5), (3, 4)]));
        var other = new Polynomial(FromArray([(10, 5), (20, 3), (1, 1)]));

        var expected = new Polynomial(FromArray([(1, 1), (1, 2), (20, 3), (3, 4), (12, 5)]));
        return t.CheckExpect(polynomial.Add(other), expected);
    }

    bool TestEmptyPolynomial(Tester t)
    {
        var empty = new Polynomial();
        return t.CheckExpect(empty.Evaluate(10), 0);
    }

    bool TestPolynomialMultiply(Tester t)
    {
        var left = new Polynomial(FromArray([(2, 0), (1, 1)]));
        var right = new Polynomial(FromArray([(4, 0), (3, 2)]));
        var expected = new Polynomial(FromArray([(8, 0), (4, 1), (6, 2), (3, 3)]));

        var zero = new Polynomial();
        return t.CheckExpect(left.Multiply(right), expected)
            && t.CheckExpect(left.Multiply(zero), zero)
            && t.CheckExpect(zero.Multiply(right), zero);
    }

    bool TestSamePolynomial(Tester t)
    {
        var first = new Polynomial(FromArray([(1, 2), (2, 0)]));
        var sameDifferentOrder = new Polynomial(FromArray([(2, 0), (1, 2)]));
        var different = new Polynomial(FromArray([(1, 2), (3, 0)]));
        var empty1 = new Polynomial();
        var empty2 = new Polynomial(new EmptyLoMonomial());

        return t.CheckExpect(first.SamePolynomial(sameDifferentOrder), true)
            && t.CheckExpect(first.SamePolynomial(different), false)
            && t.CheckExpect(empty1.SamePolynomial(empty2), true);
    }

    bool TestMultiply(Tester t)
    {
        var monomals = FromArray([(1, 2), (2, 3), (3, 4)]).Multiply(new Monomial(10, 5));
        var expected = FromArray([(10, 7), (20, 8), (30, 9)]);
        return t.CheckExpect(monomals, expected);
    }

    bool TestMultiplyAll(Tester t)
    {
        var monomials1 = FromArray([(1, 2), (3, 4)]);
        var monomials2 = FromArray([(10, 4), (20, 3), (1, 1)]);
        var expected = FromArray([(10, 6), (23, 5), (1, 3), (30, 8), (60, 7)]);
        return t.CheckExpect(monomials1.MultiplyAll(monomials2).Normalize(), expected.Normalize());
    }

    /// <summary>
    /// Helper function to create a monomial from an array of (coefficient, exponent) tuples.
    /// </summary>
    private static ILoMonomial FromArray((int coefficient, int exponent)[] array)
    {
        ILoMonomial result = new EmptyLoMonomial();
        foreach (var (coefficient, exponent) in array.Reverse())
        {
            result = new LinkLoMonomial(new Monomial(coefficient, exponent), result);
        }
        return result;
    }
}

