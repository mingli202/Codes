// write a function that accepts target string and array of strings
// return bool if target can be constructed with el of wordbank

const canConstruct = (targetString = "", wordBank = []) => {
  // table and seed it
  const table = Array(targetString.length + 1).fill(false);

  // trivial answer
  table[0] = true;

  // tabulate
  for (let i = 0; i <= targetString.length; i++) {
    if (table[i]) {
      for (let word of wordBank) {
        if (targetString.slice(i).startsWith(word)) {
          table[word.length + i] = true;
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
