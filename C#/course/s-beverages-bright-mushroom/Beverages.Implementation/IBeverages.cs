namespace Beverages;

// Do not modify this file
public interface IBeverage
{
    /// <summary>
    /// Returns true if this beverage is decaf. Coffee is never offered decaf; only Rooibos tea is decaffeinated; and milkshakes are always decaffeinated.
    /// </summary>
    /// <returns>True if this beverage is decaf.</returns>
    bool IsDecaf();

    /// <summary>
    /// Returns true if this beverage contains the given ingredient.
    /// </summary>
    /// <param name="ingredient">The ingredient to search for.</param>
    /// <returns>True if this beverage contains the ingredient.</returns>
    bool ContainsIngredient(string ingredient);

    /// <summary>
    /// Returns a string representation of this beverage.
    /// </summary>
    /// <returns>A string representation of this beverage.</returns>
    string Format();
}
