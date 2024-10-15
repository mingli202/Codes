#[derive(Debug)]
pub struct MyCalendar {
    booked: Vec<(i32, i32)>,
}

impl MyCalendar {
    pub fn new() -> Self {
        MyCalendar { booked: Vec::new() }
    }

    pub fn book(&mut self, start: i32, end: i32) -> bool {
        if self.booked.is_empty() {
            self.booked.push((start, end));
            return true;
        }

        match self.booked.binary_search_by(|(a, _)| a.cmp(&start)) {
            Ok(_) => false,
            Err(i) => {
                let mut can_book = true;

                if let Some((a, _)) = self.booked.get(i) {
                    can_book &= end <= *a;
                }

                if let Some((_, b)) = self.booked.get((i as i32 - 1) as usize) {
                    can_book &= start >= *b;
                }

                if can_book {
                    self.booked.insert(i, (start, end));
                }

                can_book
            }
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
        let mut calender = MyCalendar::new();

        assert_eq!(
            [true, false, true],
            [[10, 20], [15, 25], [20, 30]].map(|[start, end]| { calender.book(start, end) })
        );
    }
}
