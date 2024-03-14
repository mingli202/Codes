// return n th number from fibonacci sequence
// use tabulation: intead of solving recursively, we build a table
// fib(6) -> 8

const fib = (n) => {
  // create table and seed it
  const table = Array(n + 1).fill(0);

  // set trivial answer
  table[1] = 1;

  // tabulate
  for (let i = 0; i <= n; i++) {
    table[i + 1] += table[i];
    table[i + 2] += table[i];
  }

  // return
  return table[n];
};

// time: O(n)
// space: O(n)

console.log(fib(6)); // 8
console.log(fib(7)); // 13
console.log(fib(8)); // 21
console.log(fib(50)); // 12586269025
