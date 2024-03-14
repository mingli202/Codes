// write a function that takes in a target sum and array of numbers
// return array containing the shortest comb of num that adds to targetSum

const bestSum = (targetSum, numbers) => {
  // create table
  const table = Array(targetSum + 1).fill(null);

  // trivial answer
  table[0] = [];

  // tabulate
  for (let i = 0; i <= targetSum; i++) {
    if (table[i]) {
      for (let num of numbers) {
        if (!table[i + num] || table[i].length + 1 < table[i + num].length)
          table[i + num] = [...table[i], num];
      }
    }
  }

  // return
  return table[targetSum];
};

// m = target sum
// n = number.length

// time: O(m^2 * n)
// space: O(m&2)

console.log(bestSum(8, [2, 3, 5]));
console.log(bestSum(7, [5, 3, 4, 7]));
console.log(bestSum(0, [2, 4]));
console.log(bestSum(300, [7, 14]));
console.log(bestSum(834, [34, 12, 412, 41, 13, 9, 6]));
console.log(bestSum(100, [1, 4, 3, 2, 5, 25]));
