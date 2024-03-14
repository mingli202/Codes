// write function to check if there is a path from source to destination
// graph is undirected

const undirectedPathDepth = (edges, source, destination) => {
  const graph = buildGraph(edges);
  // const hasVisited = [];

  return hasPath(graph, source, destination);
};

const buildGraph = (edges = [[]]) => {
  const graph = {};

  for (let edge of edges) {
    const [nodeA, nodeB] = edge;

    if (!(nodeA in graph)) graph[nodeA] = [];
    if (!(nodeB in graph)) graph[nodeB] = [];

    graph[nodeA].push(nodeB);
    graph[nodeB].push(nodeA);
  }

  return graph;
};

const hasPath = (
  graph = {},
  source = "",
  destination = "",
  hasVisited = new Set()
) => {
  if (source === destination) return true;
  if (hasVisited.has(source)) return false;
  hasVisited.add(source);

  for (let neighbor of graph[source]) {
    if (hasPath(graph, neighbor, destination, hasVisited)) return true;
  }

  return false;
};

const edges = [
  ["i", "j"],
  ["k", "i"],
  ["m", "k"],
  ["k", "l"],
  ["o", "n"],
];

console.log(undirectedPathDepth(edges, "j", "m")); // true
