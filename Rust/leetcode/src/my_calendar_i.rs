use std::cell::RefCell;
use std::cmp::Ordering;

#[derive(Debug)]
pub struct MyCalendar {
    segment_tree: RefCell<Vec<bool>>,
}

impl MyCalendar {
    pub fn new() -> Self {
        MyCalendar {
            segment_tree: RefCell::new(vec![false; 10_usize.pow(9) + 1]),
        }
    }

    pub fn book(&self, start: i32, end: i32) -> bool {
        let left = 0;
        let right = 10i32.pow(9);
        let is_taken = self.search(left, right, start, end) && self.search(left, right, start, end);

        if is_taken {
            return false;
        } else {
        }

        is_taken
    }

    fn search(&self, left: i32, right: i32, start: i32, end: i32) -> bool {
        let mid = (left + right) / 2;

        match start.cmp(&mid) {
            Ordering::Less => self.search(left, mid - 1, start, end),
            Ordering::Greater => self.search(mid + 1, right, start, end),
            Ordering::Equal => self.segment_tree.borrow()[mid as usize],
        }
    }

    fn update(&self, start: i32, end: i32, target: i32) -> bool {
        let mid = (start + end) / 2;

        match target.cmp(&mid) {
            Ordering::Less => self.update(start, mid - 1, target),
            Ordering::Greater => self.update(mid + 1, end, target),
            Ordering::Equal => true,
        }
    }
}

impl Default for MyCalendar {
    fn default() -> Self {
        MyCalendar::new()
    }
}

#[cfg(test)]
mod tests {
    use super::MyCalendar;

    #[test]
    fn example_1() {
        let calender = MyCalendar::new();

        assert_eq!(
            [true, false, true],
            [[10, 20], [15, 25], [20, 30]].map(|[start, end]| { calender.book(start, end) })
        );
    }
}
