using System.Windows.Media;
using ImageLib;
using ImageLib.Enumerations;
using ImageLib.ImpWorld;
using ImageLib.Interfaces;

namespace Sokoban;

/// <summary>
/// The entry point of the game. Makes a game with the given level.
/// </summary>
/// <param name="level">The level state to start from.</param>
public class Game : World
{
    /// <summary>The current state of the game.</summary>
    private GameState state = GameState.New();
    private bool exit = false;

    /// <summary>
    /// The size of the window.
    /// </summary>
    private const int WINDOW_SIZE = 700;

    /// <summary>
    /// The image of the game.
    /// </summary>
    IWorldImage image;

    public Game()
        : base()
    {
        image = ScaleFinalImage(state.Render(WINDOW_SIZE));
    }

    /// <inheritdoc />
    protected override IWorldScene MakeScene()
    {
        return WorldScene.FromImage(image);
    }

    /// <inheritdoc />
    protected override void OnKeyDown(string key) =>
        HandleAction(state.HandleEvent(new KeyDown(key)));

    /// <inheritdoc />
    protected override bool ShouldWorldEnd() => exit;

    /// <inheritdoc />
    protected override IWorldScene LastScene(string message)
    {
        var text = new TextImage("Game has ended, please close the window.", 18, Colors.Black);
        var centered = new OverlayImage(
            text,
            new RectangleImage(WINDOW_SIZE, WINDOW_SIZE, OutlineMode.Fill, Colors.White)
        );
        return WorldScene.FromImage(ScaleFinalImage(centered));
    }

    /// <inheritdoc />
    protected override void OnTick() => HandleAction(state.HandleEvent(new Tick()));

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
    private ScaleImage ScaleFinalImage(WorldImage image) => Util.ScaleImage(image, WINDOW_SIZE);

    /// <summary>
    /// Applies an action returned by the current game state.
    /// </summary>
    /// <param name="action">The action to apply.</param>
    private void HandleAction(Action action)
    {
        switch (action)
        {
            case Quit:
                exit = true;
                break;
            case SwitchState(GameState state):
                this.state = state;
                UpdateImage();
                break;
            case Render:
                UpdateImage();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Updates the image of the game. Runs when the Render action is returned.
    /// </summary>
    private void UpdateImage() => image = ScaleFinalImage(state.Render(WINDOW_SIZE));
}

