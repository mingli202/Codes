const depthFirstPrint = (graph, source) => {
  // use array so we only manipulate the end of the array
  const stack = [source];

  // when its empty its done
  while (stack.length > 0) {
    const current = stack.pop();
    console.log(current);

    // push to stack the neighbors of node current
    for (let neighbor of graph[current]) {
      stack.push(neighbor);
    }
  }
};

const depthFirstPrintRecursive = (graph, source) => {
  console.log(source);

  // loop through the neighbors and call on them
  // if source has empty array then it won't call the function
  for (let neighbor of graph[source]) {
    depthFirstPrintRecursive(graph, neighbor);
  }
};

const graph = {
  a: ["b", "c"],
  b: ["d"],
  c: ["e"],
  d: ["f"],
  e: [],
  f: [],
};

// depth first travels a graph from a single direction until it can't
// then switches the next direction when a end node is reached
// a -> c -> e -> end
// a -> b -> d -> f -> end
depthFirstPrint(graph, "a"); // acebdf
depthFirstPrintRecursive(graph, "a"); // abdfce
