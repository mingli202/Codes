// write a function that takes targetSum and array of number
// return arr of combinasion that adds up to targetSum
// if non then return null

const howSum = (targetSum, numbers, memo = {}) => {
  // base case
  if (targetSum === 0) return [];
  if (targetSum < 0) return null;

  // check if already computed
  if (targetSum in memo) return memo[targetSum];

  // loop through possible remainders
  for (let num of numbers) {
    const ans = howSum(targetSum - num, numbers, memo);

    if (ans !== null) {
      memo[targetSum] = ans.includes(num) ? ans : [num, ...ans];
      return memo[targetSum];
    }
  }

  // return value if no possible comb
  memo[targetSum] = null;
  return null;
};

console.log(howSum(8, [2, 3, 5]));
console.log(howSum(7, [5, 3, 4, 7]));
console.log(howSum(0, [2, 4]));
console.log(howSum(300, [7, 14]));
console.log(howSum(834, [34, 12, 412, 41, 13, 9, 6]));
