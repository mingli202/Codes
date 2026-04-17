using ImageLib;
using Sokoban.Cells;
using Sokoban.Enum;
using Sokoban.Lists;

namespace Sokoban;

/// <summary>
/// Represents a Sokoban level, including ground and description layers.
/// </summary>
public class Level
{
    /// <summary>
    /// Built-in level definitions stored as ground and description string pairs.
    /// </summary>
    private static readonly List<(string, string)> levels =
    [
        (   // Yes this is possible. My best: 50 moves
            string.Join(
                "\n",
                [
                    "___________",
                    "__IIIIIIII_",
                    "_IIIIIII___",
                    "_I____I____",
                    "_I__I_I__I_",
                    "_I__I_I__I_",
                    "_I__I_I__I_",
                    "_I__I_I__I_",
                    "_I__I____I_",
                    "____R____I_",
                    "___________",
                ]
            ),
            string.Join(
                "\n",
                [
                    "WWWWWWWWWWW",
                    "W>_______HW",
                    "WB__r_BHW_W",
                    "W_______B_W",
                    "W____W__W_W",
                    "W_r__W__W_W",
                    "W____WB_W_W",
                    "W__WHWB_W_W",
                    "W__W_W__W_W",
                    "WH_W_W____W",
                    "WWWWWWWWWWW",
                ]
            )
        ),
        (
            string.Join(
                "\n",
                [
                    "________",
                    "________",
                    "_B______",
                    "_____G__",
                    "_R______",
                    "____Y___",
                    "___B__R_",
                    "____G___",
                    "________",
                ]
            ),
            string.Join(
                "\n",
                [
                    "__WWWWW_",
                    "WWW___W_",
                    "W_>b__W_",
                    "WWW_g_W_",
                    "W_WWy_WW",
                    "W_W____W",
                    "Wr_bgr_W",
                    "W______W",
                    "WWWWWWWW",
                ]
            )
        ),
    ];

    const int MaxHistory = 30;

    /// <summary>
    /// Determines whether another built-in level exists after the given one.
    /// </summary>
    /// <param name="levelNumber">The current zero-based level index.</param>
    /// <returns><see langword="true"/> if another level exists; otherwise <see langword="false"/>.</returns>
    public static bool HasNextLevel(int levelNumber) => levelNumber < levels.Count - 1;

    private readonly Board description;
    private readonly Board ground;

    private readonly IListOf<Board> descriptionHistory;

    private readonly Option<Point> playerPos;

    private readonly int levelNumber;

    /// <summary>Creates the default level.</summary>
    public Level()
        : this(0) { }

    /// <summary>Gets the level from the list of levels.</summary>
    /// <param name="levelNumber">The zero-based index of the built-in level to load.</param>
    public Level(int levelNumber)
        : this(levels[levelNumber].Item1, levels[levelNumber].Item2, levelNumber) { }

    /// <summary>Creates a new level from a given description and ground Board.</summary>
    /// <param name="description">The movable-object board.</param>
    /// <param name="ground">The ground board.</param>
    /// <param name="levelNumber">The zero-based level index.</param>
    public Level(
        Board description,
        Board ground,
        int levelNumber,
        IListOf<Board> descriptionHistory
    )
    {
        if (description.NRows != ground.NRows || description.NCols != ground.NCols)
        {
            throw new ArgumentException(
                "ground and description must have the same number of rows and columns"
            );
        }

        var playerPos = description
            .IndexOfWith((cell, _, _) => cell.IsPlayer())
            .Map((pos) => new Point(pos.Item1, pos.Item2));

        this.playerPos = playerPos;
        this.description = description;
        this.levelNumber = levelNumber;
        this.ground = ground;
        this.descriptionHistory = descriptionHistory.Take(MaxHistory);
    }

    /// <summary>
    /// Creates a new level from a ground and description string.
    /// </summary>
    /// <param name="ground">The ground of the level.</param>
    /// <param name="description">The description of the level.</param>
    /// <param name="levelNumber">The zero-based level index.</param>
    /// <exception cref="ArgumentException">Thrown if the parsed grids do not form matching rectangles.</exception>
    public Level(string ground, string description, int levelNumber)
        : this(
            ParseDescription(description),
            ParseGround(ground),
            levelNumber,
            new EmptyLo<Board>()
        ) { }

    /// <summary>
    /// Creates a new level from a given description and ground grids.
    /// </summary>
    /// <param name="description">The description of the level.</param>
    /// <param name="ground">The ground of the level.</param>
    /// <param name="levelNumber">The zero-based level index.</param>
    /// <exception cref="ArgumentException">Thrown if the ground and description have different numbers of rows and columns.</exception>
    public Level(
        Grid<ICell> description,
        Grid<ICell> ground,
        int levelNumber,
        IListOf<Board> descriptionHistory
    )
        : this(new Board(description), new Board(ground), levelNumber, descriptionHistory) { }

    /// <summary>Creates a new level from a given level and history.</summary>
    /// <param name="level">The level to copy.</param>
    /// <param name="groundHistory">The history of the level.</param>
    private Level(Level level, IListOf<Board> descriptionHistory)
        : this(level.description, level.ground, level.levelNumber, descriptionHistory) { }

    /// <summary>
    /// Moves the player in the specified direction.
    /// </summary>
    /// <param name="direction">The direction to move the player.</param>
    /// <returns>A new level with the player moved.</returns>
    public Level MovePlayer(Direction direction) =>
        playerPos.MapOr(this, (pos) => MoveCell(pos, direction, 2));

    /// <summary>Moves the description cell at the specified position in the specified direction.</summary>
    /// <param name="pos">The position of the cell to move.</param>
    /// <param name="direction">The direction to move the cell.</param>
    /// <param name="remainingMoves">The remaining moves.</param>
    /// <returns>A new level with the cell moved.</returns>
    public Level MoveCell(Point pos, Direction direction, int remainingMoves)
    {
        var nextPoint = Util.NextPoint(pos, direction);
        var nextDescCell = description.Get(nextPoint);

        return nextDescCell.MapOr(
            this,
            (nextDescCell) =>
                nextDescCell
                    .HandleSwap(this, nextPoint, pos, direction, remainingMoves)
                    .HandleGroundSwap(pos, direction, remainingMoves)
        );
    }

    /// <summary>
    /// Handles the ground swap at the specified position in the specified direction.
    /// It's essentially a side-effect of the swap based on the ground cell.
    /// </summary>
    /// <param name="pos">The position of the cell to swap.</param>
    /// <param name="direction">The direction to swap the cell.</param>
    /// <param name="remainingMoves">The remaining moves.</param>
    /// <returns>A new level with the cell swapped.</returns>
    public Level HandleGroundSwap(Point pos, Direction direction, int remainingMoves)
    {
        var nextPoint = Util.NextPoint(pos, direction);
        var nextGroundCell = ground.Get(nextPoint);

        return nextGroundCell.MapOr(
            this,
            (nextDescCell) =>
                nextDescCell.HandleGroundSwap(this, nextPoint, pos, direction, remainingMoves)
        );
    }

    /// <summary>Sets the description cell at the specified position to the specified cell.</summary>
    /// <param name="pos">The position of the cell to set.</param>
    /// <param name="cell">The cell to set.</param>
    /// <returns>A new level with the cell set.</returns>
    public Level SetCell(Point pos, ICell cell) =>
        new(description.Set(pos, cell), ground, levelNumber, descriptionHistory);

    /// <summary>Sets the ground cell at the specified position to the specified cell.</summary>
    /// <param name="pos">The position of the cell to set.</param>
    /// <param name="cell">The cell to set.</param>
    /// <returns>A new level with the cell set.</returns>
    public Option<ICell> GetCell(Point pos) => description.Get(pos);

    /// <summary>Swaps the description cell at the specified positions.</summary>
    /// <param name="pos1">The position of the first cell to swap.</param>
    /// <param name="pos2">The position of the second cell to swap.</param>
    /// <returns>A new level with the cells swapped.</returns>
    public Level SwapCell(Point pos1, Point pos2) =>
        new(description.Swap(pos1, pos2), ground, levelNumber, descriptionHistory);

    /// <summary>
    /// Reloads the current built-in level from its initial definition.
    /// </summary>
    /// <returns>A fresh copy of the current level.</returns>
    public Level Restart() => new(levelNumber);

    /// <summary>
    /// Pushes the current level into the history. if the contents are different. Will keep a history of 30 levels. (30 is an arbritrary number)
    /// </summary>
    /// <param name="newLevel">The new Level to add the history to.</param>
    /// <returns>A new level with the current level pushed into the history.</returns>
    public Level AddHistoryIfDifferent(Level newLevel) =>
        newLevel.description.Equals(description)
            ? this
            : new(newLevel, newLevel.descriptionHistory.PushFront(description));

    /// <summary>Undoes the last move.</summary>
    /// <returns>A new level with the last move undone.</returns>
    public Level Undo() =>
        descriptionHistory
            .Get(0)
            .MapOr(this, (desc) => new(desc, ground, levelNumber, descriptionHistory.PopFront()));

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
    /// Checks whether every target has a trophy of the matching color on it.
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
    /// Determines whether the current level has ended in a win or loss.
    /// </summary>
    /// <returns>A game-over reason when the level has ended; otherwise <see cref="None{T}" />.</returns>
    public Option<GameOverReason> EndOfLevel()
    {
        if (LevelWon())
        {
            return new Some<GameOverReason>(new Win(levelNumber));
        }

        if (playerPos.IsNone())
        {
            return new Some<GameOverReason>(
                new Lose("Game over, your player is lost!", levelNumber)
            );
        }

        return new None<GameOverReason>();
    }

    /// <summary>
    /// Returns a string representation of the level.
    /// </summary>
    /// <returns>A string representation of the level.</returns>
    public override string ToString() =>
        $"Level\n{ground.ToPrettyString()}\n{description.ToPrettyString()}";
}

