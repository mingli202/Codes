using ImageLib;
using ImageLib.FunWorld;
using TesterLib;

namespace Sokoban;

#pragma warning disable CA1822, IDE0051
/// <summary>
/// Example-based tests for the Sokoban game.
/// </summary>
class ExamplesSokoban
{
    // bool TestLevelScene(Tester t)
    // {
    //     string ground = string.Join("\n", [
    //         "________",
    //         "___R____",
    //         "________",
    //         "_B____Y_",
    //         "________",
    //         "___G____",
    //         "________"
    //     ]);
    //     string description = string.Join("\n", [
    //         "__WWW___",
    //         "__W_WW__",
    //         "WWWr_WWW",
    //         "W_b>yB_W",
    //         "WW_gWWWW",
    //         "_WW_W___",
    //         "__WWW___"
    //     ]);
    //
    //     var level = new Level(ground, description);
    //
    //     var image = level.Render();
    //     var scene = WorldScene.FromImage(new ScaleImage(image, 0.5));
    //
    //     return t.CheckExpect(new WorldCanvas().DrawScene(scene).Show(), true);
    // }
    /// <summary>
    /// Tests that the game starts successfully.
    /// </summary>
    bool TestGame(Tester t)
    {
        var game = new Game();
        return t.CheckExpect(game.Start(), true);
    }
}

