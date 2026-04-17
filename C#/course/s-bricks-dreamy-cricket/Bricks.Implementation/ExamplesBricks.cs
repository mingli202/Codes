namespace Bricks;

using System.Windows.Media;                    // general colors (as triples of red,green,blue values)
using ImageLib;              // images, like RectangleImage or OverlayImages
using ImageLib.Enumerations; // values like OutlineMode.Fill
using ImageLib.FunWorld;     // the WorldScene class
using TesterLib;             // The tester library
                                               // and predefined colors (Colors.Red, Colors.Gray, etc.)


class ExamplesBricks
{
    const int BRICK_SIZE = 72;
    const int MORTAR_THICKNESS = 10;

    /// <summary>
    /// Draws the herringbone brick pattern without WorldScene with the given brick size that includes the mortar thickness, mortar thickness, brick color, and mortar color.
    /// </summary>
    ///
    /// <example>
    ///     <code>
    ///         var brickSize = 10;
    ///         var mortarThickness = 2;
    ///         var brickColor = Colors.Red;
    ///         var mortarColor = Colors.Gray;
    ///         var image = Herringbone_1(brickSize, mortarThickness, brickColor, mortarColor);
    ///         image.SaveImage("Herringbone_1.png");
    ///     </code>
    /// </example>
    ///
    /// <param name="brickSize">The size of the brick including the mortar.</param>
    /// <param name="mortarThickness">The thickness of the mortar.</param>
    /// <param name="brickColor">The color of the brick.</param>
    /// <param name="mortarColor">The color of the mortar.</param>
    ///
    /// <returns>The image of the herringbone brick pattern.</returns>
    ///
    /// <seealso cref="Herringbone_2(int, int, Color, Color)"/>
    /// <seealso cref="Staggered_1(int, int, Color, Color)"/>
    WorldImage Herringbone_1(int brickSize, int mortarThickness, Color brickColor, Color mortarColor)
        => BuildRows_1(0, 8, 8, brickSize, CreateHalfBrick(brickSize, mortarThickness, brickColor, mortarColor), [0, 270, 90, 180], 0, new RectangleImage(brickSize * 8, brickSize * 8, OutlineMode.Fill, Colors.White)).image;

    /// <summary>
    /// Draws the herringbone brick pattern with WorldScene with the given brick size that includes the mortar thickness, mortar thickness, brick color, and mortar color.
    /// </summary>
    ///
    /// <example>
    ///     <code>
    ///         var brickSize = 10;
    ///         var mortarThickness = 2;
    ///         var brickColor = Colors.Red;
    ///         var mortarColor = Colors.Gray;
    ///         var scene = Herringbone_2(brickSize, mortarThickness, brickColor, mortarColor);
    ///         scene.SaveImage("Herringbone_2.png");
    ///     </code>
    /// </example>
    ///
    /// <param name="brickSize">The size of the brick including the mortar.</param>
    /// <param name="mortarThickness">The thickness of the mortar.</param>
    /// <param name="brickColor">The color of the brick.</param>
    /// <param name="mortarColor">The color of the mortar.</param>
    ///
    /// <returns>The image of the herringbone brick pattern.</returns>
    ///
    /// <seealso cref="Herringbone_1(int, int, Color, Color)"/>
    /// <seealso cref="Staggered_2(int, int, Color, Color)"/>
    WorldScene Herringbone_2(int brickSize, int mortarThickness, Color brickColor, Color mortarColor)
        => BuildRows_2(0, 8, 8, brickSize, CreateHalfBrick(brickSize, mortarThickness, brickColor, mortarColor), [0, 270, 90, 180], 0, new WorldScene(brickSize * 8, brickSize * 8)).scene;

    /// <summary>
    /// Draws the staggered brick pattern without WorldScene with the given brick size that includes the mortar thickness, mortar thickness, brick color, and mortar color.
    /// </summary>
    ///
    /// <example>
    ///     <code>
    ///         var brickSize = 10;
    ///         var mortarThickness = 2;
    ///         var brickColor = Colors.Blue;
    ///         var mortarColor = Colors.Gray;
    ///         var image = Staggered_1(brickSize, mortarThickness, brickColor, mortarColor);
    ///         image.SaveImage("Staggered_1.png");
    ///     </code>
    /// </example>
    ///
    /// <param name="brickSize">The size of the brick including the mortar.</param>
    /// <param name="mortarThickness">The thickness of the mortar.</param>
    /// <param name="brickColor">The color of the brick.</param>
    /// <param name="mortarColor">The color of the mortar.</param>
    ///
    /// <returns>The image of the herringbone brick pattern.</returns>
    ///
    /// <seealso cref="Herringbone_1(int, int, Color, Color)"/>
    /// <seealso cref="Staggered_2(int, int, Color, Color)"/>
    WorldImage Staggered_1(int brickSize, int mortarThickness, Color brickColor, Color mortarColor)
        => BuildRows_1(0, 8, 10, brickSize, CreateHalfBrick(brickSize, mortarThickness, brickColor, mortarColor), [90, 270], 0, new RectangleImage(brickSize * 10, brickSize * 8, OutlineMode.Fill, Colors.White)).image;

    /// <summary>
    /// Draws the staggered brick pattern with WorldScene with the given brick size that includes the mortar thickness, mortar thickness, brick color, and mortar color.
    /// </summary>
    ///
    /// <example>
    ///     <code>
    ///         var brickSize = 10;
    ///         var mortarThickness = 2;
    ///         var brickColor = Colors.Blue;
    ///         var mortarColor = Colors.Gray;
    ///         var scene = Staggered_2(brickSize, mortarThickness, brickColor, mortarColor);
    ///         scene.SaveImage("Staggered_2.png");
    ///     </code>
    /// </example>
    ///
    /// <param name="brickSize">The size of the brick including the mortar.</param>
    /// <param name="mortarThickness">The thickness of the mortar.</param>
    /// <param name="brickColor">The color of the brick.</param>
    /// <param name="mortarColor">The color of the mortar.</param>
    ///
    /// <returns>The image of the herringbone brick pattern.</returns>
    ///
    /// <seealso cref="Herringbone_2(int, int, Color, Color)"/>
    /// <seealso cref="Staggered_1(int, int, Color, Color)"/>
    WorldScene Staggered_2(int brickSize, int mortarThickness, Color brickColor, Color mortarColor)
        => BuildRows_2(0, 8, 10, brickSize, CreateHalfBrick(brickSize, mortarThickness, brickColor, mortarColor), [90, 270], 0, new WorldScene(brickSize * 10, brickSize * 8)).scene;

    /// <summary>
    /// Adds a row of bricks with WorldImage with the given brick size that includes the mortar thickness, mortar thickness, brick color, and mortar color to the given image at the given row.
    /// </summary>
    ///
    /// <remarks>
    /// Did not convert to arrow function so store rowResult in a variable.
    /// </remarks>
    ///
    /// <param name="row">The row of bricks.</param>
    /// <param name="nRows">The number of rows.</param>
    /// <param name="nCols">The number of columns.</param>
    /// <param name="brickSize">The size of the brick including the mortar.</param>
    /// <param name="halfBrick">The half brick.</param>
    /// <param name="directions">The directions.</param>
    /// <param name="directionIndex">The direction index.</param>
    /// <param name="image">The base image.</param>
    /// <returns>The result image with the row added.</returns>
    (WorldImage image, int directionIndex) BuildRows_1(int row, int nRows, int nCols, int brickSize, WorldImage halfBrick, int[] directions, int directionIndex, WorldImage image)
    {
        if (row >= nRows)
            return (image, directionIndex);

        var rowResult = BuildColumns_1(row, col: 0, nCols, brickSize, halfBrick, directions, directionIndex, image);

        return BuildRows_1(row + 1, nRows, nCols, brickSize, halfBrick, directions, rowResult.directionIndex, rowResult.image);
    }

    /// <summary>
    /// Adds a column of bricks with WorldImage with the given brick size that includes the mortar thickness, mortar thickness, brick color, and mortar color to the given image at the given column.
    /// </summary>
    ///
    /// <param name="row">The row of bricks.</param>
    /// <param name="col">The column of bricks.</param>
    ///System.Console.WriteLine("bricks: " + bricks);
    /// <param name="nCols">The number of columns.</param>
    /// <param name="brickSize">The size of the brick including the mortar.</param>
    /// <param name="halfBrick">The half brick.</param>
    /// <param name="directions">The directions.</param>
    /// <param name="directionIndex">The direction index.</param>
    /// <param name="image">The base image.</param>
    /// <returns>The result image with the column added.</returns>
    (WorldImage image, int directionIndex) BuildColumns_1(int row, int col, int nCols, int brickSize, WorldImage halfBrick, int[] directions, int directionIndex, WorldImage image)
        => col >= nCols ?
            (image, directionIndex) :
            BuildColumns_1(row, col + 1, nCols, brickSize, halfBrick, directions, (col != nCols - 1) ? (directionIndex + 1) % directions.Length : directionIndex, new OverlayOffsetAlignImage(AlignModeX.Left, AlignModeY.Top, new RotateImage(halfBrick, directions[directionIndex]), -col * brickSize, -row * brickSize, image));

    /// <summary>
    /// Adds a row of bricks with WorldScene with the given brick size that includes the mortar thickness, mortar thickness, brick color, and mortar color to the given image at the given row.
    /// </summary>
    ///
    /// <remarks>
    /// Did not convert to arrow function so store rowResult in a variable.
    /// </remarks>
    ///
    /// <param name="row">The row of bricks.</param>
    /// <param name="nRows">The number of rows.</param>
    /// <param name="nCols">The number of columns.</param>
    /// <param name="brickSize">The size of the brick including the mortar.</param>
    /// <param name="halfBrick">The half brick.</param>
    /// <param name="directions">The directions.</param>
    /// <param name="directionIndex">The direction index.</param>
    /// <param name="scene">The base scene.</param>
    /// <returns>The result scene with the row added.</returns>
    (WorldScene scene, int directionIndex) BuildRows_2(int row, int nRows, int nCols, int brickSize, WorldImage halfBrick, int[] directions, int directionIndex, WorldScene scene)
    {
        if (row >= nRows)
            return (scene, directionIndex);

        var rowResult = BuildColumns_2(row, col: 0, nCols, brickSize, halfBrick, directions, directionIndex, scene);

        return BuildRows_2(row + 1, nRows, nCols, brickSize, halfBrick, directions, rowResult.directionIndex, rowResult.scene);
    }

    /// <summary>
    /// Adds a column of bricks with WorldScene with the given brick size that includes the mortar thickness, mortar thickness, brick color, and mortar color to the given scene at the given column.
    /// </summary>
    ///
    /// <param name="row">The row of bricks.</param>
    /// <param name="col">The column of bricks.</param>
    /// <param name="nCols">The number of columns.</param>
    /// <param name="brickSize">The size of the brick including the mortar.</param>
    /// <param name="halfBrick">The half brick.</param>
    /// <param name="directions">The directions.</param>
    /// <param name="directionIndex">The direction index.</param>
    /// <param name="scene">The base scene.</param>
    /// <returns>The result scene with the column added.</returns>
    (WorldScene scene, int directionIndex) BuildColumns_2(int row, int col, int nCols, int brickSize, WorldImage halfBrick, int[] directions, int directionIndex, WorldScene scene)
        => (col >= nCols) ?
             (scene, directionIndex) :
             BuildColumns_2(row, col + 1, nCols, brickSize, halfBrick, directions, (col != nCols - 1) ? (directionIndex + 1) % directions.Length : directionIndex, scene.PlaceImageXY(new RotateImage(halfBrick, directions[directionIndex]), AlignMode.TopLeft, col * brickSize, row * brickSize));


    /// <summary>
    /// Creates a half brick with the given brick size, mortar thickness, brick color, and mortar color.
    /// </summary>
    ///
    /// <param name="brickSize">The size of the brick including the mortar.</param>
    /// <param name="mortarThickness">The thickness of the mortar.</param>
    /// <param name="brickColor">The color of the brick.</param>
    /// <param name="mortarColor">The color of the mortar.</param>
    /// <returns>The half brick.</returns>
    private OverlayOffsetAlignImage CreateHalfBrick(int brickSize, int mortarThickness, Color brickColor, Color mortarColor)
        => new OverlayOffsetAlignImage(AlignModeX.Left, AlignModeY.Top, new RectangleImage(brickSize - mortarThickness, brickSize - mortarThickness / 2, OutlineMode.Fill, brickColor), -mortarThickness / 2, -mortarThickness / 2, new RectangleImage(brickSize, brickSize, OutlineMode.Fill, mortarColor));

    bool TestCreateHalfBrick(Tester t)
    {
        var brickSize = 10;
        var mortarThickness = 2;
        var brickColor = Colors.Red;
        var mortarColor = Colors.Gray;
        var halfBrick = CreateHalfBrick(brickSize, mortarThickness, brickColor, mortarColor);
        halfBrick.SaveImage("CreateHalfBrick.png");
        return t.CheckExpect(ShowImage(halfBrick), true);
    }

    bool TestBuildRows_1(Tester t)
    {
        var brickSize = 10;
        var mortarThickness = 2;
        var brickColor = Colors.Red;
        var mortarColor = Colors.Gray;
        var image = BuildRows_1(0, 8, 10, brickSize, CreateHalfBrick(brickSize, mortarThickness, brickColor, mortarColor), [90, 270], 0, new RectangleImage(brickSize * 10, brickSize * 8, OutlineMode.Fill, Colors.White)).image;
        image.SaveImage("BuildRows_1.png");
        return t.CheckExpect(ShowImage(image), true);
    }

    bool TestBuildColumns_1(Tester t)
    {
        var brickSize = 10;
        var mortarThickness = 2;
        var brickColor = Colors.Red;
        var mortarColor = Colors.Gray;
        var image = BuildColumns_1(0, 0, 10, brickSize, CreateHalfBrick(brickSize, mortarThickness, brickColor, mortarColor), [90, 270], 0, new RectangleImage(brickSize * 10, brickSize * 8, OutlineMode.Fill, Colors.White)).image;
        image.SaveImage("BuildColumns_1.png");
        return t.CheckExpect(ShowImage(image), true);
    }

    bool TestBuildRows_2(Tester t)
    {
        var brickSize = 10;
        var mortarThickness = 2;
        var brickColor = Colors.Red;
        var mortarColor = Colors.Gray;
        var scene = BuildRows_2(0, 8, 10, brickSize, CreateHalfBrick(brickSize, mortarThickness, brickColor, mortarColor), [90, 270], 0, new WorldScene(brickSize * 10, brickSize * 8)).scene;
        scene.SaveImage("BuildRows_2.png");
        return t.CheckExpect(ShowScene(scene), true);
    }

    bool TestBuildColumns_2(Tester t)
    {
        var brickSize = 10;
        var mortarThickness = 2;
        var brickColor = Colors.Red;
        var mortarColor = Colors.Gray;
        var scene = BuildColumns_2(0, 0, 10, brickSize, CreateHalfBrick(brickSize, mortarThickness, brickColor, mortarColor), [90, 270], 0, new WorldScene(brickSize * 10, brickSize * 8)).scene;
        scene.SaveImage("BuildColumns_2.png");
        return t.CheckExpect(ShowScene(scene), true);
    }

    bool TestHerringBone_1(Tester t)
    {
        var brickColor = Colors.Red;
        var mortarColor = Colors.Black;
        var image = Herringbone_1(BRICK_SIZE, MORTAR_THICKNESS, brickColor, mortarColor);
        image.SaveImage("Herringbone_1.png");
        return t.CheckExpect(ShowImage(image), true);
    }

    bool TestHerringBone_2(Tester t)
    {
        var brickColor = Colors.Red;
        var mortarColor = Colors.Black;
        var scene = Herringbone_2(BRICK_SIZE, MORTAR_THICKNESS, brickColor, mortarColor);
        scene.SaveImage("Herringbone_2.png");
        return t.CheckExpect(ShowScene(scene), true);
    }

    bool TestStaggered_1(Tester t)
    {
        var brickColor = Colors.Blue;
        var mortarColor = Colors.Gray;
        var image = Staggered_1(BRICK_SIZE, MORTAR_THICKNESS, brickColor, mortarColor);
        image.SaveImage("Staggered_1.png");
        return t.CheckExpect(ShowImage(image), true);
    }

    bool TestStaggered_2(Tester t)
    {
        var brickColor = Colors.Blue;
        var mortarColor = Colors.Gray;
        var scene = Staggered_2(BRICK_SIZE, MORTAR_THICKNESS, brickColor, mortarColor);
        scene.SaveImage("Staggered_2.png");
        return t.CheckExpect(ShowScene(scene), true);
    }

    /// <summary>
    /// Helper function to show an Image
    /// </summary>
    /// <param name="image"></param>
    /// <returns>whether the image is shown</returns>
    private bool ShowImage(WorldImage image)
    {
        WorldScene scene = WorldScene.FromImage(image);
        return new WorldCanvas().DrawScene(scene).Show();
    }

    /// <summary>
    /// Helper function to show a Scene
    /// </summary>
    /// <param name="scene"></param>
    /// <returns>whether the scene is shown</returns>
    private bool ShowScene(WorldScene scene)
    {
        return new WorldCanvas().DrawScene(scene).Show();
    }
}

