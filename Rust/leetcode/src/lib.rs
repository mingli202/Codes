pub struct Solution;

mod data_structures;

impl Solution {
    pub fn minimum_steps(s: String) -> i64 {
        let mut k = 0;

        s.chars().enumerate().fold(0i64, |acc, (i, c)| {
            if c == '0' {
                let a = acc + (i - k) as i64;
                k += 1;
                a
            } else {
                acc
            }
        })
    }
}

#[cfg(test)]
mod test {
    use crate::Solution;

    #[test]
    fn example_1() {
        assert_eq!(7, Solution::minimum_steps("01010001".to_string()));
    }
}
