namespace Polynomials;

public class Polynomial : IExpression
{
    private readonly ILoMonomial monomials;

    public Polynomial(ILoMonomial monomials)
    {
        if (!monomials.HasUniqueExponents())
        {
            throw new ArgumentException("Monomials must have unique exponents");
        }

        this.monomials = monomials.Normalize();
    }

    public Polynomial(): this(new EmptyLoMonomial()) { }

    /// <summary>
    /// Returns the evaluation of the polynomial at x.
    /// </summary>
    public int Evaluate(int x) => monomials.Evaluate(x);

    /// <summary>
    /// Adds two polynomials together.
    /// </summary>
    public Polynomial Add(Polynomial other) => new(monomials.AddAll(other.monomials));

    /// <summary>
    /// Multiplies two polynomials together.
    /// </summary>
    public Polynomial Multiply(Polynomial other) => new(monomials.MultiplyAll(other.monomials));

    /// <summary>
    /// Returns true if the polynomials are equal.
    /// </summary>
    public bool SamePolynomial(Polynomial other) => other.monomials.Equals(monomials);
}
