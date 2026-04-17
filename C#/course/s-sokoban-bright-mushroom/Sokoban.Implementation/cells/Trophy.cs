using ImageLib;
using Sokoban.Colors;

namespace Sokoban.cells;

/// <summary>
/// A movable trophy cell with a color.
/// </summary>
/// <param name="color">The trophy color.</param>
public class Trophy(AColor color) : Cell
{
    private readonly AColor _color = color;

    /// <summary>
    /// Creates a trophy from a color character.
    /// </summary>
    /// <param name="color">The color character.</param>
    public Trophy(char color)
        : this(AColor.From(color)) { }

    /// <inheritdoc />
    public override bool IsTrophyOnTargetTarget(AColor targetColor) => _color.Equals(targetColor);

    /// <inheritdoc />
    public override WorldImage GetImage() =>
        _color switch
        {
            Blue => new FromFileImage("assets/blue_trophy.png"),
            Green => new FromFileImage("assets/green_trophy.png"),
            Red => new FromFileImage("assets/red_trophy.png"),
            Yellow => new FromFileImage("assets/yellow_trophy.png"),
            _ => throw new ArgumentOutOfRangeException(nameof(_color)),
        };

    /// <inheritdoc />
    public override bool Equals(Cell other) => other.EqualsTrophy(this);

    /// <inheritdoc />
    public override bool EqualsTrophy(Trophy other) => other._color.Equals(_color);

    /// <summary>
    /// Returns the character representation of the trophy color.
    /// </summary>
    /// <returns>The trophy character.</returns>
    public override string ToString() =>
        _color switch
        {
            Blue => "b",
            Green => "g",
            Red => "r",
            Yellow => "y",
            _ => throw new ArgumentOutOfRangeException(nameof(_color)),
        };
}

