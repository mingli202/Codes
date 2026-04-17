using System.Windows.Media;
using ImageLib;
using ImageLib.Enumerations;
using Sokoban.Enum;

namespace Sokoban;

/// <summary>
/// Defines the behavior required for a renderable, event-driven game state.
/// </summary>
public interface IGameState
{
    /// <summary>
    /// Renders the state into an image that fits the given window size.
    /// </summary>
    /// <param name="windowSize">The size of the square window.</param>
    /// <returns>The rendered state image.</returns>
    WorldImage Render(int windowSize);

    /// <summary>
    /// Handles an input event and produces a resulting action.
    /// </summary>
    /// <param name="e">The event to process.</param>
    /// <returns>The action produced by handling the event.</returns>
    Action HandleEvent(Event e);
}

/// <summary>
/// Base class for all Sokoban game states.
/// </summary>
public abstract class GameState(int moves) : IGameState
{
    protected int moves = moves;
    /// <summary>
    /// Creates the initial game state.
    /// </summary>
    /// <returns>The menu state.</returns>
    public static GameState New() => new Menu();

    /// <inheritdoc />
    public virtual WorldImage Render(int windowSize) =>
        new RectangleImage(windowSize, windowSize, OutlineMode.Fill, Colors.White);

    /// <inheritdoc />
    public virtual Action HandleEvent(Event e) => new Nothing();
};

/// <summary>
/// Main menu state where the player can start or quit the game.
/// </summary>
public class Menu() : GameState(0)
{
    /// <summary>
    /// Identifies which menu button is currently selected.
    /// </summary>
    enum FocusedButton
    {
        Play,
        Quit,
    }

    private FocusedButton _focusedButton = FocusedButton.Play;

    /// <inheritdoc />
    public override WorldImage Render(int windowSize)
    {
        var background = new RectangleImage(windowSize, windowSize, OutlineMode.Fill, Colors.White);

        var playText = new TextImage(
            "Play",
            18,
            _focusedButton == FocusedButton.Play ? Colors.White : Colors.Black
        );
        var playButton = new OverlayImage(
            playText,
            new RectangleImage(
                (int)playText.Width + 10,
                (int)playText.Height + 10,
                OutlineMode.Fill,
                _focusedButton == FocusedButton.Play ? Colors.Black : Colors.White
            )
        );

        var quitText = new TextImage(
            "Quit",
            18,
            _focusedButton == FocusedButton.Quit ? Colors.White : Colors.Black
        );
        var quitButton = new OverlayImage(
            quitText,
            new RectangleImage(
                (int)quitText.Width + 10,
                (int)quitText.Height + 10,
                OutlineMode.Fill,
                _focusedButton == FocusedButton.Quit ? Colors.Black : Colors.White
            )
        );

        var buttons = new AboveImage(playButton, quitButton);

        var bottomMenu = new TextImage(
            "Use Up/Down to navigate, Enter to select",
            18,
            Colors.Black
        );

        var image = new OverlayImage(buttons, background);

        return new OverlayOffsetAlignImage(
            AlignModeX.Center,
            AlignModeY.Bottom,
            bottomMenu,
            0,
            bottomMenu.Height,
            image
        );
    }

    /// <inheritdoc />
    public override Action HandleEvent(Event e)
    {
        switch (e)
        {
            case KeyDown key when key.Key == "Up":
                _focusedButton = FocusedButton.Play;
                return new Render();
            case KeyDown key when key.Key == "Down":
                _focusedButton = FocusedButton.Quit;
                return new Render();
            case KeyDown key when key.Key == "Enter":
                switch (_focusedButton)
                {
                    case FocusedButton.Play:
                        return new SwitchState(new Playing(0));
                    case FocusedButton.Quit:
                        return new Quit();
                    default:
                        break;
                }
                break;
            default:
                break;
        }

        return new Nothing();
    }
}

/// <summary>
/// Active gameplay state for a specific level.
/// </summary>
/// <param name="levelNumber">The zero-based index of the level being played.</param>
public class Playing(int levelNumber) : GameState(0)
{
    private Level level = new(levelNumber);

    /// <inheritdoc />
    public override WorldImage Render(int windowSize)
    {
        var background = new RectangleImage(windowSize, windowSize, OutlineMode.Fill, Colors.White);

        var movesText = new TextImage($"Moves: {moves}", 18, Colors.Black);

        var levelImage = level.Render();
        var levelText = new TextImage($"Level {levelNumber + 1}", 18, Colors.Black);
        var controlsText = new TextImage(
            "Navigate <Up/Down/Left/Right>  Quit <q>  Restart <Tab>  Undo <u>",
            18,
            Colors.Black
        );
        var bottomMenu = new AboveImage(levelText, controlsText);

        var boardRemainingHeight = background.Height - bottomMenu.Height - movesText.Height;

        var imageWithMenu = new AboveImage(
            Util.ScaleImage(levelImage, (int)boardRemainingHeight),
            bottomMenu
        );

        var imageWithMoves = new AboveImage(movesText, imageWithMenu);

        return new OverlayImage(
            imageWithMoves,
            new RectangleImage(windowSize, windowSize, OutlineMode.Fill, Colors.White)
        );
    }

    /// <inheritdoc />
    public override Action HandleEvent(Event e)
    {
        switch (e)
        {
            case KeyDown key when key.Key == "Up":
                level = level.AddHistoryIfDifferent(level.MovePlayer(Direction.Up));
                moves += 1;
                return new Render();
            case KeyDown key when key.Key == "Down":
                level = level.AddHistoryIfDifferent(level.MovePlayer(Direction.Down));
                moves += 1;
                return new Render();
            case KeyDown key when key.Key == "Left":
                level = level.AddHistoryIfDifferent(level.MovePlayer(Direction.Left));
                moves += 1;
                return new Render();
            case KeyDown key when key.Key == "Right":
                level = level.AddHistoryIfDifferent(level.MovePlayer(Direction.Right));
                moves += 1;
                return new Render();
            case KeyDown key when key.Key == "q":
                return new Quit();
            case KeyDown key when key.Key == "Tab":
                level = new Level(levelNumber);
                moves = 0;
                return new Render();
            case KeyDown key when key.Key == "u":
                level = level.Undo();
                moves += 1;
                return new Render();
            case Tick:
                return level
                    .EndOfLevel()
                    .MapOr(
                        (Action)new Nothing(),
                        (reason) => new SwitchState(new GameOver(reason, moves))
                    );
            default:
                break;
        }

        return new Nothing();
    }
}

/// <summary>
/// End-of-level state shown after a win or loss.
/// </summary>
/// <param name="Reason">The outcome that ended the level.</param>
public class GameOver(GameOverReason Reason, int moves) : GameState(moves)
{
    /// <inheritdoc />
    public override WorldImage Render(int windowSize)
    {
        var background = new RectangleImage(windowSize, windowSize, OutlineMode.Fill, Colors.White);

        var text = new TextImage(Reason.Message, 18, Colors.Black);
        var movesText = new TextImage($"Moves Taken: {moves}", 18, Colors.Black);
        var textWithMoves = new AboveImage(text, movesText);
        var centered = new OverlayImage(textWithMoves, background);

        var bottomControls = new TextImage(
            "Quit <q>  "
                + Reason switch
                {
                    Win => "Next <Tab>",
                    Lose => "Retry <Tab>",
                    _ => "",
                },
            18,
            Colors.Black
        );

        return new OverlayOffsetAlignImage(
            AlignModeX.Center,
            AlignModeY.Bottom,
            bottomControls,
            0,
            bottomControls.Height,
            Util.ScaleImage(centered, windowSize)
        );
    }

    /// <inheritdoc />
    public override Action HandleEvent(Event e)
    {
        switch (e)
        {
            case KeyDown key when key.Key == "q":
                return new Quit();
            case KeyDown key when key.Key == "Tab":
            {
                return Reason switch
                {
                    Win => Level.HasNextLevel(Reason.LevelNumber)
                        ? new SwitchState(new Playing(Reason.LevelNumber + 1))
                        : new Quit(),
                    _ => new SwitchState(new Playing(Reason.LevelNumber)),
                };
            }
            default:
                break;
        }

        return new Nothing();
    }
}

