using System.Windows.Media;
using ImageLib;
using Sokoban.Enum;

namespace Sokoban.Cells;

/// <summary>
/// Represents a row and column position in a board. (0, 0) is the top-left corner.
/// </summary>
/// <param name="Row">The row index. Represent the top-most row as 0.</param>
/// <param name="Col">The column index. Represent the left-most column as 0.</param>
public record Point(int Row, int Col);

/// <summary>
/// Defines the behavior shared by all board cells.
/// </summary>
public interface ICell
{
    /// <summary>
    /// Gets the image of the cell.
    /// </summary>
    /// <returns>The image of the cell.</returns>
    WorldImage GetImage();

    /// <summary>
    /// Compares two cells for equality.
    /// </summary>
    /// <param name="other">The other cell to compare.</param>
    /// <returns>True if the cells are equal, false otherwise.</returns>
    bool Equals(ICell other);

    /// <summary>
    /// Compares this cell to a blank cell.
    /// </summary>
    /// <param name="other">The blank cell to compare.</param>
    /// <returns>True if the cells are equal, false otherwise.</returns>
    bool EqualsBlank(Blank other);

    /// <summary>
    /// Compares this cell to a crate cell.
    /// </summary>
    /// <param name="other">The crate cell to compare.</param>
    /// <returns>True if the cells are equal, false otherwise.</returns>
    bool EqualsCrate(Crate other);

    /// <summary>
    /// Compares this cell to a player cell.
    /// </summary>
    /// <param name="other">The player cell to compare.</param>
    /// <returns>True if the cells are equal, false otherwise.</returns>
    bool EqualsPlayer(Player other);

    /// <summary>
    /// Compares this cell to a target cell.
    /// </summary>
    /// <param name="other">The target cell to compare.</param>
    /// <returns>True if the cells are equal, false otherwise.</returns>
    bool EqualsTarget(Target other);

    /// <summary>
    /// Compares this cell to a trophy cell.
    /// </summary>
    /// <param name="other">The trophy cell to compare.</param>
    /// <returns>True if the cells are equal, false otherwise.</returns>
    bool EqualsTrophy(Trophy other);

    /// <summary>
    /// Compares this cell to a wall cell.
    /// </summary>
    /// <param name="other">The wall cell to compare.</param>
    /// <returns>True if the cells are equal, false otherwise.</returns>
    bool EqualsWall(Wall other);

    /// <summary>
    /// Compares this cell to a hole cell.
    /// </summary>
    /// <param name="other">The hole cell to compare.</param>
    /// <returns>True if the cells are equal, false otherwise.</returns>
    bool EqualsHole(Hole other);

    /// <summary>Compares this cell to an ice cell.</summary>
    /// <param name="other">The ice cell to compare.</param>
    /// <returns>True if the cells are equal, false otherwise.</returns>
    bool EqualsIce(Ice other);

    /// <summary>
    /// Indicates whether this cell is a player.
    /// </summary>
    /// <returns>True if this cell is a player; otherwise false.</returns>
    bool IsPlayer();

    /// <summary>
    /// Checks if the cell is a trophy on the target. True for every cell except for when this is a target.
    /// If this is a target, it is delegated to the IsTrophyOnTargetTarget method.
    /// </summary>
    /// <param name="other">The other cell to check.</param>
    /// <returns>True if the cell is a trophy on the target, false otherwise.</returns>
    bool IsTrophyOnTarget(ICell other);

    /// <summary>
    /// Called when the cell is a target. False for every cell except for when this is a trophy.
    /// If this is a trophy, it will check whether the target is the same color as this trophy.
    /// </summary>
    /// <param name="targetColor">The target color to check.</param>
    /// <returns>True if the cell is a trophy on the target, false otherwise.</returns>
    bool IsTrophyOnTargetTarget(Color targetColor);

    /// <summary>
    /// Swaps the cell with another cell. <paramref name="otherPoint"/> is the other cell's position which wants to swap with this cell.
    /// </summary>
    /// <param name="level">">The level to swap with.</param>
    /// <param name="thisPoint">The position of the cell to swap.</param>
    /// <param name="otherPoint">The position of the cell to swap with.</param>
    /// <param name="direction">The direction of the swap</param>
    /// <param name="remainingMoves">Calculate how many other moves can this swap trigger. It is used to handle pushing and can be used in the future if the player can push more than 1 cell.</param>
    /// <returns>The level with the description cell swapped.</returns>
    Level HandleSwap(
        Level level,
        Point thisPoint,
        Point otherPoint,
        Direction direction,
        int remainingMoves
    );

    /// <summary>
    /// Method called on ground cells when swapping with another cell. THe ground cell can therefore handle additional movements if needed
    /// </summary>
    /// <param name="level">The level to swap with.</param>
    /// <param name="thisPoint">The position of the cell to swap.</param>
    /// <param name="otherPoint">The position of the cell to swap with.</param>
    /// <param name="direction">The direction of the swap</param>
    /// <param name="remainingMoves">The remaining moves.</param>
    /// <returns>The level with the desciption Board swapped.</returns>
    Level HandleGroundSwap(
        Level level,
        Point thisPoint,
        Point otherPoint,
        Direction direction,
        int remainingMoves
    );
}

/// <summary>
/// Base type for all cells in a level grid.
/// </summary>
public abstract class Cell : ICell
{
    /// <inheritdoc />
    public abstract WorldImage GetImage();

    /// <inheritdoc />
    public abstract bool Equals(ICell other);

    /// <inheritdoc />
    public virtual bool EqualsBlank(Blank other) => false;

    /// <inheritdoc />
    public virtual bool EqualsCrate(Crate other) => false;

    /// <inheritdoc />
    public virtual bool EqualsPlayer(Player other) => false;

    /// <inheritdoc />
    public virtual bool EqualsTarget(Target other) => false;

    /// <inheritdoc />
    public virtual bool EqualsTrophy(Trophy other) => false;

    /// <inheritdoc />
    public virtual bool EqualsWall(Wall other) => false;

    /// <inheritdoc />
    public virtual bool EqualsHole(Hole other) => false;

    /// <inheritdoc />
    public virtual bool EqualsIce(Ice other) => false;

    /// <inheritdoc />
    public virtual bool IsPlayer() => false;

    /// <inheritdoc />
    public virtual bool IsTrophyOnTarget(ICell other) => true;

    /// <inheritdoc />
    public virtual bool IsTrophyOnTargetTarget(Color targetColor) => false;

    /// <inheritdoc />
    public virtual Level HandleSwap(
        Level level,
        Point thisPoint,
        Point otherPoint,
        Direction direction,
        int remainingMoves
    ) => level;

    /// <inheritdoc />
    public virtual Level HandleGroundSwap(
        Level level,
        Point thisPoint,
        Point otherPoint,
        Direction direction,
        int remainingMoves
    ) => level;

    /// <summary>
    /// Creates a cell from a description character.
    /// </summary>
    /// <param name="c">The description character.</param>
    /// <returns>The cell created from the description character.</returns>
    public static ICell FromDescription(char c) =>
        c switch
        {
            'y' or 'g' or 'b' or 'r' => new Trophy(c),
            '_' => new Blank(),
            'W' => new Wall(),
            'B' => new Crate(),
            '>' or '<' or 'v' or '^' => new Player(c),
            'H' => new Hole(),
            _ => BlankWithWarning(c),
        };

    /// <summary>
    /// Creates a cell from a ground character.
    /// </summary>
    /// <param name="c">The ground character.</param>
    /// <returns>The cell created from the ground character.</returns>
    public static ICell FromGround(char c) =>
        c switch
        {
            'Y' or 'G' or 'B' or 'R' => new Target(c),
            '_' => new Blank(),
            'I' => new Ice(),
            _ => BlankWithWarning(c),
        };

    /// <summary>
    /// Creates a blank cell with a warning message when the character is invalid.
    /// </summary>
    /// <param name="c">The invalid character.</param>
    /// <returns>The blank cell with a warning message.</returns>
    private static Blank BlankWithWarning(char c)
    {
        Console.WriteLine("invalid cell character " + c);
        return new Blank();
    }
}

