using ImageLib;

namespace Lightning;

public interface ILightningBolt
{
    /// <summary>
    /// Draws the bolt recursively.
    /// </summary>
    /// <returns>bolt image</returns>
    WorldImage Draw();

    bool IsPhysicallyPossible();

    /// <summary>
    /// Gets whether this bolt has greater current than the given current.
    /// </summary>
    /// <param name="current">current</param>
    /// <returns>whether this bolt has greater current than the given current</returns>
    bool HasSmallerCurrentThan(int maxCurrent);

    /// <summary>
    /// This method takes the current bolt and a given bolt and produces a Fork using the given arguments, with this bolt on the left and the given bolt on the right... but with a twist, literally.
    /// </summary>
    /// <param name="leftLength">left length</param>
    /// <param name="rightLength">right length</param>
    /// <param name="leftCapacity">left capacity</param>
    /// <param name="rightCapacity">right capacity</param>
    /// <param name="leftTheta">left theta</param>
    /// <param name="rightTheta">right theta</param>
    /// <param name="otherBolt">other bolt</param>
    /// <returns>fork</returns>
    ILightningBolt Combine(int leftLength, int rightLength, int leftCapacity, int rightCapacity, double leftTheta, double rightTheta, ILightningBolt otherBolt);

    /// <summary>
    /// Offsets the angle of the bolt.
    /// </summary>
    /// <param name="angle">angle</param>
    /// <returns>this bolt</returns>
    ILightningBolt OffsetAngle(double angle);

    /// <summary>
    /// Design the method double GetWidth() that returns the width of this bolt, from the leftmost tip to the rightmost tip. For simplicity, ignore the actual thicknesses of segments or the branches of a fork themselves, and just assume that the thickness of each bolt is zero.
    /// </summary>
    /// <returns>width</returns>
    double GetWidth();

    /// <summary>
    /// Returns the leftmost and rightmost X coordinates of this bolt, with the given offset.
    /// </summary>
    /// <param name="offset">offset</param>
    /// <returns>left and right most X coordinates</returns>
    (double leftMostX, double rightMostX) GetLeftAndRightMostXs(double offset);
}

