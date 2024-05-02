use std::collections::HashMap;

fn main() {}

#[derive(PartialEq, Eq, Clone, Debug)]
pub struct ListNode {
    pub val: i32,
    pub next: Option<Box<ListNode>>,
}

impl ListNode {
    #[inline]
    fn new(val: i32) -> Self {
        ListNode { next: None, val }
    }
}

fn to_list_node(nums: &[i32]) -> Option<Box<ListNode>> {
    if nums.len() < 1 {
        return None;
    }

    let mut head = ListNode::new(0);
    let mut tmp = head.next;

    for n in nums {
        tmp = Some(Box::new(ListNode::new(*n)));
        tmp = tmp.unwrap().next;
    }

    Some(head.next?)
}

#[allow(dead_code)]
fn solution(list1: Option<Box<ListNode>>, list2: Option<Box<ListNode>>) -> Option<Box<ListNode>> {}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn example_1() {
        assert_eq!(
            to_list_node(&[1, 1, 2, 3, 4, 4]),
            solution(to_list_node(&[1, 2, 4]), to_list_node(&[1, 2, 3]))
        );
    }
}
