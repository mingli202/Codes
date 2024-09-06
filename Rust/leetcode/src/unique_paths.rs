use crate::Solution;

use std::collections::HashMap;

fn recursion(m: i32, n: i32, visited: &mut HashMap<(i32, i32), i32>) -> i32 {
    if m == 1 || n == 1 {
        return 1;
    }

    match visited.get(&(m, n)) {
        Some(val) => *val,
        None => {
            let num1 = recursion(m - 1, n, visited);
            let num2 = recursion(m, n - 1, visited);

            visited.insert((m, n), num1 + num2);

            num1 + num2
        }
    }
}

impl Solution {
    pub fn unique_paths(m: i32, n: i32) -> i32 {
        let mut visited = HashMap::with_capacity((m * n) as usize);

        recursion(m, n, &mut visited)
    }
}
