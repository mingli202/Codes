using System.Windows.Media;
using ImageLib;
using ImageLib.Enumerations;
using Sokoban.Enum;

namespace Sokoban.Cells;

/// <summary>
/// An empty cell.
/// </summary>
public class Blank : Cell
{
    /// <inheritdoc />
    public override WorldImage GetImage() =>
        new RectangleImage(120, 120, OutlineMode.Fill, Color.FromArgb(0, 0, 0, 0));

    /// <inheritdoc />
    public override bool Equals(ICell other) => other.EqualsBlank(this);

    /// <inheritdoc />
    public override bool EqualsBlank(Blank other) => true;

    /// <inheritdoc />
    public override Level HandleSwap(
        Level level,
        Point thisPoint,
        Point otherPoint,
        Direction direction,
        int remainingMoves
    ) => level.SwapCell(thisPoint, otherPoint);

    /// <summary>
    /// Returns the character representation of the blank cell.
    /// </summary>
    /// <returns>The blank character.</returns>
    public override string ToString() => "_";
}

