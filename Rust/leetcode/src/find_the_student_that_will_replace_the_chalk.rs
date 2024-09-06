use crate::Solution;

impl Solution {
    pub fn chalk_replacer(chalk: Vec<i32>, k: i32) -> i32 {
        let mut sum = 0;

        for val in chalk.iter() {
            sum += *val as i128;
        }

        let mut k = k as i128 % sum;

        let mut i = 0;

        loop {
            if k < chalk[i] as i128 {
                break;
            }
            k -= chalk[i] as i128;
            i += 1;

            if i == chalk.len() {
                i = 0;
            }
        }

        i as i32
    }
}

#[cfg(test)]
mod tests {
    use crate::Solution;

    #[test]
    fn example_1() {
        assert_eq!(0, Solution::chalk_replacer(vec![5, 1, 5], 22));
    }

    #[test]
    fn example_2() {
        assert_eq!(1, Solution::chalk_replacer(vec![3, 4, 1, 2], 25));
    }

    #[test]
    fn me_1() {
        assert_eq!(1, Solution::chalk_replacer(vec![123, 24, 1, 3], 1234));
    }
}
