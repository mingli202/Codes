namespace Sokoban.Colors;

/// <summary>
/// Base type for RGB colors used in the game.
/// </summary>
/// <param name="R">Red channel value.</param>
/// <param name="G">Green channel value.</param>
/// <param name="B">Blue channel value.</param>
public abstract record AColor(byte R, byte G, byte B)
{
    /// <summary>
    /// Creates a color from a character code.
    /// </summary>
    /// <param name="c">The color character.</param>
    /// <returns>The corresponding color.</returns>
    public static AColor From(char c) =>
        c switch
        {
            'Y' or 'y' => new Yellow(),
            'G' or 'g' => new Green(),
            'R' or 'r' => new Red(),
            'B' or 'b' => new Blue(),
            _ => throw new ArgumentOutOfRangeException(nameof(c)),
        };
}

/// <summary>
/// Blue color.
/// </summary>
public record Blue : AColor
{
    public Blue()
        : base(0, 0, 255) { }
}

/// <summary>
/// Green color.
/// </summary>
public record Green : AColor
{
    public Green()
        : base(0, 128, 0) { }
}

/// <summary>
/// Red color.
/// </summary>
public record Red : AColor
{
    public Red()
        : base(255, 0, 0) { }
}

/// <summary>
/// Yellow color.
/// </summary>
public record Yellow : AColor
{
    public Yellow()
        : base(255, 255, 0) { }
}
