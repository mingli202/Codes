using TesterLib;
using Sokoban.cells;
using Sokoban.Colors;
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
    bool TestAny(Tester t)
    {
        var list = FromArray([1, 2, 3, 4, 5, 6, 7]);
        return t.CheckExpect(list.Any((item, _) => item % 2 == 0), true)
            && t.CheckExpect(list.Any((_, i) => i == 6), true);
    }

    /// <summary>
    /// Tests All on a list.
    /// </summary>
    bool TestAll(Tester t)
    {
        var list = FromArray([0, 1, 2, 3, 4, 5, 6]);
        return t.CheckExpect(list.All((item, i) => item == i), true)
            && t.CheckExpect(list.All((item, _) => item == 3), false);
    }

    /// <summary>
    /// Tests SplitAt on a list.
    /// </summary>
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
    bool TestFold1(Tester t)
    {
        var list = FromArray([1, 2, 3, 4, 5, 6, 7]);
        var result = list.Fold(0, (acc, item, index) => acc + item);
        return t.CheckExpect(result, 28);
    }

    /// <summary>
    /// Tests Fold with list construction.
    /// </summary>
    bool TestFold2(Tester t)
    {
        var list = FromArray([1, 2, 3, 4, 5, 6, 7]);
        ListOf<int> initial = new EmptyLo<int>();
        var result = list.Fold(initial, (acc, item, _) => new LinkLo<int>(item * 2, acc));
        return t.CheckExpect(result.ToString(), "[2, 4, 6, 8, 10, 12, 14]");
    }

    /// <summary>
    /// Tests Map on a list.
    /// </summary>
    bool TestMap(Tester t)
    {
        var list = FromArray([1, 2, 3, 4, 5, 6, 7]);
        var result = list.Map((item, _) => item * 2);
        return t.CheckExpect(result.ToString(), "[2, 4, 6, 8, 10, 12, 14]");
    }

    /// <summary>
    /// Tests Filter on a list.
    /// </summary>
    bool TestFilter(Tester t)
    {
        var list = FromArray([1, 2, 3, 4, 5, 6, 7]);
        var result = list.Filter((item, _) => item % 2 == 0);
        return t.CheckExpect(result.ToString(), "[2, 4, 6]");
    }

    /// <summary>
    /// Tests Get on a list.
    /// </summary>
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
    /// Tests color equality.
    /// </summary>
    bool TestSameColor(Tester t)
    {
        var colorA = new Red();
        var colorB = new Red();
        var colorC = new Green();
        return t.CheckExpect(colorA.Equals(colorB), true)
            && t.CheckExpect(colorA.Equals(colorC), false);
    }

    /// <summary>
    /// Tests trophy-on-target logic.
    /// </summary>
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
    /// Tests a level win condition case.
    /// </summary>
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
        var level = new Level(ground, description);

        return t.CheckExpect(level.LevelWon(), false);
    }

    /// <summary>
    /// Tests several level win condition cases.
    /// </summary>
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

        var level = new Level(ground, description);
        var level2 = new Level(ground, redTrophyNotOnTarget);
        var level3 = new Level(ground, manyTrophies);

        return t.CheckExpect(level.LevelWon(), true)
            && t.CheckExpect(level2.LevelWon(), false)
            && t.CheckExpect(level3.LevelWon(), true);
    }

    /// <summary>
    /// Tests grid swapping.
    /// </summary>
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
    ListOf<T> FromArray<T>(T[] array, int index = 0) =>
        index >= array.Length
            ? new EmptyLo<T>()
            : new LinkLo<T>(array[index], FromArray(array, index + 1));

    /// <summary>
    /// Tests list construction from an array.
    /// </summary>
    bool TestFromArray(Tester t)
    {
        var list = FromArray([1, 2, 3]);
        return t.CheckExpect(list.ToString(), "[1, 2, 3]");
    }
}

