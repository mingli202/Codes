namespace ExamplarIntroduction;

// A ListOfInt (LoI) is one of
// -- EmptyLoInt
// -- LinkLoInt(int first, ListOfInt rest)

// Extra comment for the vine

public abstract record ListOfInt;
public record EmptyLoInt() : ListOfInt;
public record LinkLoInt(int First, ListOfInt Rest) : ListOfInt;

public abstract record ListOfBools;
public record EmptyLoBool() : ListOfBools;
public record LinkLoBool(bool First, ListOfBools Rest) : ListOfBools;

public class ListFunctions
{
    /// <summary>
    /// Returns the number of elements contained in the given list of integers.
    /// </summary>
    /// <param name="loi">The list of integers whose element count is to be computed.</param>
    /// <returns>The number of elements in the given list of integers.</returns>
    public static int Length(ListOfInt loi) => loi switch
    {
        EmptyLoInt => 0,
        LinkLoInt(_, var rest) => 1 + Length(rest),
        _ => throw new NotImplementedException(),
    };

    /// <summary>
    /// Determines whether the given list contains the given integer value.
    /// </summary>
    /// <param name="loi">The list of integers to search.</param>
    /// <param name="value">The integer value to locate in the list.</param>
    /// <returns>true if the specified value is found in the list; otherwise, false.</returns>
    public static bool Contains(ListOfInt loi, int value) => loi switch
    {
        EmptyLoInt => false,
        LinkLoInt(var first, var rest) => first == value || Contains(rest, value),
        _ => throw new NotImplementedException(),
    };

    /// <summary>
    /// Returns the sum of the elements in the given list of integers.
    /// </summary>
    /// <param name="loi">The list of integers whose sum is to be computed.</param>
    /// <returns>The sum of the elements in the given list of integers.</returns>
    public static int Sum(ListOfInt loi) => loi switch
    {
        EmptyLoInt => 0,
        LinkLoInt(var first, var rest) => first + Sum(rest),
        _ => throw new NotImplementedException(),
    };

    public static int Product(ListOfInt loi) => HandleProduct(loi, true);

    private static int HandleProduct(ListOfInt loi, bool isFirst) => loi switch
    {
        EmptyLoInt => isFirst ? 0 : 1,
        LinkLoInt(var first, var rest) => first * HandleProduct(rest, false),
        _ => throw new NotImplementedException(),
    };
}
