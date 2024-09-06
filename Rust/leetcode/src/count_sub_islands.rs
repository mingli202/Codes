use crate::Solution;

impl Solution {
    pub fn count_sub_islands(grid1: Vec<Vec<i32>>, grid2: Vec<Vec<i32>>) -> i32 {
        let mut grid2 = grid2;
        let mut stack: Vec<(usize, usize)> = vec![];
        let mut n_islands = 0;

        for i in 0..grid2.len() {
            for k in 0..grid2[0].len() {
                if grid2[i][k] != 1 {
                    continue;
                }

                stack.push((i, k));

                let mut is_sub = 1;

                while let Some((i, k)) = stack.pop() {
                    let i = i as i32;
                    let k = k as i32;

                    for (_i, _k) in [(i - 1, k), (i + 1, k), (i, k - 1), (i, k + 1)] {
                        if _i < 0
                            || _i >= grid1.len() as i32
                            || _k < 0
                            || _k >= grid1[0].len() as i32
                        {
                            continue;
                        }

                        let _i = _i as usize;
                        let _k = _k as usize;

                        if grid2[_i][_k] != 1 {
                            continue;
                        }
                        stack.push((_i, _k));
                        grid2[_i][_k] = -1;
                    }

                    if grid1[i as usize][k as usize] == 0 {
                        is_sub = 0;
                    }
                }

                n_islands += is_sub;
            }
        }

        n_islands
    }
}

#[cfg(test)]
mod test {
    use super::Solution;

    #[test]
    fn example_1() {
        assert_eq!(
            3,
            Solution::count_sub_islands(
                vec![
                    [1, 1, 1, 0, 0].to_vec(),
                    [0, 1, 1, 1, 1].to_vec(),
                    [0, 0, 0, 0, 0].to_vec(),
                    [1, 0, 0, 0, 0].to_vec(),
                    [1, 1, 0, 1, 1].to_vec()
                ],
                vec![
                    [1, 1, 1, 0, 0].to_vec(),
                    [0, 0, 1, 1, 1].to_vec(),
                    [0, 1, 0, 0, 0].to_vec(),
                    [1, 0, 1, 1, 0].to_vec(),
                    [0, 1, 0, 1, 0].to_vec()
                ]
            )
        );
    }

    #[test]
    fn example_2() {
        assert_eq!(
            2,
            Solution::count_sub_islands(
                vec![
                    [1, 0, 1, 0, 1].to_vec(),
                    [1, 1, 1, 1, 1].to_vec(),
                    [0, 0, 0, 0, 0].to_vec(),
                    [1, 1, 1, 1, 1].to_vec(),
                    [1, 0, 1, 0, 1].to_vec()
                ],
                vec![
                    [0, 0, 0, 0, 0].to_vec(),
                    [1, 1, 1, 1, 1].to_vec(),
                    [0, 1, 0, 1, 0].to_vec(),
                    [0, 1, 0, 1, 0].to_vec(),
                    [1, 0, 0, 0, 1].to_vec()
                ]
            )
        );
    }

    #[test]
    fn case_1() {
        assert_eq!(
            0,
            Solution::count_sub_islands(
                vec![
                    [1, 1, 1, 1, 0, 0].to_vec(),
                    [1, 1, 0, 1, 0, 0].to_vec(),
                    [1, 0, 0, 1, 1, 1].to_vec(),
                    [1, 1, 1, 0, 0, 1].to_vec(),
                    [1, 1, 1, 1, 1, 0].to_vec(),
                    [1, 0, 1, 0, 1, 0].to_vec(),
                    [0, 1, 1, 1, 0, 1].to_vec(),
                    [1, 0, 0, 0, 1, 1].to_vec(),
                    [1, 0, 0, 0, 1, 0].to_vec(),
                    [1, 1, 1, 1, 1, 0].to_vec()
                ],
                vec![
                    [1, 1, 1, 1, 0, 1].to_vec(),
                    [0, 0, 1, 0, 1, 0].to_vec(),
                    [1, 1, 1, 1, 1, 1].to_vec(),
                    [0, 1, 1, 1, 1, 1].to_vec(),
                    [1, 1, 1, 0, 1, 0].to_vec(),
                    [0, 1, 1, 1, 1, 1].to_vec(),
                    [1, 1, 0, 1, 1, 1].to_vec(),
                    [1, 0, 0, 1, 0, 1].to_vec(),
                    [1, 1, 1, 1, 1, 1].to_vec(),
                    [1, 0, 0, 1, 0, 0].to_vec()
                ]
            )
        );
    }
}
