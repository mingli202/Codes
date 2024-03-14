# Memoization recipe

1. Make it work: have a recusive function that works

   - Visualize problem as a tree
   - implement the tree using recursion: base case are the ending branch (brute force recusion)
   - test it

2. Make it efficient: optimized the function using memoization

   - add a memo object: make sure object is passed
   - add a new base case to return memo values
   - store return values into the memo

# Tabulation Recipe

1. Visualize the problem as a table
2. size the table based on input
3. initialize the table with default values compatible with return type
4. see the trivial answer (small subproblem that you know the answer) into the table
5. iterate through the table
6. fill further positions based on the curent position

# Conclusion

1. notice any overlapping subproblems
2. decide what is the trivially smallest input
3. think recursively to use memoization
4. think iteratively to use tabulation
5. draw a strategy first (recognize edge cases)
