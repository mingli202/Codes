namespace Sokoban.Lists;

/// <summary>
/// A functional list of values.
/// </summary>
/// <typeparam name="T">The element type.</typeparam>
public interface IListOf<T>
{
    /// <summary>
    /// Checks if all elements in the list satisfy the specified predicate.
    /// Stops after the first element that does not satisfy the predicate.
    /// </summary>
    /// <param name="predicate">The predicate to check for.</param>
    /// <returns>True if all elements satisfy the predicate, false otherwise.</returns>
    bool All(Func<T, int, bool> predicate);

    /// <summary>
    /// Checks if any element in the list satisfies the specified predicate.
    /// Stops after the first element that satisfies the predicate.
    /// </summary>
    /// <param name="predicate">The predicate to check for.</param>
    /// <returns>True if any element satisfies the predicate, false otherwise.</returns>
    bool Any(Func<T, int, bool> predicate);

    /// <summary>
    /// Checks if the list contains the specified element.
    /// </summary>
    /// <param name="val">The element to check for.</param>
    /// <returns>True if the list contains the specified element, false otherwise.</returns>
    bool Contains(T val);

    /// <summary>
    /// Compares two lists for equality.
    /// </summary>
    /// <param name="other">The other list to compare.</param>
    /// <returns>True if the lists are equal, false otherwise.</returns>
    bool Equals(IListOf<T> other);

    /// <summary>
    /// Compares this list to a <see cref="LinkLo{T}" />.
    /// </summary>
    /// <param name="other">The other list to compare.</param>
    /// <returns>True if the lists are equal, false otherwise.</returns>
    bool EqualsLinkLo(LinkLo<T> other);

    /// <summary>
    /// Compares this list to an <see cref="EmptyLo{T}" />.
    /// </summary>
    /// <param name="other">The other list to compare.</param>
    /// <returns>True if the lists are equal, false otherwise.</returns>
    bool EqualsEmptyLo(EmptyLo<T> other);

    /// <summary>Keeps only the elements that satisfy the specified predicate.</summary>
    /// <param name="predicate">The predicate to keep elements that satisfy.</param>
    /// <returns>A new list of elements that satisfy the predicate.</returns>
    IListOf<T> Filter(Func<T, int, bool> predicate);

    /// <summary>Gets the first element that satisfies the specified predicate.</summary>
    /// <param name="predicate">The predicate to test elements against.</param>
    /// <returns>The first element that satisfies the predicate, or None if no element satisfies the predicate.</returns>
    Option<T> Find(Func<T, int, bool> predicate);

    /// <summary>
    /// Folds every element into an accumulator by applying an operation, returning the final result.
    /// </summary>
    /// <param name="initialValue">The initial value of the accumulator.</param>
    /// <param name="fn">The function to apply to each element in the list.</param>
    /// <typeparam name="TResult">The type of the accumulator.</typeparam>
    /// <returns>The final result of the fold.</returns>
    TResult Fold<TResult>(TResult initialValue, Func<TResult, T, int, TResult> fn);

    /// <summary>
    /// Gets the element at the specified index.
    /// </summary>
    /// <param name="index">The index of the element to get.</param>
    /// <returns>The element at the specified index.</returns>
    Option<T> Get(int index);

    /// <summary>
    /// Gets the length of the list.
    /// </summary>
    /// <returns>The length of the list.</returns>
    int Length();

    /// <summary>
    /// Maps the elements of the list to a new type.
    /// </summary>
    /// <typeparam name="TResult">The type to map to.</typeparam>
    /// <param name="fn">The function to map with.</param>
    /// <returns>A list of mapped elements.</returns>
    IListOf<TResult> Map<TResult>(Func<T, int, TResult> fn);

    /// <summary>Pops the first element from the list.</summary>
    /// <returns>A new list with the first element removed.</returns>
    IListOf<T> PopFront();

    /// <summary>Pushes an element to the back of the list.</summary>
    /// <param name="value">The value to push.</param>
    /// <returns>A new list with the element pushed to the front.</returns>
    IListOf<T> PushFront(T value);

    /// <summary>
    /// Sets the element at the specified index.
    /// </summary>
    /// <param name="index">The index of the element to set.</param>
    /// <param name="value">The value to set.</param>
    /// <returns>A new list with the element set.</returns>
    IListOf<T> Set(int index, T value);

    /// <summary>Takes the specified number of elements from the list.</summary>
    /// <param name="count">The number of elements to skip.</param>
    /// <returns>A new list with the specified number of elements taken.</returns>
    IListOf<T> Take(int count);

    /// <summary>
    /// Splits the list into a list of lists at the specified separator.
    /// </summary>
    /// <param name="separator">The separator to split at.</param>
    /// <returns>A list of lists.</returns>
    IListOf<IListOf<T>> SplitAt(T separator);

    /// <summary>
    /// Helper method for splitting the list into a list of lists at the specified separator. Should not be used directly.
    /// </summary>
    /// <param name="separator">The separator to split at.</param>
    /// <returns>A tuple containing the sub-lists and the accumulator.</returns>
    (IListOf<IListOf<T>> subLists, IListOf<T> acc) SplitAtHelper(T separator);

    /// <summary>
    /// Gets a string representation of the list.
    /// </summary>
    /// <returns>A string representation of the list.</returns>
    string ToString();

    /// <summary>
    /// Helper method for getting a string representation of the list. Should not be used directly.
    /// </summary>
    /// <returns>A string representation of the list.</returns>
    string ToStringHelper();
}

/// <inheritdoc />
public abstract class ListOf<T> : IListOf<T>
{
    /// <inheritdoc />
    public abstract bool All(Func<T, int, bool> predicate);

    /// <inheritdoc />
    public abstract bool Any(Func<T, int, bool> predicate);

    /// <inheritdoc />
    public bool Contains(T val) => Any((v, _) => v is not null && v.Equals(val));

    /// <inheritdoc />
    public abstract bool Equals(IListOf<T> other);

    /// <inheritdoc />
    public abstract bool EqualsLinkLo(LinkLo<T> other);

    /// <inheritdoc />
    public abstract bool EqualsEmptyLo(EmptyLo<T> other);

    /// <inheritdoc />
    public IListOf<T> Filter(Func<T, int, bool> predicate)
    {
        IListOf<T> initial = new EmptyLo<T>();
        return Fold(initial, (acc, item, i) => predicate(item, i) ? new LinkLo<T>(item, acc) : acc);
    }

    /// <inheritdoc />
    public abstract Option<T> Find(Func<T, int, bool> predicate);

    /// <inheritdoc />
    public abstract TResult Fold<TResult>(TResult initialValue, Func<TResult, T, int, TResult> fn);

    /// <inheritdoc />
    public Option<T> Get(int index) => Find((_, i) => i == index);

    /// <inheritdoc />
    public abstract int Length();

    /// <inheritdoc />
    public IListOf<TResult> Map<TResult>(Func<T, int, TResult> fn)
    {
        IListOf<TResult> initial = new EmptyLo<TResult>();
        return Fold(initial, (acc, item, index) => new LinkLo<TResult>(fn(item, index), acc));
    }

    /// <inheritdoc />
    public abstract IListOf<T> PopFront();

    /// <inheritdoc />
    public IListOf<T> PushFront(T value) => new LinkLo<T>(value, this);

    /// <inheritdoc />
    public IListOf<T> Set(int index, T value) =>
        index < 0 || index >= Length() ? this : Map((item, i) => i == index ? value : item);

    /// <inheritdoc />
    public abstract IListOf<T> Take(int count);

    /// <inheritdoc />
    public abstract IListOf<IListOf<T>> SplitAt(T separator);

    /// <inheritdoc />
    public abstract (IListOf<IListOf<T>> subLists, IListOf<T> acc) SplitAtHelper(T separator);

    /// <inheritdoc />
    public abstract override string ToString();

    /// <inheritdoc />
    public abstract string ToStringHelper();
}
