use crate::{data_structures::heap::MaxHeap, Solution};

impl Solution {
    pub fn max_kelements(nums: Vec<i32>, k: i32) -> i64 {
        let mut heap = MaxHeap::from(nums);

        println!("{}", heap);

        let mut score: i64 = 0;

        for _ in 0..k {
            let max = heap.get_max().unwrap();
            score += *max as i64;
            heap.modify((*max as f64 / 3.0).ceil() as i32, 0);
        }

        score
    }
}

#[cfg(test)]
mod test {
    use crate::Solution;

    #[test]
    fn example_1() {
        assert_eq!(50, Solution::max_kelements(vec![10, 10, 10, 10, 10], 5));
    }

    #[test]
    fn example_2() {
        assert_eq!(17, Solution::max_kelements(vec![1, 10, 3, 3, 3], 3));
    }

    #[test]
    fn case_1() {
        assert_eq!(
            2476180565,
            Solution::max_kelements(vec![672579538, 806947365, 854095676, 815137524], 3)
        );
    }

    #[test]
    fn case_2() {
        assert_eq!(
            4120240855,
            Solution::max_kelements(
                vec![
                    245623986, 825265439, 662379008, 36757440, 54175861, 499499885, 550777192,
                    370626497, 908769625
                ],
                7
            )
        );
    }

    #[test]
    fn case_3() {
        assert_eq!(
            4787892725,
            Solution::max_kelements(
                vec![
                    288709591, 607805715, 930955484, 997716238, 958823991, 894870513, 948321383,
                    952075629, 642331159, 749867823, 502505385, 524448086, 295485242, 286249900
                ],
                5
            )
        )
    }
}
