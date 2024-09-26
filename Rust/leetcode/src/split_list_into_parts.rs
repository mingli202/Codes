use crate::{ListNode, Solution};

impl Solution {
    pub fn split_list_to_parts(head: Option<Box<ListNode>>, k: i32) -> Vec<Option<Box<ListNode>>> {
        let mut cursor = &head;

        let mut n = 0;

        while let Some(node) = cursor {
            n += 1;
            cursor = &node.next;
        }

        let size = n / k;
        let rem = n % k;

        let mut ans = vec![];

        let mut head = head;

        let mut current_size = size + 1;

        while let Some(node) = &head {
            if current_size == size + 1 {
                ans.push(Some((*node).clone()));
                current_size = 0;
            } else {
                current_size += 1;
                head = head.unwrap().next;
            }
        }

        ans
    }
}

#[cfg(test)]
mod test {
    use super::*;

    #[test]
    fn example_1() {
        assert_eq!(
            vec![
                ListNode::new(vec![1]),
                ListNode::new(vec![2]),
                ListNode::new(vec![3])
            ],
            Solution::split_list_to_parts(ListNode::new(vec![1, 2, 3]), 5)
        )
    }

    #[test]
    fn example_2() {
        assert_eq!(
            vec![
                ListNode::new(vec![1, 2, 3, 4]),
                ListNode::new(vec![5, 6, 7]),
                ListNode::new(vec![8, 9, 10])
            ],
            Solution::split_list_to_parts(ListNode::new(vec![1, 2, 3, 4, 5, 6, 7, 8, 9, 10]), 3)
        )
    }
}
