using ImageLib;

namespace Lightning;

public class Fork(int leftLength, int rightLength, int leftCurrent, int rightCurrent, double leftTheta, double rightTheta, ILightningBolt left, ILightningBolt right) : ILightningBolt
{
    public ILightningBolt Combine(int leftLength, int rightLength, int leftCapacity, int rightCapacity, double leftTheta, double rightTheta, ILightningBolt otherBolt)
        => new Fork(leftLength, rightLength, leftCapacity, rightCapacity, leftTheta, rightTheta, OffsetAngle(leftTheta), otherBolt.OffsetAngle(rightTheta));

    public ILightningBolt OffsetAngle(double angle) => new Fork(leftLength, rightLength, leftCurrent, rightCurrent, leftTheta + angle - 90, rightTheta + angle - 90, left.OffsetAngle(angle), right.OffsetAngle(angle));

    public WorldImage Draw() => new OverlayImage(LeftAsSegment().Draw(), RightAsSegment().Draw());

    public double GetWidth()
    {
        var (leftMostX, rightMostX) = GetLeftAndRightMostXs(0);
        Console.WriteLine(this);
        return Math.Abs(leftMostX - rightMostX);
    }

    public (double leftMostX, double rightMostX) GetLeftAndRightMostXs(double offset)
    {
        var leftMostXs = LeftAsSegment().GetLeftAndRightMostXs(offset);
        var rightMostXs = RightAsSegment().GetLeftAndRightMostXs(offset);
        return (Math.Min(leftMostXs.leftMostX, rightMostXs.leftMostX), Math.Max(leftMostXs.rightMostX, rightMostXs.rightMostX));
    }


    public bool IsPhysicallyPossible()
    {
        Console.WriteLine(ToString());
        return LeftAsSegment().IsPhysicallyPossible() && RightAsSegment().IsPhysicallyPossible();
    }



    public bool HasSmallerCurrentThan(int maxCurrent) => LeftAsSegment().HasSmallerCurrentThan(maxCurrent) && RightAsSegment().HasSmallerCurrentThan(maxCurrent);

    /// <summary>
    /// Gets the left as segment.
    /// </summary>
    /// <returns>left segment</returns>
    private Segment LeftAsSegment() => new Segment(leftLength, leftCurrent, leftTheta, left);

    /// <summary>
    /// Gets the right as segment.
    /// </summary>
    /// <returns>right segment</returns>
    private Segment RightAsSegment() => new Segment(rightLength, rightCurrent, rightTheta, right);
}

