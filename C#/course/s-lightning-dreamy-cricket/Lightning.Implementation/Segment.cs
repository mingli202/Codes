using System.Windows;
using System.Windows.Media;
using ImageLib;
using ImageLib.Enumerations;

namespace Lightning;

public class Segment(int length, int current, double theta, ILightningBolt bolt) : ILightningBolt
{
    public ILightningBolt Combine(int leftLength, int rightLength, int leftCapacity, int rightCapacity, double leftTheta, double rightTheta, ILightningBolt otherBolt)
        => new Fork(leftLength, rightLength, leftCapacity, rightCapacity, leftTheta, rightTheta, OffsetAngle(leftTheta), otherBolt.OffsetAngle(rightTheta));

    public ILightningBolt OffsetAngle(double angle) => new Segment(length, current, theta + (angle - 90), bolt.OffsetAngle(angle));

    public WorldImage Draw()
    {
        var rotatedSegment = RotatedSegment();
        var finalRotatedSegmentPinhole = GetFinalRotatedSegmentPinhole();
        var difference = DifferenceBetweenPoints(finalRotatedSegmentPinhole, rotatedSegment.Pinhole);
        var overlayImage = new OverlayImage(bolt.Draw(), rotatedSegment);
        return overlayImage.MovePinholeTo(OffsetPoint(overlayImage.Pinhole, difference), AlignMode.Pinhole);
    }

    public double GetWidth()
    {
        var (leftMostX, rightMostX) = GetLeftAndRightMostXs(0);
        Console.WriteLine(this);
        return Math.Abs(leftMostX - rightMostX);
    }

    public (double leftMostX, double rightMostX) GetLeftAndRightMostXs(double offset)
        => (Math.Min(bolt.GetLeftAndRightMostXs(offset + CalculateX()).leftMostX, offset), Math.Max(bolt.GetLeftAndRightMostXs(offset + CalculateX()).rightMostX, offset));

    public bool IsPhysicallyPossible()
    {
        Console.WriteLine(ToString());
        return bolt.HasSmallerCurrentThan(current);
    }

    public bool HasSmallerCurrentThan(int maxCurrent) => current <= maxCurrent && bolt.HasSmallerCurrentThan(current);

    #region Private helpers for Draw()

    /// <summary>
    /// Gets the final pinhole point of the segment to adjust the pinhole of the overlay image with the bolt.
    /// </summary>
    /// <returns>pinhole point</returns>
    public Point GetFinalRotatedSegmentPinhole() => InvertedRotatedSegment().Pinhole;

    /// <summary>
    /// Gets the rotated segment of this segment.
    /// </summary>
    /// <returns>rotated segment</returns>
    public WorldImage RotatedSegment() => new RotateImage(RectangleSegment(), theta);

    /// <summary>
    /// Gets the inverted rotated segment of this segment.
    /// </summary>
    /// <returns>inverted rotated segment</returns>
    public WorldImage InvertedRotatedSegment() => new RotateImage(RectangleSegment(), theta + 180);

    /// <summary>
    /// Gets the rectangle segment of this segment with the proper pinhole.
    /// </summary>
    /// <returns>rectangle segment</returns>
    public WorldImage RectangleSegment() => new RectangleImage(length, GetHeightProportionalToCurrent(), OutlineMode.Fill, Colors.LightBlue).MovePinholeTo(AlignMode.CenterLeft);

    /// <summary>
    /// Gets the height proportional to the current.
    /// </summary>
    /// <returns>height proportional to current</returns>
    public int GetHeightProportionalToCurrent() => (int)(1.5 * current) + 5;

    /// <summary>
    /// Offsets a point by a given amount.
    /// </summary>
    /// <param name="point">point to offset</param>
    /// <param name="dx">x offset</param>
    /// <param name="dy">y offset</param>
    /// <returns>offset point</returns>
    public Point OffsetPoint(Point point, double dx, double dy) => new(point.X + dx, point.Y + dy);

    /// <summary>
    /// Offsets a point by a given amount.
    /// </summary>
    /// <param name="point">point to offset</param>
    /// <param name="dPoint">point to offset by</param>
    /// <returns>offset point</returns>
    public Point OffsetPoint(Point point, Point dPoint) => OffsetPoint(point, dPoint.X, dPoint.Y);

    /// <summary>
    /// Gets the difference between two points.
    /// </summary>
    /// <param name="point1">first point</param>
    /// <param name="point2">second point</param>
    /// <returns>difference between points</returns>
    public Point DifferenceBetweenPoints(Point point1, Point point2) => new(point1.X - point2.X, point1.Y - point2.Y);

    #endregion

    #region Private helpers for GetWidth()

    /// <summary>
    /// Calculates the X coordinate of this segment.
    /// </summary>
    /// <returns>X coordinate</returns>
    public double CalculateX() => Math.Cos(ToRadians()) * length;

    /// <summary>
    /// Converts the theta to radians.
    /// </summary>
    /// <returns>radians</returns>
    public double ToRadians() => theta * Math.PI / 180;

    #endregion
}


