namespace Automata;

// the virtual and throw new NotImplementedException() are workaround for the error "The interface cannot be used as type argument. Static member does not have a most specific implementation in the interface" in the tests
public interface IOperation
{
    /// <summary>
    /// Gets the state of this IOperation
    /// </summary>
    int GetState();

    /// <summary>
    /// Gets the IOperation that is the logical OR of the given IOperation a and the given IOperation b
    /// </summary>
    /// <param name="opA">The first IOperation</param>
    /// <param name="opB">The second IOperation</param>
    /// <returns>The logical OR</returns>
    static virtual IOperation Or(IOperation opA, IOperation opB) =>
        throw new NotImplementedException();

    /// <summary>
    /// Gets the IOperation that is the logical AND of the given IOperation a and the given IOperation b
    /// </summary>
    /// <param name="opA">The first IOperation</param>
    /// <param name="opB">The second IOperation</param>
    /// <returns>The logical AND</returns>
    static virtual IOperation And(IOperation opA, IOperation opB) =>
        throw new NotImplementedException();

    /// <summary>
    /// Gets the IOperation that is the logical XOR of the given IOperation a and the given IOperation b
    /// </summary>
    /// <param name="opA">The first IOperation</param>
    /// <param name="opB">The second IOperation</param>
    /// <returns>The logical XOR</returns>
    static virtual IOperation Xor(IOperation opA, IOperation opB) =>
        throw new NotImplementedException();

    /// <summary>
    /// Gets the IOperation that is the logical NOT of the given IOperation a
    /// </summary>
    /// <param name="opA">The IOperation</param>
    /// <returns>The logical NOT</returns>
    static virtual IOperation Not(IOperation opA) => throw new NotImplementedException();
}

public class Operation(int state) : IOperation
{
    public Operation(bool state)
        : this(FromBool(state)) { }

    private readonly int state = state;

    public int GetState() => state;

    public static IOperation Or(IOperation opA, IOperation opB) =>
        new Operation(opA.GetState() == 1 || opB.GetState() == 1);

    public static IOperation And(IOperation opA, IOperation opB) =>
        new Operation(opA.GetState() == 1 && opB.GetState() == 1);

    public static IOperation Xor(IOperation opA, IOperation opB) =>
        new Operation(
            opA.GetState() == 0 && opB.GetState() == 1 || opA.GetState() == 1 && opB.GetState() == 0
        );

    public static IOperation Not(IOperation opA) => new Operation(opA.GetState() == 0);

    private static int FromBool(bool b) => b ? 1 : 0;
}
