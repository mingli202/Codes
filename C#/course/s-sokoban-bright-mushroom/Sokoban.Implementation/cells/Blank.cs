using System.Windows.Media;
using ImageLib;
using ImageLib.Enumerations;

namespace Sokoban.cells;

/// <summary>
/// An empty cell.
/// </summary>
public class Blank : Cell
{
    /// <inheritdoc />
    public override WorldImage GetImage() =>
        new RectangleImage(120, 120, OutlineMode.Fill, Color.FromArgb(0, 0, 0, 0));

    /// <inheritdoc />
    public override bool Equals(Cell other) => other.EqualsBlank(this);

    /// <inheritdoc />
    public override bool EqualsBlank(Blank other) => true;

    /// <summary>
    /// Returns the character representation of the blank cell.
    /// </summary>
    /// <returns>The blank character.</returns>
    public override string ToString() => "_";
}

