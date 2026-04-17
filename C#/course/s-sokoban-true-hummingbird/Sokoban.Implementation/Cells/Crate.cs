using ImageLib;
using Sokoban.Enum;

namespace Sokoban.Cells;

/// <summary>
/// A crate cell that can be pushed.
/// </summary>
public class Crate : Cell
{
    /// <inheritdoc />
    public override WorldImage GetImage() => new FromFileImage("assets/crate.png");

    /// <inheritdoc />
    public override bool Equals(ICell other) => other.EqualsCrate(this);

    /// <inheritdoc />
    public override bool EqualsCrate(Crate other) => true;

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
    /// Returns the character representation of the crate.
    /// </summary>
    /// <returns>The crate character.</returns>
    public override string ToString() => "B";
}

