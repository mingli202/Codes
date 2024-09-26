use crate::{ListNode, Solution};

fn gcd(a: i32, b: i32) -> i32 {
    if b == 0 {
        return a;
    }

    gcd(b, a % b)
}

impl Solution {
    pub fn insert_greatest_common_divisors(head: Option<Box<ListNode>>) -> Option<Box<ListNode>> {
        let mut head = head;

        let mut first = head.as_mut().unwrap();

        while let Some(second) = first.next.take() {
            let tmp = ListNode {
                val: gcd(first.val, second.val),
                next: Some(second),
            };
            first.next = Some(Box::new(tmp));
            first = first.next.as_mut().unwrap().next.as_mut().unwrap();
        }

        head
    }
}

#[cfg(test)]
mod tests {
    use crate::*;

    #[test]
    fn example_1() {
        assert_eq!(
            ListNode::new(vec![18, 6, 6, 2, 10, 1, 3]),
            Solution::insert_greatest_common_divisors(ListNode::new(vec![18, 6, 10, 3]))
        );
    }

    #[test]
    fn example_2() {
        assert_eq!(
            ListNode::new(vec![7]),
            Solution::insert_greatest_common_divisors(ListNode::new(vec![7]))
        );
    }
}
