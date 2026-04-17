using ImageLib;
using Sokoban.Enum;

namespace Sokoban.Cells;

/// <summary>
/// A hole cell that removes whatever moves into it.
/// </summary>
public class Ice : Cell
{
    /// <inheritdoc />
    public override WorldImage GetImage() => new FromFileImage("assets/ice.png");

    /// <inheritdoc />
    public override bool Equals(ICell other) => other.EqualsIce(this);

    /// <inheritdoc />
    public override bool EqualsIce(Ice other) => true;

    /// <summary>
    /// The ice cell simply moves whatever is on it to the next position.
    /// </summary>
    public override Level HandleGroundSwap(
        Level level,
        Point thisPoint,
        Point otherPoint,
        Direction direction,
        int remainingMoves
    ) => level.MoveCell(thisPoint, direction, remainingMoves);

    /// <summary>
    /// Returns the character representation of the hole.
    /// </summary>
    /// <returns>The hole character.</returns>
    public override string ToString() => "I";
}

