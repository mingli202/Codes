use crate::Solution;

fn recurse(i: usize, k: usize, grid: &mut [Vec<i32>], mut target: usize) -> i32 {
    if grid[i][k] == 2 {
        if target == 0 {
            return 1;
        } else {
            return 0;
        }
    }

    let mut n_paths = 0;

    grid[i][k] = -1;
    target -= 1;

    let i = i as i32;
    let k = k as i32;

    for (_i, _k) in [(i + 1, k), (i - 1, k), (i, k + 1), (i, k - 1)] {
        if _i < 0
            || _i >= grid.len() as i32
            || _k < 0
            || _k >= grid[0].len() as i32
            || grid[_i as usize][_k as usize] == -1
        {
            continue;
        }

        n_paths += recurse(_i as usize, _k as usize, grid, target);
    }
    // backtrack
    grid[i as usize][k as usize] = 0;

    n_paths
}

impl Solution {
    pub fn unique_paths_iii(grid: Vec<Vec<i32>>) -> i32 {
        let mut grid = grid;

        let mut start = (0, 0);
        let mut target = 1;

        for (i, v) in grid.iter().enumerate() {
            for (k, el) in v.iter().enumerate() {
                if *el == 1 {
                    start = (i, k);
                }
                if *el == 0 {
                    target += 1;
                }
            }
        }

        let (i, k) = start;

        recurse(i, k, &mut grid, target)
    }
}

#[cfg(test)]
mod tests {
    use crate::Solution;

    #[test]
    fn example_1() {
        assert_eq!(
            2,
            Solution::unique_paths_iii(vec![
                [1, 0, 0, 0].to_vec(),
                [0, 0, 0, 0].to_vec(),
                [0, 0, 2, -1].to_vec(),
            ])
        );
    }
}
