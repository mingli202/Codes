namespace Polynomials;

public class Monomial : IExpression, IComparable<Monomial>
{
    private readonly int coefficient;
    private readonly int degree;

    public Monomial(int coefficient, int degree)
    {
        if (degree < 0)
        {
            throw new ArgumentException("Exponent must be non-negative");
        }
        this.coefficient = coefficient;
        this.degree = degree;
    }

    /// <summary>
    /// Returns true if the exponents are the same.
    /// </summary>
    public bool SameExponent(Monomial other) => degree == other.degree;

    /// <summary>
    /// Returns true if the coefficients are the same.
    /// </summary>
    public bool SameCoefficient(int coefficient) => this.coefficient == coefficient;

    /// <summary>
    /// Returns the evaluation of the monomial at x.
    /// </summary>
    public int Evaluate(int x) => coefficient * (int)Math.Pow(x, degree);

    /// <summary>
    /// Compares the exponents of two monomials.
    /// </summary>
    public int CompareTo(Monomial? other) => degree.CompareTo(other?.degree);

    /// <summary>
    /// Adds two monomials together. Assumes both monomials have the same exponent.
    /// </summary>
    public Monomial Add(Monomial other) => new(coefficient + other.coefficient, degree);

    /// <summary>
    /// Returns true if the monomials are equal.
    /// </summary>
    public bool Equals(Monomial other) =>
        other.coefficient == coefficient && other.degree == degree;

    /// <summary>
    /// Multiplies this monomial by another monomial.
    /// </summary>
    public Monomial Multiply(Monomial other) =>
        new(coefficient * other.coefficient, degree + other.degree);
}
