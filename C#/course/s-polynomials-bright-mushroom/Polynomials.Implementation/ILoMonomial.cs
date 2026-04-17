namespace Polynomials;

public interface ILoMonomial : IExpression
{
    /// <summary>
    /// Returns true if any of the monomials in the list match the predicate.
    /// </summary>
    bool Any(Func<Monomial, bool> predicate);

    /// <summary>
    /// Returns true if all of the monomials in the list match the predicate.
    /// </summary>
    bool All(Func<Monomial, bool> predicate);

    /// <summary>
    /// Returns true if the list contains only unique exponents.
    /// </summary>
    bool HasUniqueExponents();

    /// <summary>
    /// Returns a sorted version of the list of monomials and filters out zero coefficients.
    /// </summary>
    ILoMonomial Normalize();

    /// <summary>
    /// Inserts a monomial into the sorted list. Keeps the list sorted.
    /// </summary>
    ILoMonomial InsertInSorted(Monomial monomial);

    /// <summary>
    /// Add a monomial to the list. Does not preserve normalization.
    /// </summary>
    ILoMonomial Add(Monomial monomial);

    /// <summary>
    /// Add all monomials in the other list to this list.
    /// </summary>
    ILoMonomial AddAll(ILoMonomial other);

    /// <summary>
    /// Returns true both lists are equal.
    /// </summary>
    bool Equals(ILoMonomial other);
    bool EqualsLink(LinkLoMonomial other);
    bool EqualsEmpty(EmptyLoMonomial other);

    /// <summary>
    /// Multiplies this list by a monomial.
    /// </summary>
    ILoMonomial Multiply(Monomial monomial);

    /// <summary>
    /// Returns this list multiplied by the other list.
    /// </summary>
    ILoMonomial MultiplyAll(ILoMonomial other);
}

public class LinkLoMonomial(Monomial first, ILoMonomial rest) : ILoMonomial
{
    private readonly Monomial first = first;
    private readonly ILoMonomial rest = rest;

    public bool Any(Func<Monomial, bool> predicate) => predicate(first) || rest.Any(predicate);

    public bool All(Func<Monomial, bool> predicate) => predicate(first) && rest.All(predicate);

    public bool HasUniqueExponents() =>
        rest.All(m => !m.SameExponent(first)) && rest.HasUniqueExponents();

    public ILoMonomial Normalize() =>
        first.SameCoefficient(0) ? rest.Normalize() : rest.Normalize().InsertInSorted(first);

    public ILoMonomial InsertInSorted(Monomial monomial) =>
        monomial.CompareTo(first) < 0
            ? new LinkLoMonomial(monomial, this)
            : new LinkLoMonomial(first, rest.InsertInSorted(monomial));

    public ILoMonomial Add(Monomial monomial) =>
        monomial.SameExponent(first)
            ? new LinkLoMonomial(first.Add(monomial), rest)
            : new LinkLoMonomial(first, rest.Add(monomial));

    public ILoMonomial AddAll(ILoMonomial other) => rest.AddAll(other).Add(first);

    public bool Equals(ILoMonomial other) => other.EqualsLink(this);

    public bool EqualsLink(LinkLoMonomial other) =>
        first.Equals(other.first) && rest.Equals(other.rest);

    public bool EqualsEmpty(EmptyLoMonomial other) => false;

    public ILoMonomial Multiply(Monomial monomial) =>
        new LinkLoMonomial(first.Multiply(monomial), rest.Multiply(monomial));

    public ILoMonomial MultiplyAll(ILoMonomial other) =>
        other.Multiply(first).AddAll(rest.MultiplyAll(other));

    public int Evaluate(int x) => first.Evaluate(x) + rest.Evaluate(x);
}

public class EmptyLoMonomial : ILoMonomial
{
    public bool Any(Func<Monomial, bool> predicate) => false;

    public bool All(Func<Monomial, bool> predicate) => true;

    public bool HasUniqueExponents() => true;

    public ILoMonomial Normalize() => this;

    public ILoMonomial InsertInSorted(Monomial monomial) => new LinkLoMonomial(monomial, this);

    public ILoMonomial Add(Monomial monomial) => new LinkLoMonomial(monomial, this);

    public ILoMonomial AddAll(ILoMonomial other) => other;

    public bool Equals(ILoMonomial other) => other.EqualsEmpty(this);

    public bool EqualsLink(LinkLoMonomial other) => false;

    public bool EqualsEmpty(EmptyLoMonomial other) => true;

    public ILoMonomial Multiply(Monomial monomial) => new EmptyLoMonomial();

    public ILoMonomial MultiplyAll(ILoMonomial other) => this;

    public int Evaluate(int x) => 0;
}
