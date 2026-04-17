using System.Windows;
using ImageLib;
using ImageLib.FunWorld;
using TesterLib;

namespace Lightning;

class LightningImplementationExamples
{
    bool ShowImage(WorldImage image)
    {
        return new WorldCanvas().DrawScene(WorldScene.FromImage(new VisiblePinholeImage(image))).Show();
    }

    bool ShowImage(WorldImage image, string title)
    {
        return new WorldCanvas(title).DrawScene(WorldScene.FromImage(new VisiblePinholeImage(image))).Show();
    }

    bool TestGetHeightProportionalToCurrent(Tester t)
        => t.CheckExpect(new Segment(50, 10, 90, new Tip()).GetHeightProportionalToCurrent(), 20);

    bool TestRectangleSegment(Tester t)
    {
        var segment = new Segment(50, 10, 90, new Tip());
        return t.CheckExpect(ShowImage(segment.RectangleSegment()), true);
    }

    bool TestRotatedSegment(Tester t)
    {
        var segment = new Segment(50, 10, 90, new Tip());
        return t.CheckExpect(ShowImage(segment.RotatedSegment()), true);
    }

    bool TestRotatedSegment2(Tester t)
    {
        var segment = new Segment(50, 10, 67, new Tip());
        return t.CheckExpect(ShowImage(segment.RotatedSegment()), true);
    }

    bool TestRotatedSegment3(Tester t)
    {
        var segment = new Segment(50, 10, 151, new Tip());
        return t.CheckExpect(ShowImage(segment.RotatedSegment()), true);
    }

    bool TestRotatedSegment4(Tester t)
    {
        var segment = new Segment(50, 10, 208, new Tip());
        return t.CheckExpect(ShowImage(segment.RotatedSegment(), "RotatedSegment4"), true);
    }

    bool TestInvertedRotatedSegment4(Tester t)
    {
        var segment = new Segment(50, 10, 208, new Tip());
        var invertedSegment = segment.InvertedRotatedSegment();
        return t.CheckExpect(ShowImage(invertedSegment, "InvertedRotatedSegment4"), true);
    }

    bool TestInvertedRotatedSegmentPinhole(Tester t)
    {
        var segment = new Segment(50, 10, 90, new Tip());
        return t.CheckInexact(segment.InvertedRotatedSegment().Pinhole, new Point(0, 25.0), 0.001);
    }
}

