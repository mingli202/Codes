namespace BigBangIntroduction;

using ImageLib;              // images, like RectangleImage or OverlayImages
using ImageLib.FunWorld;     // the WorldScene class
using ImageLib.Enumerations; // values like OutlineMode.Fill
using System.Windows.Media;                    // general colors (as triples of red,green,blue values)
                                               // and predefined colors (Colors.Red, Colors.Gray, etc.)
using TesterLib;

public class ExamplesBigBangLab
{
    const int BOARD_SIZE = 100;

    record Point(int X, int Y);
    record Board(Point A, Point B);
    enum UserInputState { Pressed, Released }

    abstract record GameState();
    record PlayingState(Board Board, Point UserChosenPoint, UserInputState UserInputState) : GameState();
    record GameOverState(double UserError) : GameState();

    WorldImage PointAImage;
    WorldImage PointBImage;
    WorldImage MidpointImage;
    WorldImage UserChosenPointImage;

    public ExamplesBigBangLab()
    {

    }

    WorldScene PlacePoint(Point point, WorldImage image, WorldScene scene)
    {
        return scene.PlaceImageXY(image, point.X, point.Y);
    }

    WorldScene DrawBoard(Board board)
    {
        WorldScene scene = new(BOARD_SIZE, BOARD_SIZE);
        scene = PlacePoint(board.A, PointAImage, scene);
        scene = PlacePoint(board.B, PointBImage, scene);
        scene = PlacePoint(BoardMidpoint(board), MidpointImage, scene);

        return scene;
    }

    Point BoardMidpoint(Board board)
        => Midpoint(board.A, board.B);

    private Point Midpoint(Point a, Point b)
        => new Point((a.X + b.X) / 2, (a.Y + b.Y) / 2);

    double Distance(Point a, Point b)
        => Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));

    double UserError(Point userGivenPoint, Point boardMidpoint)
        => Distance(userGivenPoint, boardMidpoint);
}

