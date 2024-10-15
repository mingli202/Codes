#![allow(unused)]

use std::collections::{HashMap, HashSet};

pub struct UnionSet {
    parents: HashMap<usize, usize>,
    size: HashMap<usize, usize>,
    count: i32,
    visited: HashSet<usize>,
}

impl UnionSet {
    pub fn new() -> Self {
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

    pub fn union(&mut self, val1: usize, val2: usize) {
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
