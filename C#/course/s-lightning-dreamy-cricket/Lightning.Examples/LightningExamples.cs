using System.Windows.Media;
using ImageLib;
using ImageLib.Enumerations;
using ImageLib.FunWorld;
using TesterLib;

namespace Lightning;

class LightningExamples
{
    // Your examples go here!

    #region Some tests

    bool TestOverlayImage(Tester t)
    {
        var rectangle1 = new RectangleImage(100, 30, OutlineMode.Fill, Colors.Red).MovePinholeTo(AlignMode.CenterRight);
        var rectangle2 = new RectangleImage(30, 100, OutlineMode.Fill, Colors.Blue);
        return t.CheckExpect(ShowImage(new OverlayImage(rectangle1, rectangle2)), true);
    }

    bool TestRotateImagePinhole(Tester t)
    {
        var rectangle1 = new RectangleImage(100, 30, OutlineMode.Fill, Colors.Red).MovePinholeTo(AlignMode.CenterLeft);
        var rotatedRectangle1 = new RotateImage(rectangle1, 45);
        return t.CheckExpect(ShowImage(rotatedRectangle1), true);
    }

    bool TestOverlayOffsetAlignImage(Tester t)
    {
        var rectangle1 = new VisiblePinholeImage(new RectangleImage(100, 30, OutlineMode.Fill, Colors.Red).MovePinholeTo(AlignMode.CenterLeft));
        var otherImage = new VisiblePinholeImage(new RotateImage(new RectangleImage(100, 30, OutlineMode.Fill, Colors.Yellow).MovePinholeTo(AlignMode.CenterRight), 67));
        return t.CheckExpect(ShowImage(new OverlayImage(otherImage, rectangle1)), true);
    }

    #endregion

    #region Tip tests

    bool TestDrawTip(Tester t)
    {
        ILightningBolt tip = new Tip();
        return t.CheckExpect(ShowImage(tip.Draw()), true);
    }

    #endregion

    #region Helpers

    bool ShowImage(WorldImage image)
    {
        return new WorldCanvas().DrawScene(WorldScene.FromImage(new VisiblePinholeImage(image))).Show();
    }

    bool ShowImage(WorldImage image, string title)
    {
        return new WorldCanvas(title).DrawScene(WorldScene.FromImage(new VisiblePinholeImage(image))).Show();
    }

    #endregion

    #region Segment tests


    bool TestSegmentDraw1(Tester t)
    {
        var segment = new Segment(50, 10, 90, new Tip());
        return t.CheckExpect(ShowImage(segment.Draw()), true);
    }

    bool TestSegmentDraw2(Tester t)
    {
        var segment = new Segment(50, 10, 23, new Tip());
        return t.CheckExpect(ShowImage(segment.Draw()), true);
    }

    bool TestSegmentDraw3(Tester t)
    {
        var segment = new Segment(50, 10, 23, new Segment(50, 10, 90, new Tip()));
        return t.CheckExpect(ShowImage(segment.Draw()), true);
    }

    bool TestSegmentDraw4(Tester t)
    {
        var segment = new Segment(50, 5, 80, new Segment(50, 12, 151, new Tip()));
        return t.CheckExpect(ShowImage(segment.Draw()), true);
    }

    #endregion

    #region Fork tests

    bool TestForkDraw1(Tester t)
    {
        var fork = new Fork(50, 100, 10, 20, 20, 120, new Tip(), new Tip());
        return t.CheckExpect(ShowImage(fork.Draw(), "Fork1"), true);
    }

    bool TestForkDraw2(Tester t)
    {
        var fork = new Fork(50, 100, 10, 20, 10, 80, new Segment(50, 10, 90, new Tip()), new Segment(50, 10, 90, new Tip()));
        return t.CheckExpect(ShowImage(fork.Draw(), "Fork2"), true);
    }

    bool TestForkDraw3(Tester t)
    {
        var fork = new Fork(50, 100, 5, 20, 10, 80, new Fork(80, 230, 10, 20, 20, 120, new Tip(), new Tip()), new Segment(50, 10, 90, new Tip()));

        return t.CheckExpect(ShowImage(fork.Draw(), "Fork3"), true);
    }

    #endregion

    #region IsPhysicallyPossible tests

    bool TestIsPhysicallySegment(Tester t)
    {
        var segment = new Segment(50, 10, 90, new Tip());
        return t.CheckExpect(segment.IsPhysicallyPossible(), true);
    }

    bool TestIsPhysicallyTwoSegmentPass(Tester t)
    {
        var segment = new Segment(50, 10, 90, new Segment(50, 9, 90, new Tip()));
        return t.CheckExpect(segment.IsPhysicallyPossible(), true);
    }

    bool TestIsPhysicallyTwoSegmentFail(Tester t)
    {
        var segment = new Segment(50, 10, 90, new Segment(50, 11, 90, new Tip()));
        return t.CheckExpect(segment.IsPhysicallyPossible(), false);
    }

    bool TestIsPhysicallyFork(Tester t)
    {
        var fork = new Fork(50, 50, 10, 15, 0, 0, new Tip(), new Tip());
        return t.CheckExpect(fork.IsPhysicallyPossible(), true);
    }

    bool TestIsPhysicallyForkAndOneSegmentPass(Tester t)
    {
        var fork = new Fork(50, 50, 10, 15, 0, 0, new Segment(50, 9, 90, new Tip()), new Tip());
        return t.CheckExpect(fork.IsPhysicallyPossible(), true);
    }

    bool TestIsPhysicallyForkAndLeftSegmentFail(Tester t)
    {
        var fork = new Fork(50, 50, 10, 15, 0, 0, new Segment(50, 11, 90, new Tip()), new Tip());
        return t.CheckExpect(fork.IsPhysicallyPossible(), false);
    }

    bool TestIsPhysicallyForkAndRightSegmentFail(Tester t)
    {
        var fork = new Fork(50, 50, 10, 15, 0, 0, new Tip(), new Segment(50, 20, 90, new Tip()));
        return t.CheckExpect(fork.IsPhysicallyPossible(), false);
    }

    bool TestIsPhysicallyForkAndTwoSegments(Tester t)
    {
        var fork = new Fork(50, 50, 10, 15, 0, 0, new Segment(50, 9, 90, new Tip()), new Segment(50, 14, 90, new Tip()));
        return t.CheckExpect(fork.IsPhysicallyPossible(), true);
    }

    bool TestIsPhysicallyForkAndTwoSegmentsLeftFail(Tester t)
    {
        var fork = new Fork(50, 50, 10, 15, 0, 0, new Segment(50, 11, 90, new Tip()), new Segment(50, 14, 90, new Tip()));
        return t.CheckExpect(fork.IsPhysicallyPossible(), false);
    }

    bool TestIsPhysicallyForkAndTwoSegmentsRightFail(Tester t)
    {
        var fork = new Fork(50, 50, 10, 15, 0, 0, new Segment(50, 9, 90, new Tip()), new Segment(50, 20, 90, new Tip()));
        return t.CheckExpect(fork.IsPhysicallyPossible(), false);
    }

    bool TestIsPhysicallyForkInForkPass(Tester t)
    {
        var fork = new Fork(50, 50, 10, 15, 0, 0, new Segment(50, 9, 90, new Tip()), new Fork(50, 50, 9, 14, 0, 0, new Segment(50, 8, 90, new Tip()), new Segment(50, 13, 90, new Tip())));
        return t.CheckExpect(fork.IsPhysicallyPossible(), true);
    }

    bool TestIsPhysicallyForkInForkFail(Tester t)
    {
        var fork = new Fork(50, 50, 10, 15, 0, 0, new Segment(50, 9, 90, new Tip()), new Fork(50, 50, 9, 12, 0, 0, new Segment(50, 8, 90, new Tip()), new Segment(50, 13, 90, new Tip())));
        return t.CheckExpect(fork.IsPhysicallyPossible(), false);
    }


    #endregion

    #region Combine Test

    bool TestCombineExample1(Tester t)
    {
        var BOLT1 = new Fork(30, 30, 10, 10, 135, 40, new Tip(), new Tip());
        var BOLT2 = new Fork(30, 30, 10, 10, 115, 65, new Tip(), new Tip());

        return t.CheckExpect(ShowImage(BOLT1.Combine(40, 50, 10, 10, 150, 30, BOLT2).Draw()), true);

    }

    #endregion

    #region GetWidth

    bool TestGetWidthTip(Tester t)
    {
        var bolt = new Segment(50, 10, 90, new Tip());
        return t.CheckInexact(bolt.GetWidth(), 0.0, 0.001);
    }

    bool TestGetWidthSegment(Tester t)
    {
        var bolt = new Segment(2, 10, 60, new Tip());
        return t.CheckInexact(bolt.GetWidth(), 1.0, 0.001);
    }

    bool TestGetWidthFork(Tester t)
    {
        var bolt = new Fork(2, 2, 0, 0, 60, 120, new Tip(), new Tip());
        return t.CheckInexact(bolt.GetWidth(), 2.0, 0.001);
    }

    bool TestGetWidthLargeFork(Tester t)
    {
        var bolt = new Fork(2, 2, 0, 0, 60, 120,
            new Segment(4, 0, 60, new Segment(10, 0, 120, new Tip())),
            new Segment(4, 0, 60, new Segment(6, 0, 60, new Tip()))
        );
        return t.CheckInexact(bolt.GetWidth(), 6.0, 0.001);
    }

    #endregion
}

