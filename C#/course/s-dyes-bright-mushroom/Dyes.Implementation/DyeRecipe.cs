namespace Dyes;

class DyeRecipe
{
    public readonly double red;
    public readonly double yellow;
    public readonly double blue;
    public readonly double black;

    private readonly double tolerance = 0.001D;

    /// <summary>
    /// Main constructor, validates all constraints and ensures total is 1g. All other constructors funnel into this one.
    /// </summary>
    public DyeRecipe(double red, double yellow, double blue, double black)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(red);
        ArgumentOutOfRangeException.ThrowIfNegative(yellow);
        ArgumentOutOfRangeException.ThrowIfNegative(blue);
        ArgumentOutOfRangeException.ThrowIfNegative(black);

        double otherTotal = red + yellow + blue;


        bool blackIsSmall = black <= 0.05 * otherTotal + tolerance;
        bool othersAreSmall = otherTotal <= 0.10 * black + tolerance;
        if (!blackIsSmall && !othersAreSmall)
        {
            throw new ArgumentOutOfRangeException(
                "Black must be at most 5% of the other colors combined, "
                    + "or the other colors must be at most 10% of the black pigment."
            );
        }
        if (yellow > 0 && blue > yellow / 10.0D)
        {
            throw new ArgumentOutOfRangeException("When yellow is present, blue must be no more than a tenth of the yellow dye.");
        }


        this.red = red;
        this.yellow = yellow;
        this.blue = blue;
        this.black = black;
    }

    /// <summary>
    /// Private constructor that accepts a tuple (for convenience) and funnels to the main constructor.
    /// </summary>
    private DyeRecipe((double red, double yellow, double blue, double black) values)
        : this(values.red, values.yellow, values.blue, values.black)
    {
    }

    /// <summary>
    /// Constructor that takes red, yellow, and blue dyes and produces the darkest perfect dye recipe.
    /// </summary>
    public DyeRecipe(double red, double yellow, double blue)
        : this(DarkestRecipe(red, yellow, blue))
    {
    }

    private static (double red, double yellow, double blue, double black) DarkestRecipe(double red, double yellow, double blue)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(red);
        ArgumentOutOfRangeException.ThrowIfNegative(yellow);
        ArgumentOutOfRangeException.ThrowIfNegative(blue);

        double colorsTotal = red + yellow + blue;
        if (colorsTotal <= 0)
        {
            throw new ArgumentOutOfRangeException("Total dye weight must be greater than 0.");
        }

        double newBlue = blue;
        if (yellow > 0D)
        {
            newBlue = blue > 0.1 * yellow ? 0.1 * yellow : blue;
        }

        colorsTotal = red + yellow + newBlue;
        // Calculate the maximum black we can add (5% of other colors)
        double maxBlack = 0.05 * colorsTotal;


        // Scale to 1 gram total
        double total = colorsTotal + maxBlack;
        double scaleFactor = 1.0 / total;

        return (red * scaleFactor, yellow * scaleFactor, newBlue * scaleFactor, maxBlack * scaleFactor);
    }
}

