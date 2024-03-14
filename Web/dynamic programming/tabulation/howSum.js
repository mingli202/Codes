// write a function that takes targetSum and array of number
// return arr of combinasion that adds up to targetSum
// if non then return null

const howSum = (targetSum, numbers) => {
  // create table and seed
  const table = Array(targetSum + 1).fill(null);

  // trivial answer
  table[0] = [];

  // tabulate
  for (let i = 0; i <= targetSum; i++) {
    if (table[i] !== null) {
      for (let num of numbers) {
        if (i + num <= table.length) {
          table[i + num] = [...table[i], num];
        }
      }
    }
  }

  // return
  return table[targetSum];
};

// m = targetSum
// n = numbers.length

// time: O(m^2*n)
// space: O(m^2)

console.log(howSum(8, [2, 3, 5]));
console.log(howSum(7, [5, 3, 4, 7]));
console.log(howSum(0, [2, 4]));
console.log(howSum(300, [7, 14]));
console.log(howSum(834, [34, 12, 412, 41, 13, 9, 6]));
