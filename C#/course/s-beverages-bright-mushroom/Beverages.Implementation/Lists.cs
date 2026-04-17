namespace Beverages;


// You may edit this interface to add methods you need
public interface ILoString
{
    /// <summary>
    /// Returns true if the list contains the given string.
    /// </summary>
    /// <param name="s">The string to search for.</param>
    /// <returns>True if the list contains the string.</returns>
    bool Contains(string s);

    /// <summary>
    /// Returns a string representation of the list.
    /// The string representation should be a comma-separated list of strings.
    /// If the list is empty, the string should be the empty string.
    /// </summary>
    /// <returns>A string representation of the list.</returns>
    string Format();

    /// <summary>
    /// Returns a string representation of the list.
    /// The string representation should be a comma-separated list of strings.
    /// If the list is empty, the string should be the empty string.
    /// The string should start with a comma.
    /// </summary>
    /// <returns>A string representation of the list.</returns>
    string FormatStartingWithComma();
}

// an empty list of strings
public class EmptyLoString : ILoString
{
    public bool Contains(string s) => false;
    public string Format() => "";
    public string FormatStartingWithComma() => "";
}

// a non-empty list of strings
public class LinkLoString : ILoString
{
    readonly string first;
    readonly ILoString rest;

    public LinkLoString(string first, ILoString rest)
    {
        this.first = first;
        this.rest = rest;
    }

    public bool Contains(string s) => s.Equals(first) || rest.Contains(s);

    public string Format() => $"{first}{rest.FormatStartingWithComma()}";
    public string FormatStartingWithComma() => $", {first}{rest.FormatStartingWithComma()}";
}
