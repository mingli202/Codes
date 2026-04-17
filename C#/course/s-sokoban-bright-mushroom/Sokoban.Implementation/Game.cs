using ImageLib;
using ImageLib.FunWorld;
using ImageLib.Interfaces;
using Sokoban.cells;

namespace Sokoban;

/// <summary>
/// The entry point of the game. Makes a game with the given level.
/// </summary>
/// <param name="level">The level state to start from.</param>
public class Game(Level level) : World()
{
    /// <summary>
    /// The current level of the game.
    /// </summary>
    private readonly Level level = level;

    /// <summary>
    /// The size of the window.
    /// </summary>
    private readonly int WINDOW_SIZE = 500;

    /// <summary>
    /// Creates a new game with initial level.
    /// </summary>
    public Game()
        : this(
            new Level(
                string.Join(
                    "\n",
                    [
                        "________",
                        "___R____",
                        "________",
                        "_B____Y_",
                        "________",
                        "___G____",
                        "________",
                    ]
                ),
                string.Join(
                    "\n",
                    [
                        "__WWW___",
                        "__W_WW__",
                        "WWWr_WWW",
                        "W_b>yB_W",
                        "WW_gWWWW",
                        "_WW_W___",
                        "__WWW___",
                    ]
                )
            )
        ) { }

    /// <inheritdoc />
    protected override IWorldScene MakeScene()
    {
        var levelImage = level.Render();
        return WorldScene.FromImage(ScaleFinalImage(levelImage));
    }

    /// <inheritdoc />
    protected override World OnKeyUp(string key)
    {
        Level levelNextFrame = key switch
        {
            "Up" => level.MovePlayer(PlayerDirection.Up),
            "Down" => level.MovePlayer(PlayerDirection.Down),
            "Left" => level.MovePlayer(PlayerDirection.Left),
            "Right" => level.MovePlayer(PlayerDirection.Right),
            _ => throw new ArgumentException("key not supported"),
        };

        return new Game(levelNextFrame);
    }

    /// <summary>
    /// Starts the game.
    /// </summary>
    /// <returns>True if the game starts successfully, false otherwise.</returns>
    public bool Start() => BigBang(WINDOW_SIZE, WINDOW_SIZE, 1 / 60f);

    /// <summary>
    /// Scales the image to fit the window.
    /// </summary>
    /// <param name="image">The image to scale.</param>
    /// <returns>The scaled image.</returns>
    private ScaleImage ScaleFinalImage(WorldImage image) =>
        new(image, WINDOW_SIZE / Math.Max(image.Width, image.Height));
}

