// write a function that takes target string and array of string
// return 2D array containing all the ways that target can be constructed

const allConstruct = (targetString, wordBank, memo = {}) => {
  // base case
  if (targetString === "") return [[]];

  // check if memoized
  if (targetString in memo) return memo[targetString];

  let ways = [];

  // recursion
  for (let word of wordBank) {
    if (targetString.startsWith(word)) {
      const returned = allConstruct(
        targetString.slice(word.length),
        wordBank,
        memo
      );

      ways.push(...returned.map((way) => [word, ...way]));
    }
  }

  memo[targetString] = ways;
  return ways;
};

// m = targetString.length
// n = wordBank.length

// brute force and memoized:
// time: O(n^m)
// space: O(m)

console.log(
  allConstruct("abcdef", ["ab", "abc", "cd", "def", "abcd", "ef", "c"])
); // 1
console.log(allConstruct("purple", ["purp", "p", "ur", "le", "purpl"])); // 2
console.log(
  allConstruct("skateboard", ["bo", "rd", "ate", "t", "ska", "sk", "boar"])
); // 0
console.log(
  allConstruct("enterapotentpot", ["a", "p", "ent", "enter", "ot", "o", "t"])
); // 4
console.log(
  allConstruct("eeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeez", [
    "e",
    "ee",
    "eee",
    "eeee",
    "eeeee",
    "eeeeee",
  ])
); // 0
