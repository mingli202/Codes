// write a function that takes target string and array of string
// return 2D array containing all the ways that target can be constructed

const allConstruct = (targetString = "", wordBank = []) => {
  // table and seed it
  const table = Array(targetString.length + 1)
    .fill()
    .map(() => []);

  // trivial answer
  table[0] = [[]];

  // tabulate
  for (let i = 0; i <= targetString.length; i++) {
    if (table[i].length !== 0) {
      for (let word of wordBank) {
        if (targetString.slice(i).startsWith(word)) {
          table[word.length + i].push(...table[i].map((way) => [...way, word]));
        }
      }
    }
  }

  // return answer
  return table[targetString.length];
};

// m = targetString.length
// n = wordBank.length

// time: O(n^m)
// space: O(n^m)

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
  allConstruct("eeeeeeeeeeeez", ["e", "ee", "eee", "eeee", "eeeee", "eeeeee"])
); // 0
