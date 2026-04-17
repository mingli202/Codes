namespace Sokoban.Lists;

/// <summary>
/// A functional list of values.
/// </summary>
/// <typeparam name="T">The element type.</typeparam>
public abstract class ListOf<T>
{
    /// <summary>
    /// Checks if all elements in the list satisfy the specified predicate.
    /// Stops after the first element that does not satisfy the predicate.
    /// </summary>
    /// <param name="predicate">The predicate to check for.</param>
    /// <returns>True if all elements satisfy the predicate, false otherwise.</returns>
    public abstract bool All(Func<T, int, bool> predicate);

    /// <summary>
    /// Checks if any element in the list satisfies the specified predicate.
    /// Stops after the first element that satisfies the predicate.
    /// </summary>
    /// <param name="predicate">The predicate to check for.</param>
    /// <returns>True if any element satisfies the predicate, false otherwise.</returns>
    public abstract bool Any(Func<T, int, bool> predicate);

    /// <summary>
    /// Checks if the list contains the specified element.
    /// </summary>
    /// <param name="val">The element to check for.</param>
    /// <returns>True if the list contains the specified element, false otherwise.</returns>
    public bool Contains(T val) => Any((v, _) => v is not null && v.Equals(val));

    /// <summary>
    /// Compares two lists for equality.
    /// </summary>
    /// <param name="other">The other list to compare.</param>
    /// <returns>True if the lists are equal, false otherwise.</returns>
    public abstract bool Equals(ListOf<T> other);

    /// <summary>
    /// Comapres this list to a LinkLo list.
    /// </summary>
    /// <param name="other">The other list to compare.</param>
    /// <returns>True if the lists are equal, false otherwise.</returns>
    public abstract bool EqualsLinkLo(LinkLo<T> other);

    /// <summary>
    /// Comapres this list to an EmptyLo list.
    /// </summary>
    /// <param name="other">The other list to compare.</param>
    /// <returns>True if the lists are equal, false otherwise.</returns>
    public abstract bool EqualsEmptyLo(EmptyLo<T> other);

    /// <summary>Keeps only the elements that satisfy the specified predicate.</summary>
    /// <param name="predicate">The predicate to keep elements that satisfy.</param>
    /// <returns>A new list of elements that satisfy the predicate.</returns>
    public ListOf<T> Filter(Func<T, int, bool> predicate)
    {
        ListOf<T> initial = new EmptyLo<T>();
        return Fold(initial, (acc, item, i) => predicate(item, i) ? new LinkLo<T>(item, acc) : acc);
    }

    /// <summary>Gets the first element that satisfies the specified predicate.</summary>
    /// <param name="predicate">The predicate to test elements against.</param>
    /// <returns>The first element that satisfies the predicate, or None if no element satisfies the predicate.</returns>
    public abstract Option<T> Find(Func<T, int, bool> predicate);

    /// <summary>
    /// Folds every element into an accumulator by applying an operation, returning the final result.
    /// </summary>
    /// <param name="initialValue">The initial value of the accumulator.</param>
    /// <param name="fn">The function to apply to every element of this list of sequence.</param>
    /// <typeparam name="TResult">The type of the accumulator.</typeparam>
    /// <returns>The final result of the fold.</returns>
    public abstract TResult Fold<TResult>(TResult initialValue, Func<TResult, T, int, TResult> fn);

    /// <summary>
    /// Gets the element at the specified index.
    /// </summary>
    /// <param name="index">The index of the element to get.</param>
    /// <returns>The element at the specified index.</returns>
    public Option<T> Get(int index) => Find((_, i) => i == index);

    /// <summary>
    /// Gets the length of the list.
    /// </summary>
    /// <returns>The length of the list.</returns>
    public abstract int Length();

    /// <summary>
    /// Maps the elements of the list to a new type.
    /// </summary>
    /// <typeparam name="TResult">The type to map to.</typeparam>
    /// <param name="fn">The function to map with.</param>
    /// <returns>A list of mapped elements.</returns>
    public ListOf<TResult> Map<TResult>(Func<T, int, TResult> fn)
    {
        ListOf<TResult> initial = new EmptyLo<TResult>();
        return Fold(initial, (acc, item, index) => new LinkLo<TResult>(fn(item, index), acc));
    }

    /// <summary>
    /// Sets the element at the specified index.
    /// </summary>
    /// <param name="index">The index of the element to set.</param>
    /// <param name="value">The value to set.</param>
    /// <returns>A new list with the element set.</returns>
    public ListOf<T> Set(int index, T value) =>
        index < 0 || index >= Length()
            ? throw new ArgumentException("Index out of range")
            : Map((item, i) => i == index ? value : item);

    /// <summary>
    /// Splits the list into a list of lists at the specified separator.
    /// </summary>
    /// <param name="separator">The separator to split at.</param>
    /// <returns>A list of lists.</returns>
    public abstract ListOf<ListOf<T>> SplitAt(T separator);

    /// <summary>
    /// Helper method for splitting the list into a list of lists at the specified separator. Should not be used directly.
    /// </summary>
    /// <param name="separator">The separator to split at.</param>
    /// <returns>A tuple containing the sub-lists and the accumulator.</returns>
    public abstract (ListOf<ListOf<T>> subLists, ListOf<T> acc) SplitAtHelper(T separator);

    /// <summary>
    /// Gets a string representation of the list.
    /// </summary>
    /// <returns>A string representation of the list.</returns>
    public abstract override string ToString();

    /// <summary>
    /// Helper method for getting a string representation of the list. Should not be used directly.
    /// </summary>
    /// <returns>A string representation of the list.</returns>
    public abstract string ToStringHelper();
}
