namespace NumberSequences;

// Do not modify this class, or any files in this Examplar project.
public class LinkLoInt : ALoInt
{
    readonly int first;
    readonly ILoInt rest;

    public LinkLoInt(int first, ILoInt rest)
    {
        this.first = first;
        this.rest = rest;
    }

    #region methods to implement
    override public bool IsFibLike() => WithFunc((a, b) => a + b);
    override public bool IsPellLike() => WithFunc((a, b) => a + 2 * b);
    override public bool IsNegaFibLike() => WithFunc((a, b) => a - b);
    override public bool IsJacobsthalLike() => WithFunc((a, b) => 2 * a + b);

    override public int FifthLargestNum() => TakeAndFold(5, (acc, x) => new LinkLoInt(x, acc).Sorted((a, b) => b - a).Take(5).front).Get(4);

    override public int MostCommonNum()
    {
        var sorted = Sorted((a, b) => a - b);
        var occurences = sorted.CountOccurencesInSorted();
        var max = occurences.Max();
        var indexOfMax = occurences.IndexOf(max);

        return sorted.RemoveDuplicatesInSorted().Get(indexOfMax);
    }

    override public int SecondLargestNum() => TakeAndFold(2, (acc, x) => new LinkLoInt(x, acc).Sorted((a, b) => b - a).Take(2).front).Get(1);

    override public int ThirdMostCommonNum()
    {
        if (Length() < 3)
            throw new ArgumentException("List not big enough!");

        var sorted = Sorted((a, b) => a - b);
        var occurences = sorted.CountOccurencesInSorted();

        if (occurences.Length() < 3)
            throw new ArgumentException("Your list has less than 3 distinct numbers!");

        var thirdMostCommonNum = occurences.Sorted((a, b) => b - a).Get(2);
        var indexOfThirdMostCommonNum = occurences.IndexOf(thirdMostCommonNum);


        return sorted.RemoveDuplicatesInSorted().Get(indexOfThirdMostCommonNum);
    }

    #endregion
    #region interface helpers
    override public bool WithFunc(Func<int, int, int> fn)
    {
        if (Length() < 3)
            return true;

        var (front, back) = Take(2);
        var fold = back.Fold((front, true), (acc, x) => FuncHandler(acc, x, fn));
        return fold.Item2;
    }

    override public ILoInt CountOccurencesInSorted() => rest.CountOccurencesInSortedInner(first, 1);
    override public ILoInt CountOccurencesInSortedInner(int prev, int count) => prev == first ? rest.CountOccurencesInSortedInner(first, count + 1) : new LinkLoInt(count, CountOccurencesInSorted());

    override public (ILoInt front, ILoInt back) TakeInner(int n)
    {
        if (n == 0)
            return (new EmptyLoInt(), this);

        var (front, back) = rest.Take(n - 1);
        return (new LinkLoInt(first, front), back);
    }

    override public ILoInt RemoveDuplicatesInSorted() => new LinkLoInt(first, RemoveDuplicatesInSortedInner(first));
    override public ILoInt RemoveDuplicatesInSortedInner(int prev) => prev == first ? rest.RemoveDuplicatesInSortedInner(first) : new LinkLoInt(first, rest.RemoveDuplicatesInSortedInner(first));

    override public ILoInt Sorted(Func<int, int, int> compare)
    {
        var sorted = rest.Sorted(compare);
        return sorted.InsertInSorted(first, compare);
    }

    override public ILoInt InsertInSorted(int x, Func<int, int, int> compare)
    {
        var cmp = compare(x, first);
        if (cmp < 1)
            return new LinkLoInt(x, this);
        else
            return new LinkLoInt(first, rest.InsertInSorted(x, compare));
    }


    override public T Fold<T>(T initialValue, Func<T, int, T> fn)
    {
        T nextVal = fn(initialValue, first);
        return rest.Fold(nextVal, fn);
    }

    override public int Get(int index) => index < 0 ? throw new ArgumentException("Can't get negative index!") : GetInner(index);
    override public int GetInner(int index) => index == 0 ? first : rest.GetInner(index - 1);
    override public int IndexOf(int value) => IndexOfInner(value, 0);
    override public int IndexOfInner(int value, int index) => first == value ? index : rest.IndexOfInner(value, index + 1);
    override public int Max() => rest.Fold(first, Math.Max);
    override public int Length() => 1 + rest.Length();

    #endregion
    #region private helpers
    private (ILoInt args, bool valid) FuncHandler((ILoInt args, bool valid) acc, int x, Func<int, int, int> fn)
    {
        var previousPrevious = acc.args.Get(0);
        var previous = acc.args.Get(1);
        var current = fn(previousPrevious, previous);

        return (ListWith(previous, current), acc.valid && current == x);
    }

    private ILoInt ListWith(int element1, int element2) => new LinkLoInt(element1, new LinkLoInt(element2, new EmptyLoInt()));
    #endregion
}
