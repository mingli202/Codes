namespace Polynomials;

public interface IExpression
{
    /// <summary>
    /// Returns the evaluation of the expression at x.
    /// </summary>
    int Evaluate(int x);
}
