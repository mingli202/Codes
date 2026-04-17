using ImageLib;
using Sokoban.Enum;

namespace Sokoban.Cells;

/// <summary>
/// A hole cell that removes whatever moves into it.
/// </summary>
public class Hole : Cell
{
    /// <inheritdoc />
    public override WorldImage GetImage() => new FromFileImage("assets/hole.png");

    /// <inheritdoc />
    public override bool Equals(ICell other) => other.EqualsHole(this);

    /// <inheritdoc />
    public override bool EqualsHole(Hole other) => true;

    /// <inheritdoc />
    public override Level HandleSwap(
        Level level,
        Point thisPoint,
        Point otherPoint,
        Direction direction,
        int remainingMoves
    ) => level.SetCell(thisPoint, new Blank()).SetCell(otherPoint, new Blank());

    /// <summary>
    /// Returns the character representation of the hole.
    /// </summary>
    /// <returns>The hole character.</returns>
    public override string ToString() => "H";
}

