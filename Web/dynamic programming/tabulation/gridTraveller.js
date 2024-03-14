// you are a traveller on a 2D grid
// you begin on the top left corner and end at the bottom right corner
// you can only move right and down each time
// how many ways can you move given a mxn grid

// function assumes that n, m > 0
// if existance of size 0 grid then replace base case by zero

// in tabulation, the initial table have roughly the same size as the input
// so make a m+1 x n+1 2D array

const gridTraveller = (m, n) => {
  // create and seed the table
  const table = Array(m + 1)
    .fill()
    .map(() => Array(n + 1).fill(0));

  // trivial answer
  table[1][1] = 1;

  // tabulate
  for (let i = 0; i <= m; i++) {
    for (let k = 0; k <= n; k++) {
      const current = table[i][k];
      if (k + 1 <= n) table[i][k + 1] += current;
      if (i + 1 <= m) table[i + 1][k] += current;
    }
  }

  // return
  return table[m][n];
};

// time: O(m*n)
// space: O(m*n)

console.log(gridTraveller(1, 1));
console.log(gridTraveller(2, 3));
console.log(gridTraveller(3, 2));
console.log(gridTraveller(3, 3));
console.log(gridTraveller(18, 18));
