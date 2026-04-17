using System.Windows.Media;
using ImageLib;
using Sokoban.Enum;

namespace Sokoban.Cells;

/// <summary>
/// A movable trophy cell with a color.
/// </summary>
/// <param name="color">The trophy color.</param>
public class Trophy(Color color) : Cell
{
    private readonly Color _color = color;

    /// <summary>
    /// Creates a trophy from a color character.
    /// </summary>
    /// <param name="color">The color character.</param>
    public Trophy(char color)
        : this(Util.ParseColor(color)) { }

    /// <inheritdoc />
    public override bool IsTrophyOnTargetTarget(Color targetColor) => _color.Equals(targetColor);

    /// <inheritdoc />
    public override WorldImage GetImage() =>
        _color switch
        {
            _ when _color == Colors.Blue => new FromFileImage("assets/blue_trophy.png"),
            _ when _color == Colors.Green => new FromFileImage("assets/green_trophy.png"),
            _ when _color == Colors.Red => new FromFileImage("assets/red_trophy.png"),
            _ when _color == Colors.Yellow => new FromFileImage("assets/yellow_trophy.png"),
            _ => throw new ArgumentOutOfRangeException(nameof(_color)),
        };

    /// <inheritdoc />
    public override bool Equals(ICell other) => other.EqualsTrophy(this);

    /// <inheritdoc />
    public override bool EqualsTrophy(Trophy other) => other._color.Equals(_color);

    /// <inheritdoc />
    public override Level HandleSwap(
        Level level,
        Point thisPoint,
        Point otherPoint,
        Direction direction,
        int remainingMoves
    )
    {
        if (remainingMoves <= 1)
        {
            return level;
        }

        return level
            .MoveCell(thisPoint, direction, remainingMoves - 1)
            .MoveCell(otherPoint, direction, remainingMoves - 2);
    }

    /// <summary>
    /// Returns the character representation of the trophy color.
    /// </summary>
    /// <returns>The trophy character.</returns>
    public override string ToString() =>
        _color switch
        {
            _ when _color == Colors.Blue => "b",
            _ when _color == Colors.Green => "g",
            _ when _color == Colors.Red => "r",
            _ when _color == Colors.Yellow => "y",
            _ => throw new ArgumentOutOfRangeException(nameof(_color)),
        };
}

