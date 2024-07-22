use std::collections::HashSet;

fn contains_nearby_almost_duplicate(nums: Vec<i32>, index_diff: i32, value_diff: i32) -> bool {
    if index_diff == 0 {
        return false;
    };

    let mut set: HashSet<i32> = HashSet::with_capacity((index_diff + 1) as usize);

    for (i, n) in nums.iter().enumerate() {
        if set.iter().any(|_s| (n - _s).abs() <= value_diff) {
            return true;
        }

        if set.len() == index_diff as usize {
            set.remove(&nums[i - index_diff as usize]);
        }

        set.insert(*n);
    }

    false
}

pub fn test() {
    assert!(contains_nearby_almost_duplicate(vec![1, 2, 3, 1], 3, 0));
    assert!(!contains_nearby_almost_duplicate(
        vec![1, 5, 9, 1, 5, 9],
        2,
        3
    ));
}
