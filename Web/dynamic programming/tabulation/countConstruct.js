// write a function that accepts a target string and array of string
// return number of ways the target can be construted with word of wordBank

const countConstruct = (targetString = "", wordBank = []) => {
  // table and seed it
  const table = Array(targetString.length + 1).fill(0);

  // trivial answer
  table[0] = 1;

  // tabulate
  for (let i = 0; i <= targetString.length; i++) {
    if (table[i]) {
      for (let word of wordBank) {
        if (targetString.slice(i).startsWith(word)) {
          table[word.length + i] += table[i];
        }
      }
    }
  }

  // return answer
  return table[targetString.length];
};

// m = target.length
// n = wordbank.length

// time: O(m^2 * n)
// space: O(m)

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
