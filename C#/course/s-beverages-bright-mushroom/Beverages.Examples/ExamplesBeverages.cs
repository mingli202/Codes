using TesterLib;

namespace Beverages;

class ExamplesBeverages
{

    // Your examples go here!
    bool TestDecafBubbleTea(Tester t)
    {
        IBeverage tea = new BubbleTea("Black", new EmptyLoString(), 200);
        return t.CheckExpect(tea.IsDecaf(), false);
    }

    bool TestDecafBubbleTea2(Tester t)
    {
        IBeverage tea = new BubbleTea("Rooibos", new LinkLoString("Rooibos", new EmptyLoString()), 200);
        return t.CheckExpect(tea.IsDecaf(), true);
    }

    bool TestDecafCoffee(Tester t)
    {
        IBeverage coffee = new Coffee("Arabica", new LinkLoString("decaf", new EmptyLoString()), "americano", false);
        return t.CheckExpect(coffee.IsDecaf(), false);
    }

    bool TestDecafMilkshake(Tester t)
    {
        IBeverage milkshake = new Milkshake("vanilla", new LinkLoString("whipped cream", new EmptyLoString()), "Ben&Jerrys", 200);
        return t.CheckExpect(milkshake.IsDecaf(), true);
    }

    bool TestContainsBubbleTea(Tester t)
    {
        IBeverage tea = new BubbleTea("Black", new LinkLoString("tea", new EmptyLoString()), 200);
        return t.CheckExpect(tea.ContainsIngredient("tea"), true) && t.CheckExpect(tea.ContainsIngredient("cream"), false);
    }

    bool TestContainsCoffee(Tester t)
    {
        IBeverage coffee = new Coffee("Arabica", new LinkLoString("decaf", new EmptyLoString()), "americano", false);
        return t.CheckExpect(coffee.ContainsIngredient("decaf"), true) && t.CheckExpect(coffee.ContainsIngredient("cream"), false);
    }

    bool TestContainsMilkshake(Tester t)
    {
        IBeverage milkshake = new Milkshake("vanilla", new LinkLoString("whipped cream", new EmptyLoString()), "Ben&Jerrys", 200);
        return t.CheckExpect(milkshake.ContainsIngredient("whipped cream"), true) && t.CheckExpect(milkshake.ContainsIngredient("cream"), false);
    }
    bool TestFormatBubbleTea(Tester t)
    {
        IBeverage tea = new BubbleTea("Black", new LinkLoString("boba", new LinkLoString("milk", new EmptyLoString())), 24);
        return t.CheckExpect(tea.Format(), "24oz Black (with boba, milk)");
    }

    bool TestFormatBubbleTeaNoMixins(Tester t)
    {
        IBeverage tea = new BubbleTea("Black", new EmptyLoString(), 24);
        return t.CheckExpect(tea.Format(), "24oz Black (without mixins)");
    }

    bool TestFormatCoffee(Tester t)
    {
        IBeverage coffee = new Coffee("Arabica", new LinkLoString("cream", new LinkLoString("sugar", new LinkLoString("hazelnut syrup", new EmptyLoString()))), "americano", false);
        return t.CheckExpect(coffee.Format(), "Hot Arabica americano (with cream, sugar, hazelnut syrup)");
    }

    bool TestFormatCoffee2(Tester t)
    {
        IBeverage coffee = new Coffee("Robusta", new EmptyLoString(), "demitasse", true);
        return t.CheckExpect(coffee.Format(), "Iced Robusta demitasse (without mixins)");
    }

    bool TestFormatMilkshake(Tester t)
    {
        IBeverage milkshake = new Milkshake("strawberry", new EmptyLoString(), "Haagen-Dasz", 24);
        return t.CheckExpect(milkshake.Format(), "24oz Haagen-Dasz strawberry (without mixins)");
    }

    bool TestFormatMilkshake2(Tester t)
    {
        IBeverage milkshake = new Milkshake("peach", new LinkLoString("sprinkles", new EmptyLoString()), "JPLicks", 32);
        return t.CheckExpect(milkshake.Format(), "32oz JPLicks peach (with sprinkles)");
    }

}

