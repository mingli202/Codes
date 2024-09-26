use crate::Solution;

impl Solution {
    pub fn spiral_matrix(m: i32, n: i32, head: Option<Box<ListNode>>) -> Vec<Vec<i32>> {
        let mut head = head;

        let mut matrix = vec![vec![-1; n as usize]; m as usize];

        let mut left: usize = 0;
        let mut right: usize = n as usize;
        let mut top: usize = 0;
        let mut bot: usize = m as usize;

        'outer: while top < bot && left < right {
            for _k in left..right {
                if let Some(list) = head {
                    matrix[top][_k] = list.val;
                    head = list.next;
                } else {
                    break 'outer;
                }
            }
            top += 1;

            for _i in top..bot {
                if let Some(list) = head {
                    matrix[_i][right - 1] = list.val;
                    head = list.next;
                } else {
                    break 'outer;
                }
            }
            right -= 1;

            for _k in (left..right).rev() {
                if let Some(list) = head {
                    matrix[bot - 1][_k] = list.val;
                    head = list.next;
                } else {
                    break 'outer;
                }
            }
            bot -= 1;

            for _i in (top..bot).rev() {
                if let Some(list) = head {
                    matrix[_i][left] = list.val;
                    head = list.next;
                } else {
                    break 'outer;
                }
            }
            left += 1;
        }

        matrix
    }
}

#[cfg(test)]
mod tests {

    use super::{ListNode, Solution};

    #[test]
    fn example_1() {
        assert_eq!(
            vec![
                [3, 0, 2, 6, 8].to_vec(),
                [5, 0, -1, -1, 1].to_vec(),
                [5, 2, 4, 9, 7].to_vec(),
            ],
            Solution::spiral_matrix(
                3,
                5,
                ListNode::new(vec![3, 0, 2, 6, 8, 1, 7, 9, 4, 2, 5, 5, 0])
            )
        );
    }

    #[test]
    fn example_2() {
        assert_eq!(
            vec![[0, 1, 2, -1].to_vec(),],
            Solution::spiral_matrix(1, 4, ListNode::new(vec![0, 1, 2]))
        );
    }
}
