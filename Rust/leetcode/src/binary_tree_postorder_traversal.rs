use std::cell::RefCell;
use std::rc::Rc;

use crate::Solution;

#[derive(Debug, PartialEq, Eq)]
pub struct TreeNode {
    pub val: i32,
    pub left: Option<Rc<RefCell<TreeNode>>>,
    pub right: Option<Rc<RefCell<TreeNode>>>,
}

impl TreeNode {
    pub fn new(val: i32) -> Self {
        TreeNode {
            val,
            left: None,
            right: None,
        }
    }
}

impl Solution {
    // 0ms, beats 100.00%
    // 2.08 mb, beats 84.51%
    pub fn postorder_traversal(root: Option<Rc<RefCell<TreeNode>>>) -> Vec<i32> {
        let mut ans: Vec<i32> = vec![];

        if let Some(node) = root {
            let left = node.borrow().left.clone();

            let mut v_left = Self::postorder_traversal(left);
            ans.append(&mut v_left);

            let right = node.borrow().right.clone();

            let mut v_right = Self::postorder_traversal(right);
            ans.append(&mut v_right);

            ans.push(node.borrow().val);
        }

        ans
    }
}
