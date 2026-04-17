using Sokoban.Lists;

namespace Sokoban;

/// <summary>
/// Utility helpers for the project.
/// </summary>
public static class Util
{
    /// <summary>
    /// Converts a string into a list of characters.
    /// </summary>
    /// <param name="s">The string to convert.</param>
    /// <returns>A list of characters.</returns>
    public static ListOf<char> ListOfCharFrom(string s) => ListOfCharFromHelper(s, 0);

    /// <summary>
    /// Helper method that converts a string into a list of characters.
    /// </summary>
    /// <param name="s">The string to convert.</param>
    /// <param name="index">The index to start converting from.</param>
    /// <returns>A list of characters.</returns>
    public static ListOf<char> ListOfCharFromHelper(string s, int index) =>
        index >= s.Length
            ? new EmptyLo<char>()
            : new LinkLo<char>(s[index], ListOfCharFromHelper(s, index + 1));
}
