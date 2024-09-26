pub struct Solution;

#[derive(PartialEq, Eq, Clone, Debug)]
pub struct ListNode {
    pub val: i32,
    pub next: Option<Box<ListNode>>,
}

impl ListNode {
    pub fn new(arr: Vec<i32>) -> Option<Box<Self>> {
        let mut head = None;

        let mut cursor = &mut head;

        for val in arr {
            *cursor = Some(Box::new(ListNode { val, next: None }));

            cursor = &mut cursor.as_mut().unwrap().next;
        }
        head
    }
}

mod my_calendar_i;
pub use my_calendar_i::MyCalendar;
