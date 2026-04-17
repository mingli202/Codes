using TesterLib;
using System.Windows.Navigation;             // The tester library

namespace NumListPractice;

class ExamplesNumList
{
    /// <summary>
    /// Takes two list of integers and produces a single list of their items, alternating from each list, beginning with the first. If the lists have different lengths, just finish with all the remaining items of the longer list
    /// </summary>
    ///
    /// <example>
    ///     <code>
    ///         var l1 = new LinkLoInt(1, new LinkLoInt(2, new EmptyLoInt()));
    ///         var l2 = new LinkLoInt(3, new LinkLoInt(4, new EmptyLoInt()));
    ///         var result = InterLeave(l1, l2);
    ///         // result is LinkLoInt(1, LinkLoInt(3, LinkLoInt(2, LinkLoInt(4, EmptyLoInt()))))
    ///     </code>
    ///     <code>
    ///         var l1 = new LinkLoInt(1, new EmptyLoInt());
    ///         var l2 = new LinkLoInt(3, new LinkLoInt(4, new EmptyLoInt()));
    ///         var result = InterLeave(l1, l2);
    ///         // result is LinkLoInt(1, LinkLoInt(3, LinkLoInt(4, EmptyLoInt())))
    ///     </code>
    /// </example>
    ///
    /// <param name="l1">The first list</param>
    /// <param name="l2">The second list</param>
    /// <returns>A list of the items from the two lists, alternating from each list, beginning with the first</returns>
    ListOfInt Interleave(ListOfInt l1, ListOfInt l2) => l1 switch
    {
        EmptyLoInt => l2,
        LinkLoInt(var first, var rest) => new LinkLoInt(first, Interleave(l2, rest)),
        _ => throw new NotImplementedException()
    };

    /// <summary>
    /// Returns the prime factors of a positive integer. The prime factors are the prime numbers that divide the positive integer.
    /// </summary>
    /// <param name="positiveInteger">The positive integer</param>
    /// <returns>The prime factors of a positive integer</returns>
    ListOfInt PrimeFactors(int positiveInteger) => PrimeFactorsWithInitialPrime(positiveInteger, 2);

    /// <summary>
    /// Returns the prime factors of a positive integer with an initial prime number. The prime factors are the prime numbers that divide the positive integer.
    /// </summary>
    /// <param name="positiveInteger">The positive integer</param>
    /// <param name="initialPrime">The initial prime number</param>
    /// <returns>The prime factors of a positive integer</returns>
    ListOfInt PrimeFactorsWithInitialPrime(int positiveInteger, int initialPrime)
    {
        if (positiveInteger == 1)
        {
            return new EmptyLoInt();
        }

        int nextPrime = DetectNextPrime(positiveInteger, initialPrime);

        return new LinkLoInt(nextPrime, PrimeFactorsWithInitialPrime(positiveInteger / nextPrime, nextPrime));
    }

    int DetectNextPrime(int positiveInteger, int factor)
    {
        if (positiveInteger == factor || factor > Math.Sqrt(positiveInteger)) return positiveInteger;
        if (positiveInteger % factor == 0)
        {
            return factor;
        }
        return DetectNextPrime(positiveInteger, factor + 1);
    }

    ListOfInt Intersection(ListOfListOfInt l1) => l1 switch
    {
        EmptyLoLoInt => new EmptyLoInt(),
        LinkLoLoInt(ListOfInt first, ListOfListOfInt rest) => GetIntersectionBetweenList(first, rest switch
        {
            EmptyLoLoInt => first,
            _ => Intersection(rest)
        })
    };

    ListOfInt GetIntersectionBetweenList(ListOfInt l1, ListOfInt l2) => l1 switch
    {
        EmptyLoInt => new EmptyLoInt(),
        LinkLoInt(int first, ListOfInt rest) => Contains(l2, first) ? new LinkLoInt(first, GetIntersectionBetweenList(rest, l2)) : GetIntersectionBetweenList(rest, l2)
    };

    bool Contains(ListOfInt l1, int nbr) => l1 switch
    {
        EmptyLoInt => false,
        LinkLoInt(int first, ListOfInt rest) => first == nbr || Contains(rest, nbr)
    };

    /// <summary>
    /// Checks if all elements of ll1 are contained in somewhere in ll2
    /// </summary>
    /// <param name="ll1">The first list</param>
    /// <param name="ll2">The second list</param>
    /// <returns>True if all elements of ll1 are contained in ll2</returns>
    bool AllContainedIn(ListOfListOfInt ll1, ListOfListOfInt ll2) => ll1 switch
    {
        EmptyLoLoInt => true,
        LinkLoLoInt(ListOfInt first, ListOfListOfInt rest) => ContainsList(ll2, first) && AllContainedIn(rest, ll2)
    };

    bool ContainsList(ListOfListOfInt ll1, ListOfInt l1) => ll1 switch
    {
        EmptyLoLoInt => false,
        LinkLoLoInt(ListOfInt first, ListOfListOfInt rest) => first == l1 || ContainsList(rest, l1)
    };

    /// <summary>
    /// Returns all possible sublists of a list of integers. A sublist of a list is any list whose elements are all present in the initial list and appear in the same relative order. Assume the initial list given to Powerlist contains no duplicates.
    /// </summary>
    /// <param name="l1">The list</param>
    /// <returns>All possible sublists of a list of integers</returns>
    ListOfListOfInt Powerlist(ListOfInt l1) => new LinkLoLoInt(new EmptyLoInt(), MakePowerlist(l1));

    ListOfListOfInt MakePowerlist(ListOfInt l1) => l1 switch
    {
        EmptyLoInt => new EmptyLoLoInt(),
        LinkLoInt(var first, var rest) => new LinkLoLoInt(new LinkLoInt(first, new EmptyLoInt()), InsertAndIncludeNumberToAllLists(MakePowerlist(rest), first))
    };

    ListOfListOfInt InsertAndIncludeNumberToAllLists(ListOfListOfInt l1, int number) => l1 switch
    {
        EmptyLoLoInt => new EmptyLoLoInt(),
        LinkLoLoInt(ListOfInt first, ListOfListOfInt rest) => new LinkLoLoInt(first, new LinkLoLoInt(new LinkLoInt(number, first), InsertAndIncludeNumberToAllLists(rest, number)))
    };


    ListOfInt BuildList(int[] arr, int index = 0)
    {
        if (index == arr.Length)
            return new EmptyLoInt();

        return new LinkLoInt(arr[index], BuildList(arr, index + 1));
    }

    ListOfListOfInt BuildListOfList(int[][] arr, int index = 0)
    {
        if (index == arr.Length)
            return new EmptyLoLoInt();

        return new LinkLoLoInt(BuildList(arr[index], 0), BuildListOfList(arr, index + 1));
    }

    string PrettyPrintLL(ListOfListOfInt l1) => "[\n" + _PrettyPrintLL(l1) + "]";

    string _PrettyPrintLL(ListOfListOfInt l1) => l1 switch
    {
        EmptyLoLoInt => "",
        LinkLoLoInt(ListOfInt first, ListOfListOfInt rest) => $"{PrettyPrintL(first)}\n{_PrettyPrintLL(rest)}"
    };

    string PrettyPrintL(ListOfInt l1) => "[" + _PrettyPrintL(l1) + "]";

    string _PrettyPrintL(ListOfInt l1) => l1 switch
    {
        EmptyLoInt => "",
        LinkLoInt(var first, var rest) => $"{first}, {_PrettyPrintL(rest)}"
    };

    bool TestInterLeave1(Tester t)
    {
        var l1 = new LinkLoInt(1, new LinkLoInt(2, new EmptyLoInt()));
        var l2 = new LinkLoInt(3, new LinkLoInt(4, new EmptyLoInt()));
        var result = Interleave(l1, l2);
        return t.CheckExpect(result, new LinkLoInt(1, new LinkLoInt(3, new LinkLoInt(2, new LinkLoInt(4, new EmptyLoInt())))));
    }

    bool TestInterLeave2(Tester t)
    {
        var l1 = new LinkLoInt(1, new EmptyLoInt());
        var l2 = new LinkLoInt(3, new LinkLoInt(4, new EmptyLoInt()));
        var result = Interleave(l1, l2);
        return t.CheckExpect(result, new LinkLoInt(1, new LinkLoInt(3, new LinkLoInt(4, new EmptyLoInt()))));
    }

    bool TestInterLeave3(Tester t)
    {
        var l1 = new LinkLoInt(3, new LinkLoInt(4, new EmptyLoInt()));
        var l2 = new LinkLoInt(1, new EmptyLoInt());
        var result = Interleave(l1, l2);
        return t.CheckExpect(result, new LinkLoInt(3, new LinkLoInt(1, new LinkLoInt(4, new EmptyLoInt()))));
    }

    bool TestInterLeave4(Tester t)
    {
        var l1 = new EmptyLoInt();
        var l2 = new EmptyLoInt();
        var result = Interleave(l1, l2);
        return t.CheckExpect(result, new EmptyLoInt());
    }

    bool TestPrimeFactors1(Tester t)
    {
        var list = PrimeFactors(18);
        return t.CheckExpect(list, new LinkLoInt(2, new LinkLoInt(3, new LinkLoInt(3, new EmptyLoInt()))));
    }

    bool TestPrimeFactors2(Tester t)
    {
        var list = PrimeFactors(2);
        return t.CheckExpect(list, new LinkLoInt(2, new EmptyLoInt()));
    }

    bool TestPrimeFactors3(Tester t)
    {
        var list = PrimeFactors(49);
        return t.CheckExpect(list, new LinkLoInt(7, new LinkLoInt(7, new EmptyLoInt())));
    }

    bool TestPrimeFactors4(Tester t)
    {
        var list = PrimeFactors(68);
        return t.CheckExpect(list, new LinkLoInt(2, new LinkLoInt(2, new LinkLoInt(17, new EmptyLoInt()))));
    }


    bool TestGetIntersectionBetweenList(Tester t)
    {
        var l1 = new int[]
        {
            1, 2, 3, 4
        };
        var l2 = new int[]
        {
            2, 3
        };
        var expected = new int[]
        {
            2, 3
        };
        var list = GetIntersectionBetweenList(BuildList(l1), BuildList(l2));
        return t.CheckExpect(list, BuildList(expected));
    }

    bool TestContained(Tester t)
    {
        var l1 = new int[] { 1, 2, 3, 4 };
        return t.CheckExpect(Contains(BuildList(l1), 2), true);
    }

    bool TestIntersection1(Tester t)
    {
        int[][] l1 = new int[][] {
            new int[] {4, 2, 3},
            new int[] {2, 8, 3},
            new int[] {3, 2, 0},
        };
        var expected = new int[]
        {
            2, 3
        };
        var list = Intersection(BuildListOfList(l1));
        return t.CheckExpect(list, BuildList(expected));
    }

    bool TestIntersection2(Tester t)
    {
        int[][] l1 = new int[][] {
            new int[] {4, 2, 3},
            new int[] {},
            new int[] {3, 2, 0},
        };
        var expected = new int[] { };
        var list = Intersection(BuildListOfList(l1));
        return t.CheckExpect(list, BuildList(expected));
    }

    bool TestIntersection3(Tester t)
    {
        int[][] l1 = new int[][] { };
        var expected = new int[] { };
        var list = Intersection(BuildListOfList(l1));
        return t.CheckExpect(list, BuildList(expected));
    }

    bool TestIntersection4(Tester t)
    {
        int[][] l1 = new int[][] {
            new int[] {4, 2, 3},
            new int[] {5, 6, 7},
            new int[] {8, 9, 0},
        };
        var expected = new int[] { };
        var list = Intersection(BuildListOfList(l1));
        return t.CheckExpect(list, BuildList(expected));
    }

    bool TestAllContainedIn1(Tester t)
    {
        int[][] l1 = new int[][] {
            new int[] {4, 2, 3},
            new int[] {2, 8, 3},
            new int[] {3, 2, 0},
        };
        int[][] l2 = new int[][] {
            new int[] {3, 2, 0},
            new int[] {2, 8, 3},
            new int[] {4, 2, 3},
        };
        return t.CheckExpect(AllContainedIn(BuildListOfList(l1), BuildListOfList(l2)), true);
    }

    bool TestAllContainedIn2(Tester t)
    {
        int[][] l1 = new int[][] {
            new int[] {4, 2, 3},
            new int[] {2, 8, 3},
            new int[] {3, 2, 0},
        };
        int[][] l2 = new int[][] {
            new int[] {4, 2, 3},
            new int[] {2, 8, 3},
            new int[] {3, 2, 0},
            new int[] {5, 6, 7},
        };
        return t.CheckExpect(AllContainedIn(BuildListOfList(l1), BuildListOfList(l2)), true);
    }

    bool TestAllContainedIn3(Tester t)
    {
        int[][] l1 = new int[][] {
            new int[] {4, 2, 3},
            new int[] {2, 8, 3},
            new int[] {3, 2, 0},
        };
        int[][] l2 = new int[][] {
            new int[] {4, 2, 3},
            new int[] {2, 8, 3},
        };
        return t.CheckExpect(AllContainedIn(BuildListOfList(l1), BuildListOfList(l2)), false);
    }

    bool TestAllContainedIn4(Tester t)
    {
        int[][] l1 = new int[][] {
            new int[] {4, 2, 3},
            new int[] {2, 8, 3},
            new int[] {3, 2, 0},
        };
        int[][] l2 = new int[][] { };
        return t.CheckExpect(AllContainedIn(BuildListOfList(l1), BuildListOfList(l2)), false);
    }

    bool TestAllContainedIn5(Tester t)
    {
        int[][] l1 = new int[][] { };
        int[][] l2 = new int[][] {
            new int[] {4, 2, 3},
            new int[] {2, 8, 3},
            new int[] {3, 2, 0},
        };
        return t.CheckExpect(AllContainedIn(BuildListOfList(l1), BuildListOfList(l2)), true);
    }

    bool TestAllContainedIn6(Tester t)
    {
        int[][] l1 = new int[][] {
            new int[] {4, 2, 3},
            new int[] {2, 8, 3},
            new int[] {3, 2, 0},
        };
        int[][] l2 = new int[][] {
            new int[] {2, 4, 3},
            new int[] {2, 8, 3},
            new int[] {3, 2, 0},
        };
        return t.CheckExpect(AllContainedIn(BuildListOfList(l1), BuildListOfList(l2)), false);
    }

    bool TestPowerList1(Tester t)
    {
        int[] l1 = new int[] { 1, 2, 3 };
        int[][] expected = new int[][] {
            new int[] {},
            new int[] {1},
            new int[] {2},
            new int[] {3},
            new int[] {1, 2},
            new int[] {1, 3},
            new int[] {2, 3},
            new int[] {1, 2, 3},
        };

        var list = Powerlist(BuildList(l1));
        return t.CheckExpect(AllContainedIn(list, BuildListOfList(expected)), true);
    }

    bool TestInsertNumberToAllLists(Tester t)
    {
        int[][] l1 = new int[][] {
            new int[] {4, 2, 3},
            new int[] {2, 8, 3},
            new int[] {3, 2, 0},
        };
        var expected = new int[][] {
            new int[] {4, 2, 3},
            new int[] {1, 4, 2, 3},
            new int[] {2, 8, 3},
            new int[] {1, 2, 8, 3},
            new int[] {3, 2, 0},
            new int[] {1, 3, 2, 0},
        };
        var list = InsertAndIncludeNumberToAllLists(BuildListOfList(l1), 1);
        return t.CheckExpect(list, BuildListOfList(expected)) && t.CheckExpect(BuildListOfList(expected), list);
    }
}

