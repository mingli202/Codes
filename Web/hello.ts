type hello = {
  h: string;
};

const l: hello = {
  h: "hello world",
};

console.log(l);

// fibonacci function
const fib = (n: number) => {
  if (n === 1) return;
  return fib(n - 1) + fib(n - 2);
};

// write some tests
console.log(fib(4));
console.log(fib(5));
