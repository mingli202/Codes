using System.Reflection;
using TesterLib;
using Sokoban.Cells;
using Sokoban.Enum;
using Sokoban.Lists;

namespace Sokoban;

#pragma warning disable CA1822, IDE0051
/// <summary>
/// Example-based tests for implementation components.
/// </summary>
class ExamplesImplementation
{
    /// <summary>
    /// Tests Any on a list.
    /// </summary>
    /// <param name="t">The tester instance.</param>
    /// <returns><see langword="true"/> when the expectations pass.</returns>
    bool TestAny(Tester t)
    {
        var list = FromArray([1, 2, 3, 4, 5, 6, 7]);
        return t.CheckExpect(list.Any((item, _) => item % 2 == 0), true)
            && t.CheckExpect(list.Any((_, i) => i == 6), true);
    }

    /// <summary>
    /// Tests All on a list.
    /// </summary>
    /// <param name="t">The tester instance.</param>
    /// <returns><see langword="true"/> when the expectations pass.</returns>
    bool TestAll(Tester t)
    {
        var list = FromArray([0, 1, 2, 3, 4, 5, 6]);
        return t.CheckExpect(list.All((item, i) => item == i), true)
            && t.CheckExpect(list.All((item, _) => item == 3), false);
    }

    /// <summary>
    /// Tests SplitAt on a list.
    /// </summary>
    /// <param name="t">The tester instance.</param>
    /// <returns><see langword="true"/> when the expectations pass.</returns>
    bool TestListSplitAt(Tester t)
    {
        var list = FromArray([1, 2, 3, 4, 2, 5, 6, 7]);
        var split = list.SplitAt(2);

        var expected = FromArray([FromArray([1]), FromArray([3, 4]), FromArray([5, 6, 7])]);
        return t.CheckExpect(split.ToString(), expected.ToString());
    }

    /// <summary>
    /// Tests Fold with a numeric accumulator.
    /// </summary>
    /// <param name="t">The tester instance.</param>
    /// <returns><see langword="true"/> when the expectations pass.</returns>
    bool TestFold1(Tester t)
    {
        var list = FromArray([1, 2, 3, 4, 5, 6, 7]);
        var result = list.Fold(0, (acc, item, index) => acc + item);
        return t.CheckExpect(result, 28);
    }

    /// <summary>
    /// Tests Fold with list construction.
    /// </summary>
    /// <param name="t">The tester instance.</param>
    /// <returns><see langword="true"/> when the expectations pass.</returns>
    bool TestFold2(Tester t)
    {
        var list = FromArray([1, 2, 3, 4, 5, 6, 7]);
        IListOf<int> initial = new EmptyLo<int>();
        var result = list.Fold(initial, (acc, item, _) => new LinkLo<int>(item * 2, acc));
        return t.CheckExpect(result.ToString(), "[2, 4, 6, 8, 10, 12, 14]");
    }

    /// <summary>
    /// Tests Map on a list.
    /// </summary>
    /// <param name="t">The tester instance.</param>
    /// <returns><see langword="true"/> when the expectations pass.</returns>
    bool TestMap(Tester t)
    {
        var list = FromArray([1, 2, 3, 4, 5, 6, 7]);
        var result = list.Map((item, _) => item * 2);
        return t.CheckExpect(result.ToString(), "[2, 4, 6, 8, 10, 12, 14]");
    }

    /// <summary>
    /// Tests Filter on a list.
    /// </summary>
    /// <param name="t">The tester instance.</param>
    /// <returns><see langword="true"/> when the expectations pass.</returns>
    bool TestFilter(Tester t)
    {
        var list = FromArray([1, 2, 3, 4, 5, 6, 7]);
        var result = list.Filter((item, _) => item % 2 == 0);
        return t.CheckExpect(result.ToString(), "[2, 4, 6]");
    }

    /// <summary>
    /// Tests Get on a list.
    /// </summary>
    /// <param name="t">The tester instance.</param>
    /// <returns><see langword="true"/> when the expectations pass.</returns>
    bool TestGet(Tester t)
    {
        var list = FromArray([1, 2, 3, 4, 5, 6, 7]);
        return t.CheckExpect(list.Get(2), new Some<int>(3))
            && t.CheckExpect(list.Get(6), new Some<int>(7));
    }

    // bool TestBoardGetCell(Tester t)
    // {
    //     var list = FromArray([
    //             FromArray(['_', 'W', '_']).Map((item, _) => Cell.FromDescription(item)),
    //             FromArray(['r', 'g', '_']).Map((item, _) => Cell.FromDescription(item)),
    //             FromArray(['_', '_', 'y']).Map((item, _) => Cell.FromDescription(item))
    //     ]);
    //     var board = new Board(list);
    //     return t.CheckExpect(board.Get(0, 1), new Some<Cell>(new Wall()))
    //         && t.CheckExpect(board.Get(1, 0), new Some<Cell>(new Trophy('R')))
    //         && t.CheckExpect(board.Get(1, 1), new Some<Cell>(new Trophy('G')));
    // }

    /// <summary>
    /// Tests trophy-on-target logic.
    /// </summary>
    /// <param name="t">The tester instance.</param>
    /// <returns><see langword="true"/> when the expectations pass.</returns>
    bool TestTrophyOnTarget(Tester t)
    {
        var trophy = new Trophy('Y');
        var trophy2 = new Trophy('G');

        var target = new Target('Y');
        var crate = new Crate();
        var player = new Player('^');
        var wall = new Wall();
        var blank = new Blank();

        return t.CheckExpect(trophy.IsTrophyOnTarget(target), true)
            && t.CheckExpect(crate.IsTrophyOnTarget(player), true)
            && t.CheckExpect(blank.IsTrophyOnTarget(trophy), true)
            && t.CheckExpect(target.IsTrophyOnTarget(wall), false)
            && t.CheckExpect(target.IsTrophyOnTarget(trophy), true)
            && t.CheckExpect(target.IsTrophyOnTarget(trophy2), false);
    }

    /// <summary>
    /// Tests that a player can move into a blank cell.
    /// </summary>
    /// <param name="t">The tester instance.</param>
    /// <returns><see langword="true"/> when the expectations pass.</returns>
    bool TestMovePlayerIntoBlank(Tester t)
    {
        var board = BoardFromDescription([">__"]);
        var ground = BoardFromGround(["___"]);
        var level = new Level(ground, board, 0, new EmptyLo<Board>());
        var moved = level.MovePlayer(Direction.Right);

        return t.CheckExpect(CellAt(moved, 0, 0).Equals(new Blank()), true)
            && t.CheckExpect(CellAt(moved, 0, 1).Equals(new Player('>')), true)
            && t.CheckExpect(CellAt(moved, 0, 2).Equals(new Blank()), true);
    }

    /// <summary>
    /// Tests that a wall blocks player movement.
    /// </summary>
    /// <param name="t">The tester instance.</param>
    /// <returns><see langword="true"/> when the expectations pass.</returns>
    bool TestMovePlayerIntoWall(Tester t)
    {
        var board = BoardFromDescription([">W_"]);
        var ground = BoardFromGround(["___"]);
        var level = new Level(ground, board, 0, new EmptyLo<Board>());
        var moved = level.MovePlayer(Direction.Right);

        return t.CheckExpect(moved.ToString(), board.ToString());
    }

    /// <summary>
    /// Tests that a player can push a crate into a blank cell.
    /// </summary>
    /// <param name="t">The tester instance.</param>
    /// <returns><see langword="true"/> when the expectations pass.</returns>
    bool TestPushCrateIntoBlank(Tester t)
    {
        var board = BoardFromDescription([">B_"]);
        var ground = BoardFromGround(["___"]);
        var level = new Level(ground, board, 0, new EmptyLo<Board>());
        var moved = level.MovePlayer(Direction.Right);

        return t.CheckExpect(CellAt(moved, 0, 0).Equals(new Blank()), true)
            && t.CheckExpect(CellAt(moved, 0, 1).Equals(new Player('>')), true)
            && t.CheckExpect(CellAt(moved, 0, 2).Equals(new Crate()), true);
    }

    /// <summary>
    /// Tests that a crate cannot be pushed through a wall.
    /// </summary>
    /// <param name="t">The tester instance.</param>
    /// <returns><see langword="true"/> when the expectations pass.</returns>
    bool TestPushCrateIntoWall(Tester t)
    {
        var board = BoardFromDescription([">BW"]);
        var ground = BoardFromGround(["___"]);
        var level = new Level(ground, board, 0, new EmptyLo<Board>());
        var moved = level.MovePlayer(Direction.Right);

        return t.CheckExpect(moved.ToString(), board.ToString());
    }

    /// <summary>
    /// Tests that pushing a crate into a hole removes the crate.
    /// </summary>
    /// <param name="t">The tester instance.</param>
    /// <returns><see langword="true"/> when the expectations pass.</returns>
    bool TestPushCrateIntoHole(Tester t)
    {
        var board = BoardFromDescription([">BH"]);
        var ground = BoardFromGround(["___"]);
        var level = new Level(ground, board, 0, new EmptyLo<Board>());
        var moved = level.MovePlayer(Direction.Right);

        return t.CheckExpect(CellAt(moved, 0, 0).Equals(new Blank()), true)
            && t.CheckExpect(CellAt(moved, 0, 1).Equals(new Player('>')), true)
            && t.CheckExpect(CellAt(moved, 0, 2).Equals(new Blank()), true);
    }

    /// <summary>
    /// Tests that moving a player into a hole loses the level.
    /// </summary>
    /// <param name="t">The tester instance.</param>
    /// <returns><see langword="true"/> when the expectations pass.</returns>
    bool TestMovePlayerIntoHoleEndsLevel(Tester t)
    {
        var level = LevelFromRows(["R__"], [">H_"]).MovePlayer(Direction.Right);
        var end = level.EndOfLevel();

        return t.CheckExpect(end.IsSome(), true)
            && t.CheckExpect(end.Map((reason) => reason is Lose).UnwrapOr(false), true)
            && t.CheckExpect(
                end.Map((reason) => reason.Message).UnwrapOr(""),
                "Game over, your player is lost!"
            );
    }

    /// <summary>
    /// Tests a level win condition case.
    /// </summary>
    /// <param name="t">The tester instance.</param>
    /// <returns><see langword="true"/> when the expectations pass.</returns>
    bool TestLevelWon1(Tester t)
    {
        string ground = string.Join(
            "\n",
            ["________", "___R____", "________", "_B____Y_", "________", "___G____", "________"]
        );
        string description = string.Join(
            "\n",
            ["__WWW___", "__W_WW__", "WWWr_WWW", "W_b>yB_W", "WW_gWWWW", "_WW_W___", "__WWW___"]
        );
        var level = new Level(ground, description, 0);

        return t.CheckExpect(level.LevelWon(), false);
    }

    /// <summary>
    /// Tests several level win condition cases.
    /// </summary>
    /// <param name="t">The tester instance.</param>
    /// <returns><see langword="true"/> when the expectations pass.</returns>
    bool TestLevelWon2(Tester t)
    {
        string ground = string.Join(
            "\n",
            ["________", "___R____", "________", "_B____Y_", "________", "___G____", "________"]
        );
        string description = string.Join(
            "\n",
            ["__WWW___", "__WrWW__", "WWW__WWW", "Wb_>_ByW", "WW__WWWW", "_WWgW___", "__WWW___"]
        );

        string redTrophyNotOnTarget = string.Join(
            "\n",
            ["__WWW___", "__W_WW__", "WWWr_WWW", "Wb_>_ByW", "WW__WWWW", "_WWgW___", "__WWW___"]
        );

        string manyTrophies = string.Join(
            "\n",
            ["__WWW___", "g_WrWWb_", "WWWrrWWW", "Wb_>_ByW", "WW__WWWW", "gWWgW_b_", "__WWW___"]
        );

        var level = new Level(ground, description, 0);
        var level2 = new Level(ground, redTrophyNotOnTarget, 0);
        var level3 = new Level(ground, manyTrophies, 0);

        return t.CheckExpect(level.LevelWon(), true)
            && t.CheckExpect(level2.LevelWon(), false)
            && t.CheckExpect(level3.LevelWon(), true);
    }

    /// <summary>
    /// Tests that an unfinished level reports no end-of-level result.
    /// </summary>
    /// <param name="t">The tester instance.</param>
    /// <returns><see langword="true"/> when the expectations pass.</returns>
    bool TestEndOfLevelNone(Tester t)
    {
        var level = LevelFromRows(["R__"], [">__"]);
        return t.CheckExpect(level.EndOfLevel().IsNone(), true);
    }

    /// <summary>
    /// Tests menu navigation and selection actions.
    /// </summary>
    /// <param name="t">The tester instance.</param>
    /// <returns><see langword="true"/> when the expectations pass.</returns>
    bool TestMenuStateTransitions(Tester t)
    {
        var playMenu = new Menu();
        var quitMenu = new Menu();

        var playAction = playMenu.HandleEvent(new KeyDown("Enter"));
        quitMenu.HandleEvent(new KeyDown("Down"));
        var quitAction = quitMenu.HandleEvent(new KeyDown("Enter"));

        return t.CheckExpect(playAction is SwitchState, true)
            && t.CheckExpect(playAction is SwitchState(Playing), true)
            && t.CheckExpect(quitAction is Quit, true);
    }

    /// <summary>
    /// Tests direct actions handled by the playing state.
    /// </summary>
    /// <param name="t">The tester instance.</param>
    /// <returns><see langword="true"/> when the expectations pass.</returns>
    bool TestPlayingHandleEventActions(Tester t)
    {
        var playing = new Playing(0);

        return t.CheckExpect(playing.HandleEvent(new KeyDown("q")) is Quit, true)
            && t.CheckExpect(playing.HandleEvent(new KeyDown("Right")) is Nothing, true)
            && t.CheckExpect(playing.HandleEvent(new KeyDown("Tab")) is Nothing, true);
    }

    /// <summary>
    /// Tests playing-state tick transitions for win and loss outcomes.
    /// </summary>
    /// <param name="t">The tester instance.</param>
    /// <returns><see langword="true"/> when the expectations pass.</returns>
    bool TestPlayingOnTickStateChanges(Tester t)
    {
        var winningLevel = LevelFromRows(["R__"], ["r>_"]);
        var losingLevel = LevelFromRows(["R__"], ["___"]);

        var winningState = PlayingWithLevel(winningLevel);
        var losingState = PlayingWithLevel(losingLevel);

        var winningAction = winningState.HandleEvent(new Tick());
        var losingAction = losingState.HandleEvent(new Tick());

        return t.CheckExpect(winningAction is SwitchState, true)
            && t.CheckExpect(GetSwitchedState(winningAction) is GameOver, true)
            && t.CheckExpect(GetGameOverReason(GetSwitchedState(winningAction)) is Win, true)
            && t.CheckExpect(losingAction is SwitchState, true)
            && t.CheckExpect(GetSwitchedState(losingAction) is GameOver, true)
            && t.CheckExpect(GetGameOverReason(GetSwitchedState(losingAction)) is Lose, true);
    }

    /// <summary>
    /// Tests game-over state transitions for win, retry, and quit actions.
    /// </summary>
    /// <param name="t">The tester instance.</param>
    /// <returns><see langword="true"/> when the expectations pass.</returns>
    bool TestGameOverStateTransitions(Tester t)
    {
        var winState = new GameOver(new Win(0), 0);
        var lastWinState = new GameOver(new Win(1), 0);
        var loseState = new GameOver(new Lose("lost", 0), 0);

        var nextAction = winState.HandleEvent(new KeyDown("Tab"));
        var finalAction = lastWinState.HandleEvent(new KeyDown("Tab"));
        var retryAction = loseState.HandleEvent(new KeyDown("Tab"));
        var quitAction = loseState.HandleEvent(new KeyDown("q"));

        return t.CheckExpect(nextAction is SwitchState, true)
            && t.CheckExpect(nextAction is SwitchState(Playing), true)
            && t.CheckExpect(finalAction is Quit, true)
            && t.CheckExpect(retryAction is SwitchState, true)
            && t.CheckExpect(retryAction is SwitchState(Playing), true)
            && t.CheckExpect(quitAction is Quit, true);
    }

    /// <summary>
    /// Tests grid swapping.
    /// </summary>
    /// <param name="t">The tester instance.</param>
    /// <returns><see langword="true"/> when the expectations pass.</returns>
    bool TestSwap(Tester t)
    {
        var grid = new Grid<int>(
            FromArray([FromArray([1, 2, 3]), FromArray([2, 3, 4]), FromArray([5, 6, 7])])
        );
        var grid2 = grid.Swap(0, 1, 1, 2);
        var grid3 = grid2.Swap(0, 1, 1, 2);
        return t.CheckExpect(grid2.ToString(), "[[1, 4, 3], [2, 3, 2], [5, 6, 7]]")
            && t.CheckExpect(grid3.ToString(), grid.ToString());
    }

    /// <summary>
    /// Builds a list from an array.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="array">The source array.</param>
    /// <param name="index">The current index being copied.</param>
    /// <returns>A functional list containing the array elements.</returns>
    IListOf<T> FromArray<T>(T[] array, int index = 0) =>
        index >= array.Length
            ? new EmptyLo<T>()
            : new LinkLo<T>(array[index], FromArray(array, index + 1));

    /// <summary>
    /// Builds a board from description rows.
    /// </summary>
    /// <param name="rows">The description rows.</param>
    /// <returns>The parsed board.</returns>
    Board BoardFromDescription(string[] rows) =>
        new(
            FromArray(rows).Map(
                (row, _) => Util.ListOfCharFrom(row).Map((c, _) => Cell.FromDescription(c))
            )
        );

    /// <summary>
    /// Builds a board from ground rows.
    /// </summary>
    /// <param name="rows">The description rows.</param>
    /// <returns>The parsed board.</returns>
    Board BoardFromGround(string[] rows) =>
        new(
            FromArray(rows).Map(
                (row, _) => Util.ListOfCharFrom(row).Map((c, _) => Cell.FromGround(c))
            )
        );

    /// <summary>
    /// Builds a level from ground and description rows.
    /// </summary>
    /// <param name="groundRows">The ground rows.</param>
    /// <param name="descriptionRows">The description rows.</param>
    /// <param name="levelNumber">The level number to assign.</param>
    /// <returns>The parsed level.</returns>
    Level LevelFromRows(string[] groundRows, string[] descriptionRows, int levelNumber = 0) =>
        new(string.Join("\n", groundRows), string.Join("\n", descriptionRows), levelNumber);

    /// <summary>
    /// Gets the cell at the specified board position.
    /// </summary>
    /// <param name="board">The board to inspect.</param>
    /// <param name="row">The row index.</param>
    /// <param name="col">The column index.</param>
    /// <returns>The cell at the given position.</returns>
    ICell CellAt(Board board, int row, int col) =>
        board.Get(row, col).Expect($"Missing cell at ({row}, {col})");

    /// <summary>
    /// Gets the cell at the specified description board position.
    /// </summary>
    /// <param name="board">The board to inspect.</param>
    /// <param name="row">The row index.</param>
    /// <param name="col">The column index.</param>
    /// <returns>The cell at the given position.</returns>
    ICell CellAt(Level level, int row, int col) =>
        level.GetCell(new Point(row, col)).Expect($"Missing cell at ({row}, {col})");

    /// <summary>
    /// Creates a playing state whose current level is replaced with the given test level.
    /// </summary>
    /// <param name="level">The level to inject.</param>
    /// <param name="levelNumber">The level number to construct the state with.</param>
    /// <returns>A playing state using the supplied level.</returns>
    Playing PlayingWithLevel(Level level, int levelNumber = 0)
    {
        var playing = new Playing(levelNumber);
        typeof(Playing)
            .GetField("level", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(playing, level);
        return playing;
    }

    /// <summary>
    /// Extracts the target state from a state-switch action.
    /// </summary>
    /// <param name="action">The action to inspect.</param>
    /// <returns>The target state carried by the action.</returns>
    GameState GetSwitchedState(Action action) =>
        (GameState)(
            typeof(SwitchState).GetProperty("state")
                ?? typeof(SwitchState).GetProperty("State")
                ?? throw new MissingMemberException(typeof(SwitchState).FullName, "state")
        ).GetValue(action)!;

    /// <summary>
    /// Extracts the game-over reason from a game-over state.
    /// </summary>
    /// <param name="state">The game-over state to inspect.</param>
    /// <returns>The stored game-over reason.</returns>
    GameOverReason GetGameOverReason(GameState state) =>
        (GameOverReason)(
            typeof(GameOver).GetProperty("Reason")?.GetValue(state)
            ?? typeof(GameOver)
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .FirstOrDefault((field) => typeof(GameOverReason).IsAssignableFrom(field.FieldType))
                ?.GetValue(state)
            ?? throw new MissingMemberException(typeof(GameOver).FullName, "Reason")
        );

    /// <summary>
    /// Tests list construction from an array.
    /// </summary>
    /// <param name="t">The tester instance.</param>
    /// <returns><see langword="true"/> when the expectations pass.</returns>
    bool TestFromArray(Tester t)
    {
        var list = FromArray([1, 2, 3]);
        return t.CheckExpect(list.ToString(), "[1, 2, 3]");
    }
}

