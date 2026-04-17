namespace NumberSequences;

// Do not modify this class, or any files in this Examplar project.
public class EmptyLoInt : ALoInt
{
    public EmptyLoInt()
    {
    }

    #region methods to implement
    override public bool IsFibLike() => true;
    override public bool IsJacobsthalLike() => true;
    override public bool IsNegaFibLike() => true;
    override public bool IsPellLike() => true;

    override public int FifthLargestNum() => throw new ArgumentException("List not big enough!");
    override public int MostCommonNum() => throw new ArgumentException("List not big enough!");
    override public int SecondLargestNum() => throw new ArgumentException("List not big enough!");
    override public int ThirdMostCommonNum() => throw new ArgumentException("List not big enough!");
    #endregion

    #region interface helpers
    override public bool WithFunc(Func<int, int, int> fn) => false;

    override public ILoInt CountOccurencesInSorted() => this;
    override public ILoInt CountOccurencesInSortedInner(int prev, int count) => new LinkLoInt(count, this);

    override public (ILoInt front, ILoInt back) TakeInner(int n) => n == 0 ? (new EmptyLoInt(), new EmptyLoInt()) : throw new ArgumentException("List not big enough!");

    override public ILoInt RemoveDuplicatesInSorted() => this;
    override public ILoInt RemoveDuplicatesInSortedInner(int prev) => this;

    override public ILoInt Sorted(Func<int, int, int> compare) => this;
    override public ILoInt InsertInSorted(int x, Func<int, int, int> predicate) => new LinkLoInt(x, this);


    override public T Fold<T>(T initialValue, Func<T, int, T> fn) => initialValue;
    override public int Get(int index) => throw new ArgumentException("out of bounds");
    override public int GetInner(int index) => throw new ArgumentException("out of bounds");
    override public int IndexOf(int value) => -1;
    override public int IndexOfInner(int value, int index) => -1;
    override public int Max() => throw new ArgumentException("Max of empty list!");
    override public int Length() => 0;
    #endregion
}
