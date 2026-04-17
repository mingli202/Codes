using ImageLib;

namespace Sokoban.cells;

/// <summary>
/// A crate cell that can be pushed.
/// </summary>
public class Crate : Cell
{
    /// <inheritdoc />
    public override WorldImage GetImage() => new FromFileImage("assets/crate.png");

    /// <inheritdoc />
    public override bool Equals(Cell other) => other.EqualsCrate(this);

    /// <inheritdoc />
    public override bool EqualsCrate(Crate other) => true;

    /// <summary>
    /// Returns the character representation of the crate.
    /// </summary>
    /// <returns>The crate character.</returns>
    public override string ToString() => "B";
}

