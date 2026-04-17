using TesterLib;

namespace TreeFunctionPractice;

public class ExamplesTreeFunctionPractice
{
    /// <summary>
    /// Takes the given ternary tree and returns its mirror.
    /// The mirror of a tree is as if looking at the tree in a mirror.
    /// </summary>
    ///
    /// <example>
    /// 1 => 1
    ///
    ///      1              1
    ///     /|\            /|\
    ///    / | \    =>    / | \
    ///   2  4  3        3  4  2
    ///        /|\      /|\      
    ///       6 8 7    7 8 6      
    /// </example>
    ///
    /// <param name="tree">The tree to mirror.</param>
    /// <returns>The mirror of the given tree.</returns>
    public NumTT Mirror(NumTT tree) => tree switch
    {
        NumLeaf(int val) => new NumLeaf(val),
        NumNode(var val, var left, var mid, var right) => new NumNode(val, Mirror(right), Mirror(mid), Mirror(left)),
        _ => throw new ArgumentException("Invalid tree")
    };

    /// <summary>
    /// Takes a ternary tree and an integer, and returns the count of how many times that integer appears somewhere in the tree.
    ///</summary>
    ///
    /// <example>
    /// CountTarget(    1     , 1) => 4
    ///                /|\    
    ///               / | \   
    ///              2  1  1
    ///                   /|\ 
    ///                  3 2 1
    ///
    /// CountTarget(    1     , 2) => 2
    ///                /|\    
    ///               / | \   
    ///              2  1  1
    ///                   /|\ 
    ///                  3 2 1
    ///
    /// CountTarget(    1     , 5) => 0
    ///                /|\    
    ///               / | \   
    ///              2  1  1
    ///                   /|\ 
    ///                  3 2 1
    /// </example>
    ///
    /// <param name="tree">The tree to count the nodes in.</param>
    /// <param name="target">The value to count the nodes with.</param>
    /// <returns>The number of nodes in the tree that have the given value.</returns>
    public int CountTarget(NumTT tree, int target) => tree switch
    {
        NumLeaf(var val) => val == target ? 1 : 0,
        NumNode(var val, var left, var mid, var right) => (val == target ? 1 : 0) + CountTarget(left, target) + CountTarget(mid, target) + CountTarget(right, target),
        _ => 0
    };

    /// <summary>
    /// Consider a ternary tree, and consider each path from the root to each leaf of the tree. Compute the product of all numbers along a path, and then sum together all the products. 
    /// </summary>
    ///
    /// <example>
    /// SumOfProducts(  1  ) => (1 * 3) + (1 * 2) + (1 * 1) = 6
    ///                /|\ 
    ///               3 2 1
    ///
    /// SumOfProducts(    2     ) => (2 * 2) + (2 * 3) + (2 * 1 * 3) + (2 * 1 * 2) + (2 * 1 * 1) = 22
    ///                  /|\                           + 2 * [(1 * 3) + (1 * 2) + (1 * 1)]       = 22 (recursive)
    ///                 / | \   
    ///                2  3  1
    ///                     /|\ 
    ///                    3 2 1
    /// </example>
    ///
    /// <param name="tree">The ternary tree to sum the products of.</param>
    /// <returns>The sum of the products of all paths in the tree.</returns>
    public int SumOfProducts(NumTT tree) => tree switch
    {
        NumLeaf(var val) => val,
        NumNode(var val, var left, var mid, var right) => val * (SumOfProducts(left) + SumOfProducts(mid) + SumOfProducts(right)),
        _ => throw new ArgumentException("Invalid tree")
    };

    /// <summary>
    /// This function should take a ternary tree and produce the product of the sums along each path.
    /// </summary>
    ///
    /// <example>
    /// ProductOfSums(  1  ) => (1 + 3) * (1 + 2) * (1 + 1) = 24
    ///                /|\ 
    ///               3 2 1
    ///
    /// ProductOfSums(    2     ) => (2 + 2) * (2 + 3) * (2 + 1 + 1) * (2 + 1 + 2) * (2 + 1 + 3) = 2400
    ///                  /|\                             
    ///                 / | \   
    ///                2  3  1
    ///                     /|\ 
    ///                    3 2 1
    ///
    /// ProductOfSums(    2     ) => (2 + 2) * (2 + 3) * (2 + 1 + 1) * (2 + 1 + 2) * (2 + 1 + 3 + 3) * (2 + 1 + 3 + 2) * (2 + 1 + 3 + 1) = 201600
    ///                  /|\                             
    ///                 / | \   
    ///                2  3  1
    ///                     /|\ 
    ///                    3 2 1
    ///                   /|\  
    ///                  3 2 1
    /// </example>
    ///
    /// <param name="tree">The ternary tree to find the product of the sums along each path.</param>
    /// <returns>The product of the sums along each path.</returns>
    public int ProductOfSums(NumTT tree) => ProductOfList(AllPathSums(tree));

    /// <summary>
    /// Returns the product of every element of the given list.
    /// </summary>
    ///
    /// <param name="list">The given list to product across</param>
    /// <returns>The product of every element in the list</returns>
    private int ProductOfList(ListOfInt list) => list switch
    {
        EmptyLoInt => 1,
        LinkLoInt(var first, var rest) => first * ProductOfList(rest),
        _ => throw new NotImplementedException()
    };

    /// <summary>
    /// This function should take a ternary tree and return a list of integers containing those sums, in order as they appear from left to right in the tree
    /// </summary>
    ///
    /// <example>
    /// AllPathSums(  1  ) => [1 + 3, 1 + 2, 1 + 1]
    ///              /|\ 
    ///             3 2 1
    ///
    /// AllPathSums(    2      ) => [2 + 2, 2 + 3, 2 + 1 + 3, 2 + 1 + 2, 2 + 1 + 1]
    ///                /|\                             
    ///               / | \   
    ///              2  3  1
    ///                   /|\ 
    ///                  3 2 1
    ///
    /// AllPathSums(    2      ) => [2 + 2, 2 + 3, 2 + 1 + 3 + 3, 2 + 1 + 3 + 2, 2 + 1 + 3 + 1, 2 + 1 + 2, 2 + 1 + 1]
    ///                /|\                             
    ///               / | \   
    ///              2  3  1
    ///                   /|\ 
    ///                  3 2 1
    ///                 /|\  
    ///                3 2 1
    /// </example>
    ///
    /// <param name="tree">The ternary tree to find the product of the sums along each path.</param>
    /// <returns>The product of the sums along each path.</returns>
    private ListOfInt AllPathSums(NumTT tree) => tree switch
    {
        NumLeaf(var val) => new LinkLoInt(val, new EmptyLoInt()),
        NumNode(var val, var left, var mid, var right) => AddNumToAll(MergeList(AllPathSums(left), MergeList(AllPathSums(mid), AllPathSums(right))), val),
        _ => throw new ArgumentException("Invalid tree")
    };

    private ListOfInt AddNumToAll(ListOfInt list, int num) => list switch
    {
        EmptyLoInt => new EmptyLoInt(),
        LinkLoInt(var first, var rest) => new LinkLoInt(first + num, AddNumToAll(rest, num))
    };

    /// <summary>
    /// This function takes two lists of integers and return a list of integers containing those integers from both lists, in order as they appear from left to right in the lists
    /// </summary>
    ///
    /// <example>
    /// MergeList([1, 2, 3], [4, 5, 6]) => [1, 2, 3, 4, 5, 6]
    /// MergeList([1, 2, 3], []) => [1, 2, 3]
    /// MergeList([], [4, 5, 6]) => [4, 5, 6]
    /// </example>
    ///
    /// <param name="list1">The first list of integers to merge.</param>
    /// <param name="list2">The second list of integers to merge.</param>
    /// <returns>The merged list of integers.</returns>
    private ListOfInt MergeList(ListOfInt list1, ListOfInt list2) => list1 switch
    {
        EmptyLoInt => list2,
        LinkLoInt(var first, var rest) => new LinkLoInt(first, MergeList(rest, list2)),
        _ => throw new ArgumentException("Invalid list")
    };

    // Your tests go here!
    public bool TestMirror1(Tester t)
    {
        var tree = new NumLeaf(1);
        return t.CheckExpect(new NumLeaf(1), Mirror(tree));
    }

    public bool TestMirror2(Tester t)
    {
        var tree = new NumNode(1, new NumLeaf(2), new NumNode(3, new NumLeaf(4), new NumLeaf(5), new NumLeaf(6)), new NumNode(7, new NumLeaf(8), new NumLeaf(9), new NumLeaf(10)));
        var expected = new NumNode(1, new NumNode(7, new NumLeaf(10), new NumLeaf(9), new NumLeaf(8)), new NumNode(3, new NumLeaf(6), new NumLeaf(5), new NumLeaf(4)), new NumLeaf(2));
        return t.CheckExpect(Mirror(tree), expected);
    }

    public bool TestCountTarget1(Tester t)
    {
        var tree = new NumNode(1, new NumLeaf(2), new NumLeaf(1), new NumNode(1, new NumLeaf(3), new NumLeaf(2), new NumLeaf(1)));
        return t.CheckExpect(CountTarget(tree, 1), 4);
    }

    public bool TestCountTarget2(Tester t)
    {
        var tree = new NumNode(1, new NumLeaf(2), new NumLeaf(1), new NumNode(1, new NumLeaf(3), new NumLeaf(2), new NumLeaf(1)));
        return t.CheckExpect(CountTarget(tree, 2), 2);
    }

    public bool TestCountTarget3(Tester t)
    {
        var tree = new NumNode(1, new NumLeaf(2), new NumLeaf(1), new NumNode(1, new NumLeaf(3), new NumLeaf(2), new NumLeaf(1)));
        return t.CheckExpect(CountTarget(tree, 5), 0);
    }

    public bool TestSumOfProducts1(Tester t)
    {
        var tree = new NumNode(1, new NumLeaf(3), new NumLeaf(2), new NumLeaf(1));
        return t.CheckExpect(SumOfProducts(tree), 6);
    }

    public bool TestSumOfProducts2(Tester t)
    {
        var tree = new NumNode(2, new NumLeaf(2), new NumLeaf(3), new NumNode(1, new NumLeaf(3), new NumLeaf(2), new NumLeaf(1)));
        return t.CheckExpect(SumOfProducts(tree), 22);
    }

    public bool TestProductOfSums1(Tester t)
    {
        var tree = new NumNode(1, new NumLeaf(3), new NumLeaf(2), new NumLeaf(1));
        return t.CheckExpect(ProductOfSums(tree), 24);
    }

    public bool TestProductOfSums2(Tester t)
    {
        var tree = new NumNode(2, new NumLeaf(2), new NumLeaf(3), new NumNode(1, new NumLeaf(3), new NumLeaf(2), new NumLeaf(1)));
        return t.CheckExpect(ProductOfSums(tree), 2400);
    }


    public bool TestProductOfSums3(Tester t)
    {
        var tree = new NumNode(2, new NumLeaf(2), new NumLeaf(3), new NumNode(1, new NumNode(3, new NumLeaf(3), new NumLeaf(2), new NumLeaf(1)), new NumLeaf(2), new NumLeaf(1)));
        return t.CheckExpect(ProductOfSums(tree), 201600);
    }

    public bool TestMergeList1(Tester t)
    {
        var list1 = new LinkLoInt(1, new LinkLoInt(2, new LinkLoInt(3, new EmptyLoInt())));
        var list2 = new LinkLoInt(4, new LinkLoInt(5, new LinkLoInt(6, new EmptyLoInt())));
        return t.CheckExpect(MergeList(list1, list2), new LinkLoInt(1, new LinkLoInt(2, new LinkLoInt(3, new LinkLoInt(4, new LinkLoInt(5, new LinkLoInt(6, new EmptyLoInt())))))));
    }

    public bool TestMergeList2(Tester t)
    {
        var list1 = new LinkLoInt(1, new LinkLoInt(2, new LinkLoInt(3, new EmptyLoInt())));
        var list2 = new EmptyLoInt();
        return t.CheckExpect(MergeList(list1, list2), new LinkLoInt(1, new LinkLoInt(2, new LinkLoInt(3, new EmptyLoInt()))));
    }

    public bool TestMergeList3(Tester t)
    {
        var list1 = new EmptyLoInt();
        var list2 = new LinkLoInt(4, new LinkLoInt(5, new LinkLoInt(6, new EmptyLoInt())));
        return t.CheckExpect(MergeList(list1, list2), new LinkLoInt(4, new LinkLoInt(5, new LinkLoInt(6, new EmptyLoInt()))));
    }

    public bool TestAllPathSums1(Tester t)
    {
        var tree = new NumNode(1, new NumLeaf(3), new NumLeaf(2), new NumLeaf(1));
        return t.CheckExpect(AllPathSums(tree), new LinkLoInt(1 + 3, new LinkLoInt(1 + 2, new LinkLoInt(1 + 1, new EmptyLoInt()))));
    }

    public bool TestAllPathSums2(Tester t)
    {
        var tree = new NumNode(2, new NumLeaf(2), new NumLeaf(3), new NumNode(1, new NumLeaf(3), new NumLeaf(2), new NumLeaf(1)));
        return t.CheckExpect(AllPathSums(tree), new LinkLoInt(2 + 2, new LinkLoInt(2 + 3, new LinkLoInt(2 + 1 + 3, new LinkLoInt(2 + 1 + 2, new LinkLoInt(2 + 1 + 1, new EmptyLoInt()))))));
    }

    public bool TestAllPathSums3(Tester t)
    {
        var tree = new NumNode(2, new NumLeaf(2), new NumLeaf(3), new NumNode(1, new NumNode(3, new NumLeaf(3), new NumLeaf(2), new NumLeaf(1)), new NumLeaf(2), new NumLeaf(1)));
        return t.CheckExpect(AllPathSums(tree), new LinkLoInt(2 + 2, new LinkLoInt(2 + 3, new LinkLoInt(2 + 1 + 3 + 3, new LinkLoInt(2 + 1 + 3 + 2, new LinkLoInt(2 + 1 + 3 + 1, new LinkLoInt(2 + 1 + 2, new LinkLoInt(2 + 1 + 1, new EmptyLoInt()))))))));
    }
}

