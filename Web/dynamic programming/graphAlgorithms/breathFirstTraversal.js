const breadthFirstPrint = (graph, source) => {
  const queue = [source];

  while (queue.length > 0) {
    // removes first node of queue and sets it as the current one
    const current = queue.shift();
    console.log(current);

    // loop through neighbors and add to back of queue
    for (let neighbor of graph[current]) {
      queue.push(neighbor);
    }
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

// adds neighbors to queue to process their neighbors later
// neighbors of a: a -> b -> c
// neighbors of b: b -> d
// neighbors of c: c -> e
// neighbors of d: d -> f
// neighbors of e: e -> end
// neighbors of f: f -> end

// a -> b -> c -> d -> e -> f
breadthFirstPrint(graph, "a"); // abcdef
