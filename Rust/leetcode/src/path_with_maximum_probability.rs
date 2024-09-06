use std::collections::{HashMap, HashSet};

use crate::Solution;

#[derive(Debug)]
struct Graph {
    adjacency_list: HashMap<i32, Vec<i32>>,
    prob: HashMap<(i32, i32), f64>,
    distance: HashMap<i32, f64>,
    visited: HashSet<i32>,
}

impl Graph {
    fn new(n: usize, edges: Vec<Vec<i32>>, succ_prob: Vec<f64>) -> Graph {
        let mut adjacency_list = HashMap::with_capacity(n);
        let mut prob = HashMap::new();

        for (i, edge) in edges.iter().enumerate() {
            let entry_1 = adjacency_list.entry(edge[0]).or_insert(vec![]);
            entry_1.push(edge[1]);

            let entry_2 = adjacency_list.entry(edge[1]).or_insert(vec![]);
            entry_2.push(edge[0]);

            prob.insert((edge[0], edge[1]), succ_prob[i]);
            prob.insert((edge[1], edge[0]), succ_prob[i]);
        }

        Graph {
            adjacency_list,
            prob,
            distance: HashMap::new(),
            visited: HashSet::new(),
        }
    }

    fn search(&mut self, start_node: i32, end_node: i32) -> f64 {
        let mut stack = vec![start_node];

        self.distance.insert(start_node, 1.0);

        while let Some(current) = stack.pop() {
            self.visited.insert(current);

            match self.adjacency_list.get(&current) {
                None => return 0.0,
                Some(connected) => {
                    for node in connected.iter() {
                        if !self.visited.contains(node) {
                            stack.push(*node);
                            self.visited.insert(*node);
                        }

                        let p = self.distance.get(&current).unwrap()
                            * self.prob.get(&(current, *node)).unwrap();

                        let p_node = self.distance.get(node).unwrap_or(&0.0);

                        if p > *p_node {
                            self.distance.insert(*node, p);
                        }
                    }
                }
            }
        }

        *self.distance.get(&end_node).unwrap_or(&0.0)
    }
}

impl Solution {
    pub fn max_probability(
        n: i32,
        edges: Vec<Vec<i32>>,
        succ_prob: Vec<f64>,
        start_node: i32,
        end_node: i32,
    ) -> f64 {
        let mut graph = Graph::new(n as usize, edges, succ_prob);

        graph.search(start_node, end_node)
    }
}

#[cfg(test)]
mod tests {
    use crate::Solution;

    #[test]
    fn example_1() {
        assert_eq!(
            0.25,
            Solution::max_probability(
                3,
                vec![[0, 1].to_vec(), [1, 2].to_vec(), [0, 2].to_vec(),],
                vec![0.5, 0.5, 0.2],
                0,
                2
            )
        );
    }

    #[test]
    fn example_2() {
        assert_eq!(
            0.3,
            Solution::max_probability(
                3,
                vec![[0, 1].to_vec(), [1, 2].to_vec(), [0, 2].to_vec(),],
                vec![0.5, 0.5, 0.3],
                0,
                2
            )
        );
    }

    #[test]
    fn example_3() {
        assert_eq!(
            0.0,
            Solution::max_probability(3, vec![[0, 1].to_vec()], vec![0.5], 0, 2)
        );
    }
}
