namespace NumberSequences;

public interface ILoInt
{
    /// <summary>
    /// Determines if this list of sequence is Fibonacci-like. E.g. that it follows the rule F(n + 1) = F(n-1) + F(n). Starting numbers can be anything.
    /// </summary>
    /// <returns>True if the list is Fibonacci-like, false otherwise.</returns>
    bool IsFibLike();


    /// <summary>
    /// Determines if this list of sequence is Pell-like. E.g. that it follows the rule F(n + 1) = F(n-1) + 2 * F(n). Starting numbers can be anything.
    /// </summary>
    /// <returns>True if the list is Pell-like, false otherwise.</returns>
    bool IsPellLike();

    /// <summary>
    /// Determines if this list of sequence is Nega-Fibonacci-like. E.g. that it follows the rule F(n + 1) = F(n-1) - F(n). Starting numbers can be anything.
    /// </summary>
    /// <returns>True if the list is Nega-Fibonacci-like, false otherwise.</returns>
    bool IsNegaFibLike();

    /// <summary>
    /// Determines if this list of sequence is Jacobsthal-like. E.g. that it follows the rule F(n + 1) = 2 * F(n-1) + F(n). Starting numbers can be anything.
    /// </summary>
    /// <returns>True if the list is Jacobsthal-like, false otherwise.</returns>
    bool IsJacobsthalLike();

    /// <summary>
    /// Computes the second largest number in this list of sequence. Repeated numbers are counted as different numbers
    /// </summary>
    /// <returns>The second largest number in this list of sequence.</returns>
    int SecondLargestNum();

    /// <summary>
    /// Computes the fifth largest number in this list of sequence. Repeated numbers are counted as different numbers
    /// </summary>
    /// <returns>The second largest number in this list of sequence.</returns>
    int FifthLargestNum();

    /// <summary>
    /// Computes the most common number in this list of sequence. If multiple numbers are the most common, the smallest number is returned.
    /// </summary>
    /// <returns>The most common number in this list of sequence.</returns>
    int MostCommonNum();

    /// <summary>
    /// Computes the fifth most common number in this list of sequence. If multiple numbers are the fifth most common, the smallest number is returned.
    /// </summary>
    /// <returns>The fifth most common number in this list of sequence.</returns>
    int ThirdMostCommonNum();


    /// <summary>
    /// Helper function to conviniently pass a function to serve as the rule for checking if this sequence is [Pattern]-like.
    /// </summary>
    /// <param name="fn">The function to pass as the rule.</param>
    /// <returns>True if this sequence is [Pattern>]-like, false otherwise.</returns>
    bool WithFunc(Func<int, int, int> fn);

    /// <summary>
    /// Counts the number of times each number in this list of sequence occurs. It expects this list to be sorted. It returns a list of numbers where the element at index i is the number of times the element at index i of this list sorted without duplicates occurs.
    /// </summary>
    ///
    /// <example>
    /// this   = [1, 4, 5, 6, 4, 1, 4];
    /// sorted = [1, 1, 4, 4, 4, 5, 6]
    ///
    /// sortedWithoutDuplicates = [1, 4, 5, 6]
    /// occurrences             = [1, 3, 1, 1]
    /// </example>
    ///
    /// <returns>A list of numbers where ocurrences[i] is the number of times sortedWithoutDuplicates[i] occurs in this list of sequence.</returns>
    ILoInt CountOccurencesInSorted();

    /// <summary>
    /// The helper function for CountOccurencesInSorted. It is not meant to be used directly.
    /// </summary>
    /// <param name="prev">The previous number in the list of sequence.</param>
    /// <param name="count">The number of times the previous number occurs in the list of sequence.</param>
    /// <returns>A list of numbers where ocurrences[i] is the number of times sortedWithoutDuplicates[i] occurs in this list of sequence.</returns>
    ILoInt CountOccurencesInSortedInner(int prev, int count);

    /// <summary>
    /// Removes duplicates from this list of numbers. It expects this list to be sorted because it checks for the previous value in the list and decides to include itself or not.
    /// </summary>
    /// <returns>The list of sequence without duplicates.</returns>
    ILoInt RemoveDuplicatesInSorted();

    /// <summary>
    /// The helper function for RemoveDuplicatesInSorted. It is not meant to be used directly.
    /// </summary>
    /// <param name="prev">The previous number in the list of sequence.</param>
    /// <returns>The list of sequence without duplicates.</returns>
    ILoInt RemoveDuplicatesInSortedInner(int prev);

    /// <summary>
    /// Sorts this list of numbers with the given comparison function.
    /// </summary>
    /// <param name="compare">The function to compare two numbers and determine which one comes before the other. If a < b, then a comes before b.</param>
    /// <returns>The list of sequence sorted.</returns>
    ILoInt Sorted(Func<int, int, int> compare);

    /// <summary>
    /// Inserts a number into this list of sorted numbers with the given comparison function.
    /// </summary>
    /// <param name="x">The number to insert.</param>
    /// <param name="compare">The function to compare two numbers and determine which one comes before the other. If a < b, then a comes before b.</param>
    /// <returns>The sorted list of number with the number inserted.</returns>
    ILoInt InsertInSorted(int x, Func<int, int, int> compare);

    /// <summary>
    /// Takes the first n elements of this list of sequence.
    /// </summary>
    /// <param name="n">The number of elements to take.</param>
    /// <returns>A tuple of two lists. The first list is the first n elements of this list of sequence. The second list is the rest of the list of sequence.</returns>
    (ILoInt front, ILoInt back) Take(int n);

    /// <summary>
    /// Folds every element into an accumulator by applying an operation, returning the final result.
    /// </summary>
    /// <param name="initialValue">The initial value of the accumulator.</param>
    /// <param name="fn">The function to apply to every element of this list of sequence.</param>
    /// <typeparam name="T">The type of the accumulator.</typeparam>
    /// <returns>The final result of the fold.</returns>
    T Fold<T>(T initialValue, Func<T, int, T> fn);

    /// <summary>
    /// Convenient method to combine Take and Fold. It takes the first n elements of this list. Then it folds the rest of the list with the first n elements as the initial value of the accumulator.
    /// </summary>
    /// <param name="n">The number of elements to take.</param>
    /// <param name="fn">The function to apply to every element of this list of sequence.</param>
    /// <typeparam name="T">The type of the accumulator.</typeparam>
    /// <returns>The final result of the fold.</returns>
    ILoInt TakeAndFold(int n, Func<ILoInt, int, ILoInt> fn);

    /// <summary>
    /// Gets the element at the given index.
    /// </summary>
    /// <param name="index">The index of the element to get.</param>
    /// <returns>The element at the given index.</returns>
    int Get(int index);

    /// <summary>
    /// Gets the element at the given index. It is not meant to be used directly.
    /// </summary>
    /// <param name="index">The index of the element to get.</param>
    /// <returns>The element at the given index.</returns>
    int GetInner(int index);

    /// <summary>
    /// Gets the index of the first element that is equal to the given value in this list.
    /// </summary>
    /// <param name="value">The value to search for.</param>
    /// <returns>The index of the first element that is equal to the given value.</returns>
    int IndexOf(int value);

    /// <summary>
    /// Gets the index of the first element that is equal to the given value in this list. It is not meant to be used directly.
    /// </summary>
    /// <param name="value">The value to search for.</param>
    /// <param name="index">The index of the element to start the search from.</param>
    /// <returns>The index of the first element that is equal to the given value.</returns>
    int IndexOfInner(int value, int index);

    /// <summary>
    /// Gets the maximum element in this list.
    /// </summary>
    /// <returns>The maximum element in this list.</returns>
    int Max();

    /// <summary>
    /// Gets the length of this list.
    /// </summary>
    /// <returns>The length of this list.</returns>
    int Length();
}

public abstract class ALoInt : ILoInt
{
    #region methods to implement
    abstract public bool IsFibLike();
    abstract public bool IsPellLike();
    abstract public bool IsNegaFibLike();
    abstract public bool IsJacobsthalLike();
    abstract public int SecondLargestNum();
    abstract public int FifthLargestNum();
    abstract public int MostCommonNum();
    abstract public int ThirdMostCommonNum();
    #endregion

    #region interface helpers
    abstract public bool WithFunc(Func<int, int, int> fn);

    abstract public ILoInt CountOccurencesInSorted();
    abstract public ILoInt CountOccurencesInSortedInner(int prev, int count);


    abstract public (ILoInt front, ILoInt back) TakeInner(int n);
    abstract public ILoInt RemoveDuplicatesInSorted();
    abstract public ILoInt RemoveDuplicatesInSortedInner(int prev);
    abstract public ILoInt Sorted(Func<int, int, int> compare);
    abstract public ILoInt InsertInSorted(int x, Func<int, int, int> predicate);

    public (ILoInt front, ILoInt back) Take(int n) => n < 0 ? throw new ArgumentException("Can't take by less than 0!") :
        TakeInner(n);
    abstract public T Fold<T>(T initialValue, Func<T, int, T> fn);
    public ILoInt TakeAndFold(int n, Func<ILoInt, int, ILoInt> fn)
    {
        var (front, back) = Take(n);
        return back.Fold(front, fn);
    }

    abstract public int Get(int index);
    abstract public int GetInner(int index);
    abstract public int IndexOf(int value);
    abstract public int IndexOfInner(int value, int index);
    abstract public int Max();
    abstract public int Length();
    #endregion
}
