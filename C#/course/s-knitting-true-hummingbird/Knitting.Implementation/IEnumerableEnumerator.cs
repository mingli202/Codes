namespace Knitting;

// You may enhance the interfaces with new methods if necessary,
// but you may not edit these existing methods.

public interface IEnumerableEnumerator<T> : IEnumerator<T>, IEnumerable<T>
    where T : notnull
{
    /// <summary>
    /// Add an item to the end of the enumerable.
    /// </summary>
    IEnumerableEnumerator<T> Add(T item);

    /// <summary>
    /// Repeat the enumerable the specified number of times.
    /// </summary>
    IEnumerableEnumerator<T> Repeat(int numberOfRepeats);

    /// <summary>
    /// Clone the enumerable. Leaves the original untouched, so consuming the clone will not affect the original.
    /// </summary>
    IEnumerableEnumerator<T> Clone();

    /// <summary>
    /// Aggregates the sequence into a single value.
    /// </summary>
    /// <typeparam name="TReturn">The type of the accumulated result.</typeparam>
    /// <param name="seed">The initial accumulator value.</param>
    /// <param name="func">The function used to combine the accumulator with each item.</param>
    /// <returns>The final accumulated value.</returns>
    TReturn Fold<TReturn>(TReturn seed, Func<TReturn, T, TReturn> func);

    /// <summary>
    /// Renders the sequence by concatenating the string form of each item.
    /// </summary>
    /// <returns>A string containing each item in enumeration order.</returns>
    string ToString();
}

public class EnumerableEnumerator<T> : IEnumerableEnumerator<T>
    where T : notnull
{
    private Func<IEnumerable<T>> factory;
    private IEnumerator<T>? enumerator;

    /// <summary>
    /// Creates an enumerator wrapper backed by the supplied sequence factory.
    /// </summary>
    /// <param name="factory">The factory used to create fresh underlying sequences.</param>
    private EnumerableEnumerator(Func<IEnumerable<T>> factory)
    {
        this.factory = factory;
    }

    /// <summary>
    /// Wraps an enumerable in an <see cref="EnumerableEnumerator{T}"/>.
    /// </summary>
    /// <param name="item">The enumerable to wrap.</param>
    /// <returns>A new enumerator backed by <paramref name="item"/>.</returns>
    public static EnumerableEnumerator<T> From(IEnumerable<T> item) => new(() => item);

    /// <summary>
    /// Wraps a single-pass enumerator in an <see cref="EnumerableEnumerator{T}"/>.
    /// </summary>
    /// <param name="enumerator">The enumerator to consume.</param>
    /// <returns>A new enumerator that yields the remaining items from <paramref name="enumerator"/>.</returns>
    public static EnumerableEnumerator<T> From(IEnumerator<T> enumerator)
    {
        IEnumerable<T> Fn()
        {
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }

        return new EnumerableEnumerator<T>(Fn);
    }

    /// <summary>
    /// Lazily creates and caches the active enumerator instance.
    /// </summary>
    /// <returns>The underlying enumerator used for imperative enumeration members.</returns>
    private IEnumerator<T> Enum()
    {
        enumerator ??= factory().GetEnumerator();
        return enumerator;
    }

    /// <summary>
    /// Creates a fresh enumerator over the wrapped sequence.
    /// </summary>
    /// <returns>A new enumerator instance.</returns>
    public IEnumerator<T> GetEnumerator() => new EnumerableEnumerator<T>(factory);

    /// <summary>
    /// Creates a fresh generic enumerator over the wrapped sequence.
    /// </summary>
    /// <returns>A new generic enumerator instance.</returns>
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Creates a fresh non-generic enumerator over the wrapped sequence.
    /// </summary>
    /// <returns>A new non-generic enumerator instance.</returns>
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() =>
        GetEnumerator();

    /// <summary>
    /// Gets the current item from the active enumeration.
    /// </summary>
    /// <returns>The current item boxed as an object.</returns>
    object System.Collections.IEnumerator.Current => Current;

    /// <summary>
    /// Gets the current item from the active enumeration.
    /// </summary>
    /// <returns>The current item.</returns>
    public T Current => Enum().Current;

    /// <summary>
    /// Advances the active enumeration by one item.
    /// </summary>
    /// <returns><see langword="true"/> when another item is available; otherwise, <see langword="false"/>.</returns>
    public bool MoveNext() => Enum().MoveNext();

    /// <summary>
    /// Resets the active enumeration to its initial position.
    /// </summary>
    public void Reset() => Enum().Reset();

    /// <summary>
    /// Releases resources held by the active enumerator.
    /// </summary>
    public void Dispose() => Enum().Dispose();

    # region IEnumerableEnumerator<T> methods

    /// <summary>
    /// Appends an item to the end of the sequence.
    /// </summary>
    /// <param name="stitch">The item to append.</param>
    /// <returns>A new enumerator that yields the current items followed by <paramref name="stitch"/>.</returns>
    public IEnumerableEnumerator<T> Add(T stitch)
    {
        var source = this;
        IEnumerable<T> Fn()
        {
            foreach (var item in source)
            {
                yield return item;
            }
            yield return stitch;
        }
        return new EnumerableEnumerator<T>(Fn);
    }

    /// <summary>
    /// Repeats the sequence a fixed number of times.
    /// </summary>
    /// <param name="numberOfRepeats">The number of times to replay the sequence.</param>
    /// <returns>A new enumerator containing the repeated items in order.</returns>
    public IEnumerableEnumerator<T> Repeat(int numberOfRepeats)
    {
        IEnumerableEnumerator<T> source = this;
        IEnumerable<T> Fn()
        {
            for (int i = 0; i < numberOfRepeats; i++)
            {
                // Make a copy of the source to solve single pass items
                IEnumerableEnumerator<T> newSource = EnumerableEnumerator<T>.From([]);

                foreach (var item in source)
                {
                    newSource = newSource.Add(item);
                    yield return item;
                }

                source = newSource;
            }
        }

        return new EnumerableEnumerator<T>(Fn);
    }

    /// <summary>
    /// Duplicates the remaining sequence into two independent buffered enumerators.
    /// </summary>
    /// <returns>A clone that can be consumed without affecting the reset original sequence.</returns>
    public IEnumerableEnumerator<T> Clone()
    {
        List<T> listA = [];
        List<T> listB = [];

        foreach (var item in this)
        {
            listA.Add(item);
            listB.Add(item);
        }

        factory = () => listA;

        return new EnumerableEnumerator<T>(() => listB);
    }


    /// <summary>
    /// Aggregates the sequence into a single value by iterating through each item once.
    /// </summary>
    /// <typeparam name="TReturn">The type of the accumulated result.</typeparam>
    /// <param name="seed">The initial accumulator value.</param>
    /// <param name="func">The function used to combine the accumulator with each item.</param>
    /// <returns>The final accumulated value.</returns>
    public TReturn Fold<TReturn>(TReturn seed, Func<TReturn, T, TReturn> func)
    {
        foreach (var item in this)
        {
            seed = func(seed, item);
        }
        return seed;
    }

    # endregion

    /// <summary>
    /// Concatenates the string representation of each item in the sequence.
    /// </summary>
    /// <returns>A string containing every rendered item in order.</returns>
    public override string ToString() => Fold("", (acc, s) => acc + s.ToString());
}
