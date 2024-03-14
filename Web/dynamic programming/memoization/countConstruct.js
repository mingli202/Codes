// write a function that accepts a target string and array of string
// return number of ways the target can be construted with word of wordBank

const countConstruct = (targetString, wordBank, memo = {}) => {
  // base case
  if (targetString === "") return 1;

  // check if memoized
  if (targetString in memo) return memo[targetString];

  let ways = 0;

  // recursion
  for (let word of wordBank) {
    if (targetString.startsWith(word)) {
      ways += countConstruct(targetString.slice(word.length), wordBank, memo);
    }
  }

  memo[targetString] = ways;
  return ways;
};

// m = target.length
// n = wordBank.length

// brute force
// time: O(n^m * m)
// space: O(m^2)

// memoized:
// time: O(n * m^2)
// space: O(m^2)

console.log(countConstruct("abcdef", ["ab", "abc", "cd", "def", "abcd"])); // 1
console.log(countConstruct("purple", ["purp", "p", "ur", "le", "purpl"])); // 2
console.log(
  countConstruct("skateboard", ["bo", "rd", "ate", "t", "ska", "sk", "boar"])
); // 0
console.log(
  countConstruct("enterapotentpot", ["a", "p", "ent", "enter", "ot", "o", "t"])
); // 4
console.log(
  countConstruct("eeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeef", [
    "e",
    "ee",
    "eee",
    "eeee",
    "eeeee",
    "eeeeee",
    "eeeeeef",
  ])
); // 52862035640
