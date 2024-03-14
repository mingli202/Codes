// write a function 'fib(n)' that takes in number as its argument.
// the function should return the n-th number of the fibonacci sequence.

// function is too slow -> memoization
// use object; keys is args to function, val is the return value

const fib = (n, memo = {}) => {
  // check if n has alreaddy been calculated before
  if (n in memo) return memo[n];

  // check if it's the base case
  if (n <= 2) return 1;

  // store the value to access it later
  memo[n] = fib(n - 1, memo) + fib(n - 2, memo);

  // return the value that you just stored
  return memo[n];
};

console.log(fib(6));
console.log(fib(7));
console.log(fib(8));
console.log(fib(50));
