// write a function 'cansum(targetsum, numbers)'
// takes in a target sum and an array of numbers as args
// return a boolean if possible to get targetSum from num in array
// may use elem of arr as many times possible
// all inputs are non-negative

const canSum = (targetSum, numbers, memo = {}) => {
  // base case
  if (targetSum === 0) return true;

  // check is already computed
  if (targetSum in memo) return memo[targetSum];

  // recursion type shit
  // if at least one combinasion works, then true
  memo[targetSum] = numbers.some((num) => {
    if (targetSum < num) return false;

    return canSum(targetSum - num, numbers, memo);
  });

  return memo[targetSum];
};

console.log(canSum(7, [2, 3]));
console.log(canSum(7, [5, 3, 4, 7]));
console.log(canSum(7, [2, 4]));
console.log(canSum(8, [2, 3, 5]));
console.log(canSum(300, [7, 14]));
