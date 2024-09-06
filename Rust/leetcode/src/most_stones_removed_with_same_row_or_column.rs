use crate::Solution;

use std::collections::{HashMap, HashSet};

pub struct UnionSet {
    parents: HashMap<usize, usize>,
    size: HashMap<usize, usize>,
    count: i32,
    visited: HashSet<usize>,
}

impl UnionSet {
    fn new() -> Self {
        UnionSet {
            parents: HashMap::with_capacity(20002),
            size: HashMap::with_capacity(20002),
            count: 0,
            visited: HashSet::with_capacity(20002),
        }
    }

    fn find(&mut self, val: usize) -> usize {
        if !self.visited.contains(&val) {
            self.count += 1;
            self.visited.insert(val);
        }

        match self.parents.get(&val) {
            None => val,
            Some(v) => {
                let root = self.find(*v);

                self.parents.insert(val, root);

                root
            }
        }
    }

    fn union(&mut self, val1: usize, val2: usize) {
        let root1: usize = self.find(val1);
        let root2: usize = self.find(val2);

        if root1 == root2 {
            return;
        }

        let size1 = self.size.get(&root1).unwrap_or(&1);
        let size2 = self.size.get(&root2).unwrap_or(&1);

        if size1 < size2 {
            self.parents.insert(root2, root1);
            self.size.insert(root1, size1 + size2);
        } else {
            self.parents.insert(root1, root2);
            self.size.insert(root2, size1 + size2);
        }

        self.count -= 1;
    }
}

impl Solution {
    pub fn remove_stones(stones: Vec<Vec<i32>>) -> i32 {
        if stones.is_empty() {
            return 0;
        }

        let mut set = UnionSet::new();

        for s in stones.iter() {
            set.union(s[0] as usize, s[1] as usize + 10001);
        }

        stones.len() as i32 - set.count
    }
}

#[cfg(test)]
mod tests {
    use super::Solution;

    #[test]
    fn example_1() {
        assert_eq!(
            5,
            Solution::remove_stones(vec![
                [0, 0].to_vec(),
                [0, 1].to_vec(),
                [1, 0].to_vec(),
                [1, 2].to_vec(),
                [2, 1].to_vec(),
                [2, 2].to_vec(),
            ]),
        );
    }

    #[test]
    fn example_2() {
        assert_eq!(
            3,
            Solution::remove_stones(vec![
                [0, 0].to_vec(),
                [0, 2].to_vec(),
                [1, 1].to_vec(),
                [2, 0].to_vec(),
                [2, 2].to_vec(),
            ])
        )
    }

    #[test]
    fn example_3() {
        assert_eq!(0, Solution::remove_stones(vec![[0, 0].to_vec()]));
    }

    #[test]
    fn case_1() {
        assert_eq!(
            2,
            Solution::remove_stones(vec![[0, 1].to_vec(), [1, 0].to_vec(), [1, 1].to_vec(),])
        );
    }
}
