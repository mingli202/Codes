namespace Beverages;

public abstract class ABeverage(string variety, int size, ILoString mixins) : IBeverage
{
    protected string variety { get; } = variety;
    protected int size { get; } = size;
    protected ILoString mixins { get; } = mixins;

    public abstract bool IsDecaf();
    public bool ContainsIngredient(string ingredient) => mixins.Contains(ingredient);
    public abstract string Format();
    public string FormatList()
    {
        string s = mixins.Format();

        if (s.Length == 0)
            return "without mixins";

        return "with " + s;
    }
}

// Represents a bubble-tea drink, with various mixins
public class BubbleTea : ABeverage
{
    /// <summary>
    /// Creates a new bubble-tea drink.
    /// </summary>
    /// <param name="variety">The variety of the tea. Black tea, Oolong, Green tea, etc.</param>
    /// <param name="mixins">The ingredients of the tea. boba, extra sugar, milk, etc.</param>
    /// <param name="size">The size of the tea. In ounces.</param>
    public BubbleTea(string variety, ILoString mixins, int size) : base(variety, size, mixins) { }

    override
    public bool IsDecaf() => variety.ToLower().Equals("rooibos");

    override
    public string Format() => $"{size}oz {variety} ({FormatList()})";
}


// Represents any coffee-based drink
public class Coffee : ABeverage
{
    readonly string style; // americano, demitasse, espresso, etc.
    readonly bool isIced; // whether it's cold or hot

    /// <summary>
    /// Creates a new coffee drink.
    /// </summary>
    /// <param name="variety">The variety of the coffee. Arabica, Robusta, Excelsa, Liberica, etc.</param>
    /// <param name="mixins">The ingredients of the coffee. cream, sugar, flavored syrup, etc.</param>
    /// <param name="style">The style of the coffee. americano, demitasse, espresso, etc.</param>
    /// <param name="isIced">Whether the coffee is cold or hot.</param>
    public Coffee(string variety, ILoString mixins, string style, bool isIced) : base(variety, 0, mixins)
    {
        this.style = style;
        this.isIced = isIced;
    }

    override
    public bool IsDecaf() => false;

    override
    public string Format() => $"{(isIced ? "Iced" : "Hot")} {variety} {style} ({FormatList()})";
}

// Represents an ice-cream-based blended drink
public class Milkshake : ABeverage
{
    readonly string brandName; // Ben&Jerrys, JPLicks, etc

    /// <summary>
    /// Creates a new milkshake drink.
    /// </summary>
    /// <param name="flavor">The flavor of the milkshake. vanilla, mint-chip, strawberry, etc.</param>
    /// <param name="toppings">The toppings of the milkshake. whipped cream, sprinkles, cookie crumbles, etc.</param>
    /// <param name="brandName">The brand of the milkshake. Ben&Jerrys, JPLicks, etc.</param>
    /// <param name="size">The size of the milkshake. In ounces.</param>
    public Milkshake(string flavor, ILoString toppings, string brandName, int size) : base(flavor, size, toppings)
    {
        this.brandName = brandName;
    }

    override
    public bool IsDecaf() => true;


    override
    public string Format() => $"{size}oz {brandName} {variety} ({FormatList()})";
}
