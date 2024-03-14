// write a function that accepts target string and array of strings
// return bool if target can be constructed with el of wordbank

const canConstruct = (targetString, wordBank, memo = {}) => {
  // base case
  if (targetString === "") return true;

  // check memo
  if (targetString in memo) return memo[targetString];

  // loop through possible combinasions
  for (let word of wordBank) {
    if (targetString.startsWith(word)) {
      const suffix = targetString.slice(word.length);

      const returned = canConstruct(suffix, wordBank, memo);
      if (returned) {
        memo[targetString] = true;
        return true;
      }
    }
  }

  memo[targetString] = false;
  return false;
};

// m = target.length
// n = wordbank.length

// brute force:
// height of tree = m
// time: O(n^m * m)

// memoize:
// time: O(n * m^2)
// space: O(m^2)
console.log(canConstruct("abcdef", ["ab", "abc", "cd", "def", "abcd"])); // abc + def
console.log(canConstruct("purple", ["purp", "p", "ur", "le", "purpl"]));
console.log(
  canConstruct("skateboard", ["bo", "rd", "ate", "t", "ska", "sk", "boar"])
); // false
console.log(
  canConstruct("enterapotentpot", ["a", "p", "ent", "enter", "ot", "o", "t"])
); // true
console.log(
  canConstruct("eeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeef", [
    "e",
    "ee",
    "eee",
    "eeee",
    "eeeee",
    "eeeeee",
  ])
);
