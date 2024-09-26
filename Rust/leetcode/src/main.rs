use leetcode::MyCalendar;

fn main() {
    let calender = MyCalendar::new();
    dbg!(&calender);

    assert_eq!(
        [true, false, true],
        [[10, 20], [15, 25], [20, 30]].map(|[start, end]| { calender.book(start, end) })
    );
}
