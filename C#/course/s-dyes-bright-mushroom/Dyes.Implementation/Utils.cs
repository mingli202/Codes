namespace Dyes;

public class Utils
{
    public static bool CompareDoubles(double a, double b, double tolerance = 0.001D) => Math.Abs(a - b) < tolerance;
}
