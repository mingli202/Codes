#[cfg(test)]
mod test {
    use crate::data_structures::circular_deque::MyCircularDeque;

    #[test]
    fn example_1() {
        let mut deque = MyCircularDeque::new(3);

        assert!(deque.insert_last(1)); // return True
        assert!(deque.insert_last(2)); // return True
        assert!(deque.insert_front(3)); // return True
        assert!(!deque.insert_front(4)); // return False, the queue is full.
        assert_eq!(2, deque.get_rear()); // return 2
        assert!(deque.is_full()); // return True
        assert!(deque.delete_last()); // return True
        assert!(deque.insert_front(4)); // return True
        assert_eq!(4, deque.get_front()); // return 4
    }
}
