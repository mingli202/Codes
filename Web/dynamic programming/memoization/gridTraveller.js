// you are a traveller on a 2D grid
// you begin on the top left corner and end at the bottom right corner
// you can only move right and down each time
// how many ways can you move given a mxn grid

// function assumes that n, m > 0
// if existance of size 0 grid then replace base case by zero
const gridTraveller = (n, m, memo = {}) => {
  // there is only way to travel if either is 1
  if (n === 1 || m === 1) return 1;

  // check if it has already been computed
  const key = n + "," + m;
  if (key in memo) return memo[key];

  // store computed value
  memo[key] = gridTraveller(n - 1, m, memo) + gridTraveller(n, m - 1, memo);

  // return value
  return memo[key];
};

console.log(gridTraveller(21, 21));
