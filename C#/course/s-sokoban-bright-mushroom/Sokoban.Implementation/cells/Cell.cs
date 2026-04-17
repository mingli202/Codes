using ImageLib;
using Sokoban.Colors;

namespace Sokoban.cells;

/// <summary>
/// Base type for all cells in a level grid.
/// </summary>
public abstract class Cell
{
    /// <summary>
    /// Gets the image of the cell.
    /// </summary>
    /// <returns>The image of the cell.</returns>
    public abstract WorldImage GetImage();

    /// <summary>
    /// Compares two cells for equality.
    /// </summary>
    /// <param name="other">The other cell to compare.</param>
    /// <returns>True if the cells are equal, false otherwise.</returns>
    public abstract bool Equals(Cell other);

    /// <summary>
    /// Compares this cell to a blank cell.
    /// </summary>
    /// <param name="other">The blank cell to compare.</param>
    /// <returns>True if the cells are equal, false otherwise.</returns>
    public virtual bool EqualsBlank(Blank other) => false;

    /// <summary>
    /// Compares this cell to a crate cell.
    /// </summary>
    /// <param name="other">The crate cell to compare.</param>
    /// <returns>True if the cells are equal, false otherwise.</returns>
    public virtual bool EqualsCrate(Crate other) => false;

    /// <summary>
    /// Compares this cell to a player cell.
    /// </summary>
    /// <param name="other">The player cell to compare.</param>
    /// <returns>True if the cells are equal, false otherwise.</returns>
    public virtual bool EqualsPlayer(Player other) => false;

    /// <summary>
    /// Compares this cell to a target cell.
    /// </summary>
    /// <param name="other">The target cell to compare.</param>
    /// <returns>True if the cells are equal, false otherwise.</returns>
    public virtual bool EqualsTarget(Target other) => false;

    /// <summary>
    /// Compares this cell to a trophy cell.
    /// </summary>
    /// <param name="other">The trophy cell to compare.</param>
    /// <returns>True if the cells are equal, false otherwise.</returns>
    public virtual bool EqualsTrophy(Trophy other) => false;

    /// <summary>
    /// Compares this cell to a wall cell.
    /// </summary>
    /// <param name="other">The wall cell to compare.</param>
    /// <returns>True if the cells are equal, false otherwise.</returns>
    public virtual bool EqualsWall(Wall other) => false;

    /// <summary>
    /// Indicates whether this cell is a player.
    /// </summary>
    /// <returns>True if this cell is a player; otherwise false.</returns>
    public virtual bool IsPlayer() => false;

    /// <summary>
    /// Checks if the cell is a trophy on the target. True for every cell except for when this is a target.
    /// If this is a target, it is delegated to the IsTrophyOnTargetTarget method.
    /// </summary>
    /// <param name="other">The other cell to check.</param>
    /// <returns>True if the cell is a trophy on the target, false otherwise.</returns>
    public virtual bool IsTrophyOnTarget(Cell other) => true;

    /// <summary>
    /// Called when the cell is a target. False for every cell except for when this is a trophy.
    /// If this is a trophy, it will check whether the target is the same color as this trophy.
    /// </summary>
    /// <param name="targetColor">The target color to check.</param>
    /// <returns>True if the cell is a trophy on the target, false otherwise.</returns>
    public virtual bool IsTrophyOnTargetTarget(AColor targetColor) => false;

    /// <summary>
    /// Creates a cell from a description character.
    /// </summary>
    /// <param name="c">The description character.</param>
    /// <returns>The cell created from the description character.</returns>
    public static Cell FromDescription(char c) =>
        c switch
        {
            'y' or 'g' or 'b' or 'r' => new Trophy(c),
            '_' => new Blank(),
            'W' => new Wall(),
            'B' => new Crate(),
            '>' or '<' or 'v' or '^' => new Player(c),
            _ => throw new ArgumentException("does not match any description cell: " + c),
        };

    /// <summary>
    /// Creates a cell from a ground character.
    /// </summary>
    /// <param name="c">The ground character.</param>
    /// <returns>The cell created from the ground character.</returns>
    public static Cell FromGround(char c) =>
        c switch
        {
            'Y' or 'G' or 'B' or 'R' => new Target(c),
            '_' => new Blank(),
            _ => throw new ArgumentException("does not match any ground cell " + c),
        };
}

