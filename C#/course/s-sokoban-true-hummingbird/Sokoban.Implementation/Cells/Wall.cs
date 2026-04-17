using ImageLib;

namespace Sokoban.Cells;

/// <summary>
/// A wall cell that blocks movement.
/// </summary>
public class Wall : Cell
{
    /// <inheritdoc />
    public override WorldImage GetImage() => new FromFileImage("assets/wall.png");

    /// <inheritdoc />
    public override bool Equals(ICell other) => other.EqualsWall(this);

    /// <inheritdoc />
    public override bool EqualsWall(Wall other) => true;

    /// <summary>
    /// Returns the character representation of the wall.
    /// </summary>
    /// <returns>The wall character.</returns>
    public override string ToString() => "W";
}

