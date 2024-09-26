use crate::Solution;
use std::collections::HashMap;

#[derive(Clone, Debug, PartialEq, Eq)]
struct TrieNode {
    count: i32,
    next: HashMap<char, TrieNode>,
}

impl TrieNode {
    fn new() -> Self {
        TrieNode {
            count: 0,
            next: HashMap::with_capacity(26),
        }
    }

    fn insert(&mut self, s: &str) {
        let mut cursor = self;

        for c in s.chars() {
            cursor = cursor.next.entry(c).or_insert(TrieNode::new());
            cursor.count += 1;
        }
    }

    fn get(&self, s: &str) -> i32 {
        let mut cursor = self;

        let mut count = 0;

        for c in s.chars() {
            cursor = cursor.next.get(&c).unwrap();
            count += cursor.count;
        }
        count
    }
}

impl Solution {
    pub fn sum_prefix_scores(words: Vec<String>) -> Vec<i32> {
        let mut trie = TrieNode::new();

        for word in &words {
            trie.insert(word);
        }

        words.iter().map(|word| trie.get(word)).collect()
    }
}
