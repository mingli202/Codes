use std::collections::HashSet;

fn contains_nearby_duplicate(nums: Vec<i32>, k: i32) -> bool {
    if k == 0 {
        return false;
    };

    let mut set: HashSet<i32> = HashSet::with_capacity((k + 1) as usize);

    for (i, n) in nums.iter().enumerate() {
        match set.get(n) {
            Some(_) => return true,
            _ => {
                if set.len() == k as usize {
                    set.remove(&nums[i - k as usize]);
                }

                set.insert(*n);
            }
        };
    }

    false
}

pub fn test() {
    assert!(contains_nearby_duplicate(vec![1, 2, 3, 1], 3));
    assert!(contains_nearby_duplicate(vec![1, 0, 1, 1], 1));
    assert!(!contains_nearby_duplicate(vec![1, 2, 3, 1, 2, 3], 2));
    assert!(!contains_nearby_duplicate(vec![1, 2, 1], 0));
}
