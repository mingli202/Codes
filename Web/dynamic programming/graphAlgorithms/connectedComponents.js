// count the number of connected components (number of islands)

const countConnectedComponents = (graph = {}) => {
  let count = 0;

  const visitedNodes = new Set();

  for (let key in graph) {
    // check if already visited
    if (travel(graph, +key, visitedNodes)) count++;
  }

  return count;
};

const travel = (graph, source, visited = new Set()) => {
  // return false if already visited
  if (visited.has(source)) return false;
  visited.add(source);

  // populate the visited set
  for (let neighbor of graph[source]) {
    travel(graph, neighbor, visited);
  }

  // if it just finished travelling down a component
  return true;
};

const graph = {
  0: [8, 1, 5],
  1: [0],
  5: [0, 8],
  8: [0, 5],
  2: [3, 4],
  3: [2, 4],
  4: [3, 2],
};

console.log(countConnectedComponents(graph)); // 2
