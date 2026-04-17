using ImageLib;
using Sokoban.cells;

namespace Sokoban;

/// <summary>
/// Represents a Sokoban level, including ground and description layers.
/// </summary>
public class Level
{
    private readonly Board description;
    private readonly Board ground;

    private readonly int playerRow;
    private readonly int playerCol;

    /// <summary>
    /// Creates a new level from a given description and ground Board.
    /// </summary>
    /// <param name="description">The description of the level.</param>
    /// <param name="ground">The ground of the level.</param>
    /// <exception cref="ArgumentException">Thrown if the ground and description have different numbers of rows and columns, or if no player is found in the description.</exception>
    public Level(Board description, Board ground)
    {
        if (description.NRows != ground.NRows || description.NCols != ground.NCols)
        {
            throw new ArgumentException(
                "ground and description must have the same number of rows and columns"
            );
        }

        var (playerRow, playerCol) = description
            .IndexOfWith((cell, _, _) => cell.IsPlayer())
            .Expect("no player found in your description!");

        this.playerRow = playerRow;
        this.playerCol = playerCol;
        this.description = description;
        this.ground = ground;
    }

    /// <summary>
    /// Creates a new level from a given description and ground grids.
    /// </summary>
    /// <param name="description">The description of the level.</param>
    /// <param name="ground">The ground of the level.</param>
    /// <exception cref="ArgumentException">Thrown if the ground and description have different numbers of rows and columns, or if no player is found in the description.</exception>
    public Level(Grid<Cell> description, Grid<Cell> ground)
        : this(new Board(description), new Board(ground)) { }

    /// <summary>
    /// Creates a new level from a ground and description string.
    /// </summary>
    /// <param name="ground">The ground of the level.</param>
    /// <param name="description">The description of the level.</param>
    /// <exception cref="ArgumentException">Thrown if the ground and description have different numbers of rows and columns, or if no player is found in the description.</exception>
    public Level(string ground, string description)
        : this(ParseDescription(description), ParseGround(ground)) { }

    /// <summary>
    /// Moves the player in the specified direction.
    /// </summary>
    /// <param name="direction">The direction to move the player.</param>
    /// <returns>A new level with the player moved.</returns>
    public Level MovePlayer(PlayerDirection direction)
    {
        var (newPlayerRow, newPlayerCol) = direction switch
        {
            PlayerDirection.Up => (playerRow - 1, playerCol),
            PlayerDirection.Down => (playerRow + 1, playerCol),
            PlayerDirection.Left => (playerRow, playerCol - 1),
            PlayerDirection.Right => (playerRow, playerCol + 1),
            _ => throw new ArgumentException("direction not supported"),
        };

        return new Level(
            description.Swap(playerRow, playerCol, newPlayerRow, newPlayerCol),
            ground
        );
    }

    /// <summary>
    /// Parses the description of the level into a Board.
    /// </summary>
    /// <param name="description">The description of the level.</param>
    /// <returns>The board of cells representing the description.</returns>
    private static Board ParseDescription(string description) =>
        new(
            Util.ListOfCharFrom(description)
                .SplitAt('\n')
                .Map((row, _) => row.Map((c, _) => Cell.FromDescription(c)))
        );

    /// <summary>
    /// Parses the ground of the level into a Board.
    /// </summary>
    /// <param name="ground">The ground of the level.</param>
    /// <returns>The board of cells representing the ground.</returns>
    private static Board ParseGround(string ground) =>
        new(
            Util.ListOfCharFrom(ground)
                .SplitAt('\n')
                .Map((row, _) => row.Map((c, _) => Cell.FromGround(c)))
        );

    /// <summary>
    /// Renders the level as an image.
    /// </summary>
    /// <returns>The image of the level.</returns>
    public WorldImage Render() => new OverlayImage(description.Render(), ground.Render());

    /// <summary>
    /// Checks if the level is won. The level is won if all the target have a trophy of the same color on them.
    /// </summary>
    /// <returns>True if the level is won, false otherwise.</returns>
    public bool LevelWon() =>
        ground.All(
            (groundCell, i, k) =>
                description
                    .Get(i, k)
                    .MapOr(false, (descCell) => groundCell.IsTrophyOnTarget(descCell))
        );

    /// <summary>
    /// Returns a string representation of the level.
    /// </summary>
    /// <returns>A string representation of the level.</returns>
    public override string ToString() =>
        $"Level\n{ground.ToPrettyString()}\n{description.ToPrettyString()}";
}

