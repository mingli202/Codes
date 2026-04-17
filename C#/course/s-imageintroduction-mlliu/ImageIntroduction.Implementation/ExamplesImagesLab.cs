namespace ImageIntroduction;

using ImageLib;              // images, like RectangleImage or OverlayImages
using ImageLib.FunWorld;     // the WorldScene class
using ImageLib.Enumerations; // values like OutlineMode.Fill
using System.Windows.Media;                    // general colors (as triples of red,green,blue values)
                                               // and predefined colors (Colors.Red, Colors.Gray, etc.)
using TesterLib;
using System.Text.Json.Serialization;


public class ExamplesImagesLab
{
  // Define constants and images
  const int imageWidth = 100;
  const int imageHeight = 100;
  const int squareSize = 20;
  private readonly WorldImage cheesyImage;
  private readonly WorldImage builtImage;
  ExamplesImagesLab()
  {
    cheesyImage = CheesyLoadImage();
    builtImage = BuildImage();

    cheesyImage.SaveImage("public/cheesyImage.png");
    builtImage.SaveImage("public/builtImage.png");
  }

  private static FromFileImage CheesyLoadImage()
  {
    var image = new FromFileImage("public/car.png");
    return image;
  }

  private static OverlayOffsetImage BuildImage()
  {
    var bgImage = new RectangleImage(imageWidth, imageHeight, OutlineMode.Fill, Color.FromRgb(0, 0, 255));
    var redRect1 = new RectangleImage(3 * squareSize, squareSize, OutlineMode.Fill, Color.FromRgb(255, 0, 0));
    var redRect2 = new RectangleImage(imageWidth, squareSize, OutlineMode.Fill, Color.FromRgb(255, 0, 0));
    var blackCircle = new CircleImage(10, OutlineMode.Fill, Color.FromRgb(0, 0, 0));

    var image = new OverlayOffsetImage(redRect1, 0, 0, bgImage);
    image = new OverlayOffsetImage(redRect2, 0, -squareSize, image);
    image = new OverlayOffsetImage(blackCircle, -squareSize, -2 * squareSize, image);
    image = new OverlayOffsetImage(blackCircle, squareSize, -2 * squareSize, image);


    return image;
  }



  bool TestShowCar(Tester t)
  {
    WorldImage img = BuildImage();
    WorldScene scene = WorldScene.FromImage(img);
    bool result = new WorldCanvas().DrawScene(scene).Show();

    return t.CheckExpect(result, true);
  }
}

