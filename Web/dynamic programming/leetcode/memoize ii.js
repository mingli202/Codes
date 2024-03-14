/**
 * @param {Function} fn
 * @return {Function}
 */
function memoize(fn) {
  // map object to simulate a trie
  const memo = new Map();

  return function (...args) {
    // current will travel down the trie
    let current = memo;

    // for every argument travel down its corresponding value
    for (let i = 0; i < args.length; i++) {
      // if argument node doesn't exist then create one
      if (current.has(args[i]) === false) {
        current.set(args[i], new Map());
      }

      // move current down the trie
      current = current.get(args[i]);
    }
    // check if there is already a computed value
    if (current.has("val") === false) {
      const val = fn(...args);
      current.set("val", val);
      return val;
    }

    return current.get("val");
  };
}

let callCount = 0;

const o = {};

const fn1 = function (...arr) {
  callCount++;
  return arr.reduce((a, b) => a + b, 0);
};
const inputs1 = [[], [1], [1], [], [1, 2], [1, 2]];

const fn2 = function (a, b) {
  callCount++;

  return { ...a, ...b };
};
const inputs2 = [
  [o, {}],
  [o, o],
  // [{}, {}],
  // [{}, o],
  // [o, {}],
  // [o, o],
  // [{}, o],
  // [o, {}],
  [o, o],
  [o, o],
  [o, {}],
  [o, {}],
  [o, {}],
];

const fn3 = function (a, b, c) {
  callCount++;
  return { ...a, ...b, c: c || null };
};
const inputs3 = [
  [o, { a: 1 }, 2],
  [o, o, 1],
  [o, o, 1],
  [o, o],
  [o, o],
];

const fn4 = function (a) {
  callCount++;
  return !!a;
};
const inputs4 = [[null], [undefined], [null], [undefined]];

const memoized = memoize(fn4);
for (let input of inputs4) {
  // console.log({ input });
  console.log({ val: memoized(...input), callCount });
}
