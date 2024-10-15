#![allow(unused)]

pub struct MyCircularDeque {
    vec: Vec<i32>,
    len: usize,
    capacity: usize,
    front: usize,
    back: usize,
}

/**
 * `&self` means the method takes an immutable reference.
 * If you need a mutable reference, change it to `&mut self` instead.
 */
impl MyCircularDeque {
    pub fn new(k: i32) -> Self {
        MyCircularDeque {
            vec: vec![-1; k as usize],
            len: 0,
            capacity: k as usize,
            front: 0,
            back: k as usize - 1,
        }
    }

    pub fn insert_front(&mut self, value: i32) -> bool {
        if self.is_full() {
            return false;
        }

        self.front = (self.front + self.capacity - 1) % self.capacity;

        self.vec[self.front] = value;

        self.len += 1;
        true
    }

    pub fn insert_last(&mut self, value: i32) -> bool {
        if self.is_full() {
            return false;
        }

        self.back = (self.back + 1) % self.capacity;

        self.vec[self.back] = value;

        self.len += 1;
        true
    }

    pub fn delete_front(&mut self) -> bool {
        if self.is_empty() {
            return false;
        }

        self.vec[self.front] = -1;

        self.front = (self.front + 1) % self.capacity;

        self.len -= 1;
        true
    }

    pub fn delete_last(&mut self) -> bool {
        if self.is_empty() {
            return false;
        }

        self.vec[self.back] = -1;

        self.back = (self.back + self.capacity - 1) % self.capacity;

        self.len -= 1;
        true
    }

    pub fn get_front(&self) -> i32 {
        self.vec[self.front]
    }

    pub fn get_rear(&self) -> i32 {
        self.vec[self.back]
    }

    pub fn is_empty(&self) -> bool {
        self.len == 0
    }

    pub fn is_full(&self) -> bool {
        self.len == self.capacity
    }
}
