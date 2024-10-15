use crate::Solution;

use crate::data_structures::unionset::UnionSet;

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
