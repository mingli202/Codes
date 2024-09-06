use crate::Solution;

impl Solution {
    pub fn missing_rolls(rolls: Vec<i32>, mean: i32, n: i32) -> Vec<i32> {
        let m = rolls.len() as i32;

        let remainder = mean * (m + n) - rolls.iter().sum::<i32>();

        if remainder < n || remainder > n * 6 {
            return vec![];
        }

        let middle = remainder / n;
        let mut left_out = remainder % n;

        let mut ans = vec![middle; n as usize];

        let offset = 6 - middle;

        let mut i = 0;
        while left_out + middle > 6 {
            ans[i] = 6;

            left_out -= offset;
            i += 1;
        }

        ans[i] += left_out;

        ans
    }
}

#[cfg(test)]
mod tests {
    use super::Solution;

    #[test]
    fn example_1() {
        assert_eq!(vec![6, 6], Solution::missing_rolls(vec![3, 2, 4, 3], 4, 2));
    }

    #[test]
    fn example_2() {
        assert_eq!(
            vec![3, 2, 2, 2],
            Solution::missing_rolls(vec![1, 5, 6], 3, 4)
        );
    }

    #[test]
    fn example_3() {
        let ans: Vec<i32> = vec![];
        assert_eq!(ans, Solution::missing_rolls(vec![1, 2, 3, 4], 6, 4));
    }

    #[test]
    fn case_1() {
        assert_eq!(
            vec![
                6, 6, 6, 5, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
                4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4
            ],
            Solution::missing_rolls(
                vec![
                    4, 5, 6, 2, 3, 6, 5, 4, 6, 4, 5, 1, 6, 3, 1, 4, 5, 5, 3, 2, 3, 5, 3, 2, 1, 5,
                    4, 3, 5, 1, 5
                ],
                4,
                40
            )
        )
    }
}
