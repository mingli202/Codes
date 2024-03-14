// write a function that takes in a target sum and array of numbers
// return array containing the shortest comb of num that adds to targetSum

const bestSum = (targetSum, numbers, memo = {}) => {
  // base case
  if (targetSum === 0) return [];
  if (targetSum < 0) return null;

  // check if already computed
  if (targetSum in memo) return memo[targetSum];

  // to retain the best sum
  let toReturn = null;

  // loop through possible combinasion
  for (let n of numbers) {
    const remainder = targetSum - n;

    const ans = bestSum(remainder, numbers, memo);

    if (ans) {
      const possible = [n, ...ans];

      // check which one is shorter
      if (!toReturn || possible.length < toReturn.length) {
        toReturn = possible;
      }
    }
  }

  memo[targetSum] = toReturn;
  return toReturn;
};

// m = target sum
// n = number.length

// brute force:
// time = O(n^m * m)
// space = O(m^2)

// memoized
// time = O(m^2 * m)
// space = O(m^2)

console.log(bestSum(8, [2, 3, 5]));
console.log(bestSum(7, [5, 3, 4, 7]));
console.log(bestSum(0, [2, 4]));
console.log(bestSum(300, [7, 14]));
console.log(bestSum(834, [34, 12, 412, 41, 13, 9, 6]));
console.log(bestSum(100, [1, 4, 3, 2, 5, 25]));
