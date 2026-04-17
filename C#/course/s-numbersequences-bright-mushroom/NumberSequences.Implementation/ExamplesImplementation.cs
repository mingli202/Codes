using TesterLib;

namespace NumberSequences;

#pragma warning disable IDE0051
class ExamplesImplementation
{
    bool TestCountOccurences1(Tester t)
    {
        var list = BuildList([1, 2, 3, 4, 5]);
        return t.CheckExpect(list.CountOccurencesInSorted(), BuildList([1, 1, 1, 1, 1]));
    }

    bool TestCountOccurences2(Tester t)
    {
        var list = BuildList([1, 1, 1, 1, 1, 2, 2, 3, 4, 5, 5, 5]);
        return t.CheckExpect(list.CountOccurencesInSorted(), BuildList([5, 2, 1, 1, 3]));
    }

    bool TestSorted1(Tester t)
    {
        var list = BuildList([5, 1, 4, 2, 3]);
        return t.CheckExpect(list.Sorted((a, b) => a - b), BuildList([1, 2, 3, 4, 5]));
    }

    bool TestSorted2(Tester t)
    {
        var list = BuildList([5, 1, 4, 2, 3]);
        return t.CheckExpect(list.Sorted((a, b) => b - a), BuildList([5, 4, 3, 2, 1]));
    }

    bool TestSorted3(Tester t)
    {
        var list = BuildList([]);
        return t.CheckExpect(list.Sorted((a, b) => b - a), BuildList([]));
    }

    bool TestSorted4(Tester t)
    {
        var list = BuildList([1, 2, 4, 5, 3, 2, 4]);
        return t.CheckExpect(list.Sorted((a, b) => a - b), BuildList([1, 2, 2, 3, 4, 4, 5]));
    }

    bool TestRemoveDuplicatesInSorted1(Tester t)
    {
        var list = BuildList([1, 2, 3, 4, 5, 5]);
        return t.CheckExpect(list.RemoveDuplicatesInSorted(), BuildList([1, 2, 3, 4, 5]));
    }

    bool TestRemoveDuplicatesInSorted2(Tester t)
    {
        var list = BuildList([1, 1, 1, 2, 3, 3, 3, 3, 4, 5, 5]);
        return t.CheckExpect(list.RemoveDuplicatesInSorted(), BuildList([1, 2, 3, 4, 5]));
    }

    bool TestRemoveDuplicatesInSorted3(Tester t)
    {
        var list = BuildList([]);
        return t.CheckExpect(list.RemoveDuplicatesInSorted(), BuildList([]));
    }

    bool TestTake1(Tester t)
    {
        var list = BuildList([1, 2, 3, 4, 5]);
        var (front, back) = list.Take(0);

        return t.CheckExpect(front, BuildList([])) && t.CheckExpect(back, BuildList([1, 2, 3, 4, 5]));
    }
    bool TestTake2(Tester t)
    {
        var list = BuildList([1, 2, 3, 4, 5]);
        return t.CheckException(new ArgumentException("Can't take by less than 0!"), list.Take, -1);
    }

    bool TestTake3(Tester t)
    {
        var list = BuildList([1, 2, 3, 4, 5]);
        var (front, back) = list.Take(3);

        // 1 -> 2 -> 3 -> 4 -> 5 -> null
        //                |
        // 1 -> 2 -> 3 -> null    4 -> 5 -> null
        //
        // n = 0

        return t.CheckExpect(front, BuildList([1, 2, 3])) && t.CheckExpect(back, BuildList([4, 5]));
    }

    bool TestTake4(Tester t)
    {
        var list = BuildList([1, 2, 3, 4, 5]);
        var (front, back) = list.Take(5);

        return t.CheckExpect(front, BuildList([1, 2, 3, 4, 5])) && t.CheckExpect(back, BuildList([]));
    }

    bool TestTake5(Tester t)
    {
        var list = BuildList([1, 2, 3, 4, 5]);
        return t.CheckException(new ArgumentException("List not big enough!"), list.Take, 6);
    }

    bool TestTake6(Tester t)
    {
        var list = BuildList([]);
        return t.CheckException(new ArgumentException("List not big enough!"), list.Take, 2);
    }

    bool TestTake7(Tester t)
    {
        var list = BuildList([1]);
        return t.CheckException(new ArgumentException("List not big enough!"), list.Take, 2);
    }

    bool TestTake8(Tester t)
    {
        var list = BuildList([]);
        var (front, back) = list.Take(0);

        return t.CheckExpect(front, BuildList([])) && t.CheckExpect(back, BuildList([]));
    }

    bool TestFold1(Tester t)
    {
        var list = BuildList([1, 2, 3, 4, 5]);
        return t.CheckExpect(list.Fold(0, (acc, x) => acc + x), 15);
    }

    bool TestFold2(Tester t)
    {
        var list = BuildList([]);
        return t.CheckExpect(list.Fold(0, (acc, x) => acc + x), 0);
    }

    bool TestFold3(Tester t)
    {
        var list = BuildList([1, 2, 3, 4, 5]);
        ILoInt defaultVal = new EmptyLoInt();
        return t.CheckExpect(list.Fold(defaultVal, (acc, x) => new LinkLoInt(x, acc)), BuildList([5, 4, 3, 2, 1]));
    }

    bool TestGet1(Tester t)
    {
        var list = BuildList([1, 2, 3, 4, 5]);
        return t.CheckExpect(list.Get(0), 1);
    }
    bool TestGet2(Tester t)
    {
        var list = BuildList([1, 2, 3, 4, 5]);
        return t.CheckExpect(list.Get(2), 3);
    }
    bool TestGet3(Tester t)
    {
        var list = BuildList([1, 2, 3, 4, 5]);
        return t.CheckException(new ArgumentException("out of bounds"), list.Get, 5);
    }
    bool TestGet4(Tester t)
    {
        var list = BuildList([1, 2, 3, 4, 5]);
        return t.CheckException(new ArgumentException("Can't get negative index!"), list.Get, -1);
    }

    bool TestGet5(Tester t)
    {
        var list = BuildList([]);
        return t.CheckException(new ArgumentException("out of bounds"), list.Get, -1);
    }

    bool TestIndexOf1(Tester t)
    {
        var list = BuildList([1, 2, 3, 4, 5]);
        return t.CheckExpect(list.IndexOf(3), 2);
    }
    bool TestIndexOf2(Tester t)
    {
        var list = BuildList([1, 2, 3, 4, 5]);
        return t.CheckExpect(list.IndexOf(6), -1);
    }

    bool TestIndexOf3(Tester t)
    {
        var list = BuildList([]);
        return t.CheckExpect(list.IndexOf(1), -1);
    }

    bool TestMax1(Tester t)
    {
        var list = BuildList([5, 10, 3, 1, -5]);
        return t.CheckExpect(list.Max(), 10);
    }
    bool TestMax2(Tester t)
    {
        var list = BuildList([]);
        return t.CheckException(new ArgumentException("Max of empty list!"), list.Max);
    }

    private ILoInt BuildList(int[] arr) => BuildListInner(arr, 0);
    private ILoInt BuildListInner(int[] arr, int index) => index == arr.Length ? new EmptyLoInt() : new LinkLoInt(arr[index], BuildListInner(arr, index + 1));
}

