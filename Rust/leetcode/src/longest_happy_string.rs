use crate::Solution;

struct MiniHeap {
    arr: Vec<(i32, char)>,
}

impl MiniHeap {
    pub fn new(a: i32, b: i32, c: i32) -> Self {
        let mut arr = Vec::with_capacity(3);

        arr.push((a, 'a'));
        arr.push((b, 'b'));

        if arr[1].0 > arr[0].0 {
            (arr[0], arr[1]) = (arr[1], arr[0]);
        }

        arr.push((c, 'c'));

        if arr[2].0 > arr[0].0 {
            (arr[2], arr[0]) = (arr[0], arr[2]);
        }

        MiniHeap { arr }
    }

    fn heapify(&mut self) {
        let mut largest = 0;

        if self.arr[largest].0 < self.arr[1].0 {
            largest = 1;
        }

        if self.arr[largest].0 < self.arr[2].0 {
            largest = 2;
        }

        if largest != 0 {
            (self.arr[largest], self.arr[0]) = (self.arr[0], self.arr[largest]);
        }
    }

    pub fn decrement(&mut self) {
        self.arr[0].0 -= 1;

        self.heapify();
    }

    pub fn get_max(&mut self) -> char {
        self.arr[0].1
    }

    pub fn get_second_and_decrement(&mut self) -> Option<char> {
        let mut second = 1;

        if self.arr[2].0 > self.arr[second].0 {
            second = 2;
        }

        self.arr[second].0 -= 1;

        if self.arr[second].0 < 0 {
            return None;
        }

        Some(self.arr[second].1)
    }
}

impl Solution {
    pub fn longest_diverse_string(a: i32, b: i32, c: i32) -> String {
        let mut s: Vec<char> = Vec::with_capacity((a + b + c) as usize);

        let mut miniheap = MiniHeap::new(a, b, c);

        let mut last = ' ';
        let mut count = 0;

        for _ in 0..(a + b + c) {
            let mut c = miniheap.get_max();

            if c == last && count == 2 {
                if let Some(_c) = miniheap.get_second_and_decrement() {
                    c = _c;
                    count = 0;
                } else {
                    break;
                }
            } else {
                miniheap.decrement();
                count += 1;
            }
            last = c;

            s.push(c);
        }

        s.iter().collect()
    }
}

#[cfg(test)]
mod test {
    use crate::Solution;

    #[test]
    fn example_1() {
        assert_eq!(
            "ccbccacc".to_string(),
            Solution::longest_diverse_string(1, 1, 7)
        );
    }

    #[test]
    fn example_2() {
        assert_eq!(
            "aabaa".to_string(),
            Solution::longest_diverse_string(7, 1, 0)
        )
    }
}
