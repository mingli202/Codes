using System.Windows.Media;
using ImageLib;
using Sokoban.Cells;
using Sokoban.Enum;
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
    public static IListOf<char> ListOfCharFrom(string s) => ListOfCharFromHelper(s, 0);

    /// <summary>
    /// Helper method that converts a string into a list of characters.
    /// </summary>
    /// <param name="s">The string to convert.</param>
    /// <param name="index">The index to start converting from.</param>
    /// <returns>A list of characters.</returns>
    public static IListOf<char> ListOfCharFromHelper(string s, int index) =>
        index >= s.Length
            ? new EmptyLo<char>()
            : new LinkLo<char>(s[index], ListOfCharFromHelper(s, index + 1));

    /// <summary>
    /// Parses a level color marker into its corresponding WPF color.
    /// </summary>
    /// <param name="c">The color character.</param>
    /// <returns>The parsed color.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="c"/> is not a supported color marker.</exception>
    public static Color ParseColor(char c) =>
        c switch
        {
            'Y' or 'y' => Colors.Yellow,
            'G' or 'g' => Colors.Green,
            'R' or 'r' => Colors.Red,
            'B' or 'b' => Colors.Blue,
            _ => throw new ArgumentOutOfRangeException(nameof(c)),
        };

    /// <summary>
    /// Scales the image to fit a square of the given size.
    /// </summary>
    /// <param name="image">The image to scale.</param>
    /// <param name="size">The target square size in pixels.</param>
    /// <returns>The scaled image.</returns>
    public static ScaleImage ScaleImage(WorldImage image, int size) =>
        new(image, size / Math.Max(image.Width, image.Height));

    /// <summary>
    /// Gets the next point in the given direction.
    /// </summary>
    /// <param name="p">The current point.</param>
    /// <param name="direction">The direction to move.</param>
    /// <returns>The next point in the given direction.</returns>
    public static Point NextPoint(Point p, Direction direction) =>
        direction switch
        {
            Direction.Up => new Point(p.Row - 1, p.Col),
            Direction.Down => new Point(p.Row + 1, p.Col),
            Direction.Left => new Point(p.Row, p.Col - 1),
            Direction.Right => new Point(p.Row, p.Col + 1),
            _ => throw new ArgumentException("direction not supported"),
        };
}

