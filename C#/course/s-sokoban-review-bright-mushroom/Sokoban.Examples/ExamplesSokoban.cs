using System.Windows.Media;
using ImageLib;
using ImageLib.Enumerations;
using ImageLib.ImpWorld;
using TesterLib;
using Sokoban.Interfaces;
using Sokoban.Lists;
using Sokoban.Objects;


namespace Sokoban;

class ExamplesSokoban
{
    string exampleLevelGround =
        "________\n" +
        "___R____\n" +
        "________\n" +
        "_B____Y_\n" +
        "________\n" +
        "___G____\n" +
        "________";
    string exampleLevelContents =
        "__WWW___\n" +
        "__W_WW__\n" +
        "WWWr_WWW\n" +
        "W_b>yB_W\n" +
        "WW_gWWWW\n" +
        "_WW_W___\n" +
        "__WWW___";

    #region Test StringBoardParser
    public bool TestFullBoardParsing(Tester t)
    {
        return t.CheckNoException(() => new SokobanBoardManager(exampleLevelGround, exampleLevelContents));
    }

    public bool TestWidthMismatch(Tester t)
    {
        string ground = "____\n____";
        string contents = "____\n__";

        return t.CheckException(
            new ArgumentException(StringBoardParser.WIDTH_MISMATCH_ERROR_MSG),
            () => StringBoardParser.ParseBoard(ground, contents)
        );
    }

    public bool TestHeightMismatch(Tester t)
    {
        string ground = "____\n____";
        string contents = "____";

        return t.CheckException(
            new ArgumentException(StringBoardParser.HEIGHT_MISMATCH_ERROR_MSG),
            () => StringBoardParser.ParseBoard(ground, contents)
        );
    }

    public bool TestSingleTargetCell(Tester t)
    {
        string ground = "R";
        string contents = "_";

        return t.CheckNoException(() => StringBoardParser.ParseBoard(ground, contents));
    }

    public bool TestSingleRowLength(Tester t)
    {
        string ground = "____";
        string contents = "W_Bv";

        return t.CheckNoException(() =>
        {
            var row = StringBoardParser.ParseBoard(ground, contents);
            t.CheckExpect(row.Width(), 4);
        });
    }

    public bool TestInvalidGroundCharacter(Tester t)
    {
        string ground = "Z";
        string contents = "_";

        return t.CheckException(
            new ArgumentException("Unknown ground character : 'Z'"),
            () => StringBoardParser.ParseBoard(ground, contents)
        );
    }

    public bool TestInvalidContentCharacter(Tester t)
    {
        string ground = "____";
        string contents = "___o";

        return t.CheckException(
            new ArgumentException("Unknown content character : 'o'"),
            () => StringBoardParser.ParseBoard(ground, contents)
        );
    }
    public bool TestWhitespaceHandlingStillInvalid(Tester t)
    {
        string ground = " _ _ _ ";
        string contents = " W_B__ ";

        return t.CheckException(
            new ArgumentException("Unknown ground character : ' '"),
            () => StringBoardParser.ParseBoard(ground, contents)
        );
    }
    public bool TestWhiteSpaceHandlingValid(Tester t)
    {
        string ground = " ____ ";
        string contents = "W_Bv ";

        return t.CheckNoException(() => StringBoardParser.ParseBoard(ground, contents));
    }
    #endregion

    #region Test SokobanBoardManager StartGame and Drawing
    public bool TestStartExampleLevel(Tester t)
    {
        SokobanBoardManager board = new SokobanBoardManager(exampleLevelGround, exampleLevelContents);
        return t.CheckExpect(board.StartGame(), true);
    }

    public bool TestStartExampleLevelDownscaled(Tester t)
    {
        SokobanBoardManager board = new SokobanBoardManager(exampleLevelGround, exampleLevelContents, 20);
        return t.CheckExpect(board.StartGame(), true);
    }

    public bool TestStartExampleLevelUpscaled(Tester t)
    {
        SokobanBoardManager board = new SokobanBoardManager(exampleLevelGround, exampleLevelContents, 200);
        return t.CheckExpect(board.StartGame(), true);
    }

    public bool TestStartGameMovable(Tester t)
    {
        string exampleLevelGround =
            "________\n" +
            "___R____\n" +
            "________\n" +
            "_B____Y_\n" +
            "________\n" +
            "___G____\n" +
            "________";
        string exampleLevelContents =
            "________\n" +
            "________\n" +
            "_W___v__\n" +
            "_g______\n" +
            "__B_____\n" +
            "________\n" +
            "________";

        SokobanBoardManager board = new SokobanBoardManager(exampleLevelGround, exampleLevelContents);
        return t.CheckExpect(board.StartGame(), true);
    }
    #endregion

    #region Test SokobanBoardManager LevelWon

    public void TestWonLevel(Tester t)
    {
        string exampleLevelGround =
            "________\n" +
            "________";
        string exampleLevelContents =
            "________\n" +
            "________";

        SokobanBoardManager board = new SokobanBoardManager(exampleLevelGround, exampleLevelContents);
        t.CheckExpect(board.LevelWon(), true);
    }
    public void TestWonLevelWithTrophy(Tester t)
    {
        string exampleLevelGround =
            "__R_____\n" +
            "________";
        string exampleLevelContents =
            "__r_____\n" +
            "________";

        SokobanBoardManager board = new SokobanBoardManager(exampleLevelGround, exampleLevelContents);
        t.CheckExpect(board.LevelWon(), true);
    }
    public void TestWonLevelWithMultipleTargetAndTrophies(Tester t)
    {
        string exampleLevelGround =
            "__RRR___\n" +
            "________";
        string exampleLevelContents =
            "__rrr___\n" +
            "________";

        SokobanBoardManager board = new SokobanBoardManager(exampleLevelGround, exampleLevelContents);
        t.CheckExpect(board.LevelWon(), true);
    }
    public void TestWonLevelNotWon(Tester t)
    {
        string exampleLevelGround =
            "__RBG___\n" +
            "________";
        string exampleLevelContents =
            "__rrr___\n" +
            "________";

        SokobanBoardManager board = new SokobanBoardManager(exampleLevelGround, exampleLevelContents);
        t.CheckExpect(board.LevelWon(), false);
    }
    #endregion

    #region Test Cells

    // ===== DrawCell Tests =====
    public bool TestCellDrawCell_SmallSize(Tester t)
    {
        ICell floor = new Floor(new EmptyContent(), new Point(0, 0));
        WorldImage image = floor.DrawCell(32);
        return t.CheckExpect(image != null, true, "DrawCell with size 32 should return valid image");
    }

    public bool TestCellDrawCell_WithContent(Tester t)
    {
        ICell floorWithPlayer = new Floor(new Player(), new Point(1, 1));
        WorldImage playerImage = floorWithPlayer.DrawCell(50);

        ICell floorWithTrophy = new Floor(new Trophy(Colors.Red), new Point(2, 2));
        WorldImage trophyImage = floorWithTrophy.DrawCell(64);

        return t.CheckExpect(playerImage != null, true, "DrawCell with player content should render")
            && t.CheckExpect(trophyImage != null, true, "DrawCell with trophy content should render");
    }

    public bool TestCellDrawCell_TargetWithEmptyContent(Tester t)
    {
        ICell target = new Target(Colors.Blue, new EmptyContent(), new Point(3, 3));
        WorldImage image = target.DrawCell(50);
        return t.CheckExpect(image != null, true, "Target cell should render with empty content");
    }

    // ===== ContainsPlayer Tests =====
    public bool TestCellContainsPlayerOrTrophy(Tester t)
    {
        ICell floorWithPlayer = new Floor(new Player(), new Point(0, 0));
        ICell floorEmpty = new Floor(new EmptyContent(), new Point(0, 0));
        ICell floorWithTrophy = new Floor(new Trophy(Colors.Red), new Point(1, 1));

        return t.CheckExpect(floorWithPlayer.ContainsPlayer(), true, "Cell with player should contain player")
            && t.CheckExpect(floorEmpty.ContainsPlayer(), false, "Cell with empty content should not contain player")
            && t.CheckExpect(floorWithTrophy.ContainsPlayer(), false, "Cell with trophy should not contain player");
    }

    // ===== Position Tests =====
    public bool TestCellPosition(Tester t)
    {
        Point posOrigin = new Point(0, 0);
        Point posArbitrary = new Point(5, 7);

        ICell floor1 = new Floor(new EmptyContent(), posOrigin);
        ICell floor2 = new Floor(new EmptyContent(), posArbitrary);

        return t.CheckExpect(floor1.Position(), posOrigin, "Position at origin should return (0,0)")
            && t.CheckExpect(floor2.Position(), posArbitrary, "Position should return exact coordinates");
    }

    // ===== CanMoveTo Tests =====
    public bool TestCellCanMoveTo(Tester t)
    {
        ICell floorEmpty = new Floor(new EmptyContent(), new Point(0, 0));
        ICell floorWithPlayer = new Floor(new Player(), new Point(0, 0));
        ICell floorWithTrophy = new Floor(new Trophy(Colors.Green), new Point(1, 1));
        ICell targetWithEmpty = new Target(Colors.Red, new EmptyContent(), new Point(2, 2));

        return t.CheckExpect(floorEmpty.CanMoveTo(), true, "Cell with empty content should be movable")
            && t.CheckExpect(floorWithPlayer.CanMoveTo(), false, "Cell with player should not be movable")
            && t.CheckExpect(floorWithTrophy.CanMoveTo(), false, "Cell with trophy should not be movable")
            && t.CheckExpect(floorWithTrophy.ReplaceContent(new Wall()).CanMoveTo(), false, "Cell with wall should not be movable")
            && t.CheckExpect(targetWithEmpty.CanMoveTo(), true, "Target cell with empty content should be movable");
    }

    // ===== ReplaceContent Tests =====
    public bool TestCellReplaceContent(Tester t)
    {
        Point originalPosition = new Point(5, 5);
        ICell floor1 = new Floor(new EmptyContent(), new Point(0, 0));
        ICell newFloor1 = floor1.ReplaceContent(new Player());

        ICell floor2 = new Floor(new Player(), new Point(1, 1));
        ICell newFloor2 = floor2.ReplaceContent(new EmptyContent());

        ICell floor3 = new Floor(new EmptyContent(), originalPosition);
        ICell newFloor3 = floor3.ReplaceContent(new Player());

        ICell floor4 = new Floor(new Trophy(Colors.Red), new Point(2, 3));
        ICell newFloor4 = floor4.ReplaceContent(new Trophy(Colors.Blue));

        ICell target = new Target(Colors.Blue, new EmptyContent(), new Point(3, 3));
        ICell newTarget = target.ReplaceContent(new Trophy(Colors.Blue));

        return t.CheckExpect(newFloor1.ContainsPlayer(), true, "Replacing empty with player should work")
            && t.CheckExpect(newFloor2.ContainsPlayer(), false, "Replacing player with empty should work")
            && t.CheckExpect(newFloor3.Position(), originalPosition, "ReplaceContent should preserve position")
            && t.CheckExpect(newFloor4.CanMoveTo(), false, "Replacing trophy with another trophy should work")
            && t.CheckExpect(newTarget.HasTrophyOfSameColor(), true, "Target color should be preserved after replacement");
    }

    // ===== HasTrophyOfSameColor Tests =====

    public bool TestCellHasTrophyOfSameColor(Tester t)
    {
        return t.CheckExpect(new Target(Colors.Green, new EmptyContent(), new Point(2, 2)).HasTrophyOfSameColor(), false, "Target without trophy should return false")
            && t.CheckExpect(new Floor(new Trophy(Colors.Red), new Point(3, 3)).HasTrophyOfSameColor(), false, "Floor cell should always return false")
            && t.CheckExpect(new Target(Colors.Yellow, new Trophy(Colors.Yellow), new Point(0, 0)).HasTrophyOfSameColor(), true, "Yellow target with yellow trophy")
            && t.CheckExpect(new Target(Colors.Green, new Trophy(Colors.Green), new Point(1, 1)).HasTrophyOfSameColor(), true, "Green target with green trophy")
            && t.CheckExpect(new Target(Colors.Blue, new Trophy(Colors.Blue), new Point(2, 2)).HasTrophyOfSameColor(), true, "Blue target with blue trophy");
    }

    // ===== IsWon Tests =====
    public bool TestCellIsWon(Tester t)
    {
        ICell targetMatchingRed = new Target(Colors.Red, new Trophy(Colors.Red), new Point(0, 0));
        ICell targetNoTrophy = new Target(Colors.Blue, new EmptyContent(), new Point(1, 1));
        ICell targetWrongColor = new Target(Colors.Green, new Trophy(Colors.Red), new Point(2, 2));
        ICell targetMatchingBlue = new Target(Colors.Blue, new Trophy(Colors.Blue), new Point(4, 4));
        ICell floorAlwaysWon = new Floor(new EmptyContent(), new Point(5, 5));

        return t.CheckExpect(targetMatchingRed.IsWon(), true, "Target with matching trophy should indicate won")
            && t.CheckExpect(targetNoTrophy.IsWon(), false, "Target without trophy should not indicate won")
            && t.CheckExpect(targetWrongColor.IsWon(), false, "Target with wrong color trophy should not indicate won")

            && t.CheckExpect(targetMatchingBlue.IsWon(), true, "Blue target with blue trophy should indicate won")
            && t.CheckExpect(floorAlwaysWon.IsWon(), true, "Floor cells should always indicate won");
    }

    #endregion

    #region Test Content

    // ===== DrawContent Tests =====
    public bool TestContentDrawContent(Tester t)
    {
        IContent player = new Player();
        WorldImage playerImage = player.DrawContent(32);

        IContent empty = new EmptyContent();
        WorldImage emptyImage = empty.DrawContent(32);

        IContent trophy = new Trophy(Colors.Red);
        WorldImage trophyImage = trophy.DrawContent(64);

        return t.CheckExpect(playerImage != null, true, "DrawContent for player should return valid image")
            && t.CheckExpect(emptyImage != null, true, "DrawContent for empty should return valid image")
            && t.CheckExpect(trophyImage != null, true, "DrawContent for trophy should return valid image");
    }

    // ===== IsPlayer Tests =====
    public bool TestContentIsPlayer(Tester t)
    {
        IContent player = new Player();
        IContent empty = new EmptyContent();
        IContent trophy = new Trophy(Colors.Blue);

        return t.CheckExpect(player.IsPlayer(), true, "Player content should return true for IsPlayer()")
            && t.CheckExpect(empty.IsPlayer(), false, "Empty content should return false for IsPlayer()")
            && t.CheckExpect(trophy.IsPlayer(), false, "Trophy content should return false for IsPlayer()");
    }

    // ===== IsEmpty Tests =====
    public bool TestContentIsEmpty(Tester t)
    {
        IContent empty = new EmptyContent();
        IContent player = new Player();
        IContent trophy = new Trophy(Colors.Green);

        return t.CheckExpect(empty.IsEmpty(), true, "Empty content should return true for IsEmpty()")
            && t.CheckExpect(player.IsEmpty(), false, "Player content should return false for IsEmpty()")
            && t.CheckExpect(trophy.IsEmpty(), false, "Trophy content should return false for IsEmpty()");
    }

    // ===== IsTrophy Tests =====
    public bool TestContentIsTrophy(Tester t)
    {
        IContent trophy = new Trophy(Colors.Red);
        IContent player = new Player();
        IContent empty = new EmptyContent();

        return t.CheckExpect(trophy.IsTrophy(), true, "Trophy content should return true for IsTrophy()")
            && t.CheckExpect(player.IsTrophy(), false, "Player content should return false for IsTrophy()")
            && t.CheckExpect(empty.IsTrophy(), false, "Empty content should return false for IsTrophy()");
    }

    // ===== HasSameColor Tests =====
    public bool TestContentHasSameColor(Tester t)
    {
        IContent trophyRed = new Trophy(Colors.Red);
        IContent trophyBlue = new Trophy(Colors.Blue);
        IContent player = new Player();

        return t.CheckExpect(trophyRed.HasSameColor(Colors.Red), true, "Red trophy should match red color")
            && t.CheckExpect(trophyRed.HasSameColor(Colors.Blue), false, "Red trophy should not match blue color")
            && t.CheckExpect(trophyBlue.HasSameColor(Colors.Blue), true, "Blue trophy should match blue color")
            && t.CheckExpect(player.HasSameColor(Colors.Red), false, "Player content should return false for HasSameColor()");
    }

    #endregion

    #region Test ILoCell

    // ===== Length Tests =====
    public bool TestLoCellLength(Tester t)
    {
        ICell cell1 = new Floor(new EmptyContent(), new Point(0, 0));
        ICell cell2 = new Floor(new Player(), new Point(1, 0));
        ILoCell emptyRow = new EmptyLoCell();
        ILoCell singleCell = new LinkLoCell(cell1, emptyRow);
        ILoCell twoCell = new LinkLoCell(cell2, singleCell);

        return t.CheckExpect(emptyRow.Length(), 0, "Empty row should have length 0")
            && t.CheckExpect(singleCell.Length(), 1, "Single cell row should have length 1")
            && t.CheckExpect(twoCell.Length(), 2, "Two cell row should have length 2");
    }

    // ===== DrawLoCell Tests =====
    public bool TestLoCellDrawLoCell(Tester t)
    {
        ICell cell = new Floor(new EmptyContent(), new Point(0, 0));
        ILoCell row = new LinkLoCell(cell, new EmptyLoCell());
        WorldImage image = row.DrawLoCell(32);

        return t.CheckExpect(image != null, true, "DrawLoCell should return valid image");
    }

    // ===== ContainsPlayer Tests =====
    public bool TestLoCellContainsPlayer(Tester t)
    {
        ICell empty = new Floor(new EmptyContent(), new Point(0, 0));
        ICell withPlayer = new Floor(new Player(), new Point(1, 0));
        ILoCell emptyRow = new EmptyLoCell();
        ILoCell rowNoPlayer = new LinkLoCell(empty, emptyRow);
        ILoCell rowWithPlayer = new LinkLoCell(withPlayer, rowNoPlayer);

        return t.CheckExpect(emptyRow.ContainsPlayer(), false, "Empty row should not contain player")
            && t.CheckExpect(rowNoPlayer.ContainsPlayer(), false, "Row without player should return false")
            && t.CheckExpect(rowWithPlayer.ContainsPlayer(), true, "Row with player should return true");
    }

    // ===== CanMoveTo Tests =====
    public bool TestLoCellCanMoveTo(Tester t)
    {
        ICell movable = new Floor(new EmptyContent(), new Point(0, 0));
        ICell notMovable = new Floor(new Trophy(Colors.Red), new Point(1, 0));
        ILoCell row = new LinkLoCell(movable, new LinkLoCell(notMovable, new EmptyLoCell()));

        return t.CheckExpect(row.CanMoveTo(0), true, "Movable cell should allow movement")
            && t.CheckExpect(row.CanMoveTo(1), false, "Non-movable cell should not allow movement");
    }

    // ===== IsRowWon Tests =====
    public bool TestLoCellIsRowWon(Tester t)
    {
        ICell empty = new Floor(new EmptyContent(), new Point(0, 0));
        ICell targetWon = new Target(Colors.Red, new Trophy(Colors.Red), new Point(1, 0));
        ILoCell rowEmpty = new LinkLoCell(empty, new EmptyLoCell());
        ILoCell rowWon = new LinkLoCell(targetWon, new EmptyLoCell());

        return t.CheckExpect(rowEmpty.IsRowWon(), true, "Row with only floors should be won")
            && t.CheckExpect(rowWon.IsRowWon(), true, "Row with satisfied target should be won");
    }

    #endregion

    #region Test ILoloCell

    // ===== Dimensions Tests =====
    public bool TestLoLoCellDimensions(Tester t)
    {
        ICell cell = new Floor(new EmptyContent(), new Point(0, 0));
        ILoCell row1 = new LinkLoCell(cell, new EmptyLoCell());
        ILoCell row2 = new LinkLoCell(cell, row1);
        ILoloCell grid = new LinkLoloCell(row1, new LinkLoloCell(row2, new EmptyLoloCell()));

        return t.CheckExpect(grid.Width(), 1, "Grid width should match row length")
            && t.CheckExpect(grid.Height(), 2, "Grid height should match number of rows");
    }

    // ===== DrawLoLoCell Tests =====
    public bool TestLoLoCellDrawLoLoCell(Tester t)
    {
        ICell cell = new Floor(new EmptyContent(), new Point(0, 0));
        ILoCell row = new LinkLoCell(cell, new EmptyLoCell());
        ILoloCell grid = new LinkLoloCell(row, new EmptyLoloCell());
        WorldImage image = grid.DrawLoLoCell(32);

        return t.CheckExpect(image != null, true, "DrawLoLoCell should return valid image");
    }

    // ===== CanMoveTo Tests =====
    public bool TestLoLoCellCanMoveTo(Tester t)
    {
        string exampleLevelGround =
            "________\n" +
            "___R____\n" +
            "________\n" +
            "_B____Y_\n" +
            "________";
        string exampleLevelContents =
            "________\n" +
            "________\n" +
            "_W<_____\n" +
            "_g______\n" +
            "__B_____";
        ILoloCell grid = StringBoardParser.ParseBoard(exampleLevelGround, exampleLevelContents);

        return t.CheckExpect(grid.CanMoveTo(new Point(2, 1)), true, "Movable cell should allow movement")
            && t.CheckExpect(grid.CanMoveTo(new Point(1, 2)), false, "Non-movable cell should not allow movement");
    }

    // ===== IsLevelWon Tests =====
    public bool TestLoLoCellIsLevelWon(Tester t)
    {
        ICell empty = new Floor(new EmptyContent(), new Point(0, 0));
        ILoCell row = new LinkLoCell(empty, new EmptyLoCell());
        ILoloCell gridEmpty = new LinkLoloCell(row, new EmptyLoloCell());

        return t.CheckExpect(gridEmpty.IsLevelWon(), true, "Grid with only floors should be won");
    }

    // ===== GetRow Tests =====
    public bool TestLoLoCellGetRow(Tester t)
    {
        string exampleLevelGround =
            "___\n" +
            "___\n" +
            "___\n" +
            "_B_\n" +
            "___";
        string exampleLevelContents =
            "___\n" +
            "___\n" +
            "_W<\n" +
            "_g_\n" +
            "__B";
        ILoloCell grid = StringBoardParser.ParseBoard(exampleLevelGround, exampleLevelContents);

        ILoCell rowWithOnlyFloors = new LinkLoCell(new Floor(new EmptyContent(), new Point(0, 1)),
            new LinkLoCell(new Floor(new EmptyContent(), new Point(1, 1)),
            new LinkLoCell(new Floor(new EmptyContent(), new Point(2, 1)),
            new EmptyLoCell())));
        ILoCell rowWithStuff = new LinkLoCell(new Floor(new EmptyContent(), new Point(0, 2)),
            new LinkLoCell(new Floor(new Wall(), new Point(1, 2)),
            new LinkLoCell(new Floor(new Player(), new Point(2, 2)),
            new EmptyLoCell())));

        return t.CheckExpect(grid.GetRow(5), new EmptyLoCell(), "Retrieving a row outside the grid returns an empty row")
            && t.CheckExpect(grid.GetRow(1), rowWithOnlyFloors, "Retrieving a row that is only empty floor returns that row")
            && t.CheckExpect(grid.GetRow(2), rowWithStuff, "Retrieving a row with stuff returns that row with stuff");
    }

    #endregion

}


