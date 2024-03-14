// write a function hasPath
// checks if can start at source and end at destination

// n: number of nodes
// time: O(n^2)
// space: O(n)

// depthFirst
const hasPathDepth = (graph, source, destination) => {
  if (source === destination) return true;

  for (let neighbors of graph[source]) {
    if (hasPathDepth(graph, neighbors, destination)) return true;
  }

  return false;
};

// breadth
const hasPathBreadth = (graph, source, destination) => {
  const queue = [source];

  while (queue.length > 0) {
    const current = queue.shift();
    if (current === destination) return true;

    for (let neighbor of graph[current]) {
      queue.push(neighbor);
    }
  }

  return false;
};

const graph = {
  f: ["g", "i"],
  g: ["h"],
  h: [],
  i: ["g", "k"],
  j: ["i"],
  k: [],
};

console.log(hasPathDepth(graph, "f", "k")); // true
console.log(hasPathDepth(graph, "j", "f")); // false
console.log(hasPathBreadth(graph, "f", "k")); // true
console.log(hasPathBreadth(graph, "j", "f")); // false
