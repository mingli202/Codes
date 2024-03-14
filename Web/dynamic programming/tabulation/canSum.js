// write a function 'cansum(targetsum, numbers)'
// takes in a target sum and an array of numbers as args
// return a boolean if possible to get targetSum from num in array
// may use elem of arr as many times possible
// all inputs are non-negative

const canSum = (targetSum, numbers) => {
  // create table and seed
  const table = Array(targetSum + 1).fill(false);

  // trivial answer
  table[0] = true;

  // tabulate
  for (let i = 0; i <= table.length; i++) {
    if (table[i]) {
      for (let num of numbers) {
        if (i + num < table.length) table[i + num] = true;
      }
    }
  }

  return table[targetSum];
};

// m = targetsum
// n = numbers.length

// time: O(m * n)
// space: O(m)

console.log(canSum(7, [2, 3]));
console.log(canSum(7, [5, 3, 4, 7]));
console.log(canSum(7, [2, 4]));
console.log(canSum(8, [2, 3, 5]));
console.log(canSum(300, [7, 14]));
