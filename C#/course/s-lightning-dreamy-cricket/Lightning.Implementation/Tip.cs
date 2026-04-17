using System.Drawing;
using System.Windows.Media;
using ImageLib;
using ImageLib.Enumerations;

namespace Lightning;

public class Tip : ILightningBolt
{
    public ILightningBolt Combine(int leftLength, int rightLength, int leftCapacity, int rightCapacity, double leftTheta, double rightTheta, ILightningBolt otherBolt)
        => new Fork(leftLength, rightLength, leftCapacity, rightCapacity, leftTheta, rightTheta, OffsetAngle(leftTheta), otherBolt.OffsetAngle(rightTheta));


    public ILightningBolt OffsetAngle(double angle) => this;

    public WorldImage Draw()
    {
        return new CircleImage(5, OutlineMode.Fill, Colors.LightGray).CenterPinhole();
    }

    public double GetWidth() => 0;

    public (double leftMostX, double rightMostX) GetLeftAndRightMostXs(double offsetX) => (offsetX, offsetX);


    public bool HasSmallerCurrentThan(int maxCurrent) => true;
    public bool IsPhysicallyPossible() => true;
}

