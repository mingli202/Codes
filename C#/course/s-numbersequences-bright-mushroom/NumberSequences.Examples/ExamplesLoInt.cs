using TesterLib;

namespace NumberSequences;

#pragma warning disable IDE0051, CA1822
class ExamplesLoInt
{
    // Your tests go here!
    bool TestIsFibLike2(Tester t)
        => t.CheckExpect(BuildList([-3, -2, -5, -7, -12]).IsFibLike(), true);
    bool TestIsFibLike3(Tester t)
        => t.CheckExpect(BuildList([1, 1, 2, 3, 5, 8, 14]).IsFibLike(), false);
    bool TestIsFibLike7(Tester t)
        => t.CheckExpect(BuildList([1, 2]).IsFibLike(), true);
    bool TestIsFibLike8(Tester t)
        => t.CheckExpect(BuildList([0, 0, 0, 0, 0]).IsFibLike(), true);

    bool TestIsPellLike3(Tester t) => t.CheckExpect(BuildList([1, 2, 6]).IsPellLike(), false);
    bool TestIsPellLike4(Tester t) => t.CheckExpect(BuildList([-1, -2, -5, -12, -29]).IsPellLike(), true);
    bool TestIsPellLike7(Tester t)
        => t.CheckExpect(BuildList([1, 2]).IsPellLike(), true);

    bool TestIsNegaFibLike2(Tester t) => t.CheckExpect(BuildList([2, 3, -1, 4, -5, 9]).IsNegaFibLike(), true);
    bool TestIsNegaFibLike3(Tester t) => t.CheckExpect(BuildList([-1, 1, -2, 3, -5, 7]).IsNegaFibLike(), false);
    bool TestIsNegaFibLike6(Tester t)
        => t.CheckExpect(BuildList([1, 2]).IsNegaFibLike(), true);

    bool TestIsJacobsthalLike2(Tester t) => t.CheckExpect(BuildList([-5, 10, 0, 20, 20, 60, 100]).IsJacobsthalLike(), true);
    bool TestIsJacobsthalLike3(Tester t) => t.CheckExpect(BuildList([0, 1, 1, 3, 5, 11, 21, 43, 86]).IsJacobsthalLike(), false);
    bool TestIsJacobsthalLike6(Tester t)
        => t.CheckExpect(BuildList([1, 2]).IsJacobsthalLike(), true);

    bool TestSecondLargestNum1(Tester t) => t.CheckExpect(BuildList([-4, -100, 4, 6, 8, 3, 2, 8, 1]).SecondLargestNum(), 8);
    bool TestSecondLargestNum2(Tester t) => t.CheckExpect(BuildList([9, 2]).SecondLargestNum(), 2);
    bool TestSecondLargestNum4(Tester t) => CheckThrow(t, BuildList([]).SecondLargestNum);

    bool TestFifthLargestNum1(Tester t) => t.CheckExpect(BuildList([3, 4, 1, 5, 6, 7, 8, 9, 10, 2, -10]).FifthLargestNum(), 6);
    bool TestFifthLargestNum2(Tester t) => t.CheckExpect(BuildList([1, 10, 4, 5, 8, 6, 7, 8, 9, 2, 10, 3]).FifthLargestNum(), 8);
    bool TestFifthLargestNum4(Tester t) => CheckThrow(t, BuildList([]).FifthLargestNum);

    bool TestMostCommonNum1(Tester t)
    {
        var mostCommon = BuildList([5, 2, 1, 4, 3]).MostCommonNum();
        var answer = new HashSet<int>([5, 2, 1, 4, 3]);
        return t.CheckExpect(answer.Contains(mostCommon), true);
    }
    bool TestMostCommonNum2(Tester t) => t.CheckExpect(BuildList([4, 2, 3, 5, 2]).MostCommonNum(), 2);
    bool TestMostCommonNum3(Tester t)
    {
        var mostCommon = BuildList([4, 3, 2, 2, 5, 3, -4, 5, 7, 0, 1, 4]).MostCommonNum();
        var answer = new HashSet<int>([4, 2, 3, 5]);
        return t.CheckExpect(answer.Contains(mostCommon), true);
    }
    bool TestMostCommonNum5(Tester t) => CheckThrow(t, BuildList([]).MostCommonNum);

    bool TestThirdMostCommonNum1(Tester t)
    {
        var thirdMostCommon = BuildList([1, 2, 3, 4, 5, 6, 7, 8, 9, 10]).ThirdMostCommonNum();
        var answer = new HashSet<int>([1, 2, 3, 4, 5, 6, 7, 8, 9, 10]);
        return t.CheckExpect(answer.Contains(thirdMostCommon), true);
    }
    bool TestThirdMostCommonNum2(Tester t)
    {
        var thirdMostCommon = BuildList([1, 2, 1, 1, 2, 3]).ThirdMostCommonNum();
        var answer = new HashSet<int>([3]);
        return t.CheckExpect(answer.Contains(thirdMostCommon), true);
    }
    bool TestThirdMostCommonNum3(Tester t) => CheckThrow(t, BuildList([]).ThirdMostCommonNum);
    bool TestThirdMostCommonNum4(Tester t) => CheckThrow(t, BuildList([1]).ThirdMostCommonNum);
    bool TestThirdMostCommonNum5(Tester t) => CheckThrow(t, BuildList([1, 2]).ThirdMostCommonNum);

    bool TestThirdMostCommonNum6(Tester t) => CheckThrow(t, BuildList([5, 7, 7, 5, 5, 5, 7]).ThirdMostCommonNum);

    private bool CheckThrow<T>(Tester t, Func<T> fn)
    {
        try
        {
            fn();
            return t.CheckExpect(false, true, "Expected this method to throw an exception!");
        }
        catch
        {
            return t.CheckExpect(true, true);
        }
    }

    private ILoInt BuildList(int[] arr) => BuildListInner(arr, 0);
    private ILoInt BuildListInner(int[] arr, int index) => index == arr.Length ? new EmptyLoInt() : new LinkLoInt(arr[index], BuildListInner(arr, index + 1));
}

