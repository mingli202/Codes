use std::collections::HashSet;

fn contains_duplicate(nums: Vec<i32>) -> bool {
    let mut set: HashSet<i32> = HashSet::with_capacity(nums.len());

    for i in nums {
        match set.get(&i) {
            Some(_) => return true,
            _ => set.insert(i),
        };
    }

    false
}

pub fn test() {
    assert!(contains_duplicate(vec![1, 2, 3, 1]));
    assert!(!contains_duplicate(vec![1, 2, 3, 4]));
    assert!(contains_duplicate(vec![1, 1, 1, 3, 3, 4, 3, 2, 4, 2]));
}
