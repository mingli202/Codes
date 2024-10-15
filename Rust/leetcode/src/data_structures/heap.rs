#![allow(unused)]

use std::fmt::Display;

pub struct MaxHeap {
    arr: Vec<i32>,
}

impl Default for MaxHeap {
    fn default() -> Self {
        MaxHeap::new()
    }
}

impl Display for MaxHeap {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        write!(f, "{:?}", self.arr)
    }
}

impl MaxHeap {
    pub fn new() -> Self {
        MaxHeap { arr: vec![] }
    }

    pub fn from(mut arr: Vec<i32>) -> Self {
        for i in 0..arr.len() {
            let mut k = i;

            while k > 0 && arr[k] > arr[(k - 1) / 2] {
                (arr[k], arr[(k - 1) / 2]) = (arr[(k - 1) / 2], arr[k]);
                k = (k - 1) / 2;
            }
        }

        MaxHeap { arr }
    }

    fn heapify(&mut self, index: usize) {
        assert!(index < self.arr.len(), "Index out of bounds");

        let mut largest = index;

        if let Some(l) = self.left(index) {
            if self.arr[l] > self.arr[largest] {
                largest = l;
            }
        }

        if let Some(r) = self.right(index) {
            if self.arr[r] > self.arr[largest] {
                largest = r;
            }
        }

        if largest == index {
            return;
        }

        (self.arr[largest], self.arr[index]) = (self.arr[index], self.arr[largest]);
        self.heapify(largest);
    }

    fn left(&self, index: usize) -> Option<usize> {
        if index * 2 + 1 >= self.arr.len() {
            None
        } else {
            Some(index * 2 + 1)
        }
    }

    fn right(&self, index: usize) -> Option<usize> {
        if index * 2 + 2 >= self.arr.len() {
            None
        } else {
            Some(index * 2 + 2)
        }
    }

    pub fn get_max(&self) -> Option<&i32> {
        self.arr.first()
    }

    pub fn modify(&mut self, value: i32, index: usize) {
        self.arr[index] = value;
        self.heapify(index);
    }

    pub fn pop_max(&mut self) -> Option<i32> {
        if self.arr.is_empty() {
            return None;
        }

        let last = self.arr.pop().unwrap();

        if self.arr.is_empty() {
            return Some(last);
        }

        let head = self.arr[0];

        self.modify(last, 0);

        Some(head)
    }

    pub fn into_vec(mut self) -> Vec<i32> {
        let mut v = Vec::with_capacity(self.arr.len());

        while let Some(val) = self.pop_max() {
            v.push(val);
        }

        v
    }
}
