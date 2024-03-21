#[allow(dead_code)]
pub mod my_fns {
    use rand::Rng;

    pub fn randomize(n: u32) -> Vec<u32> {
        let mut v: Vec<u32> = (0..n).collect();
        let mut new_arr: Vec<u32> = Vec::new();

        for _ in 0..v.len() {
            let random_index = rand::thread_rng().gen_range(0..v.len());
            new_arr.push(v.remove(random_index));
        }

        new_arr
    }

    pub fn my_quick_sort(unsorted: &mut [u32]) -> Vec<u32> {
        if unsorted.len() < 2 {
            return unsorted.to_vec();
        }

        let pivot = &unsorted.last().unwrap().to_owned();

        let mut k = 0;

        // swap elements less than pivot to the left
        for i in 0..unsorted.len() {
            if unsorted.get(i).unwrap() < pivot {
                unsorted.swap(k, i);
                k += 1;
            }
        }

        // put the pivot in its place
        unsorted.swap(k, unsorted.len() - 1);

        // sort the two parts
        let left = my_quick_sort(&mut unsorted[..k]);

        let right = my_quick_sort(&mut unsorted[k + 1..]);

        // merge them
        [&left[..], &[unsorted[k]], &right[..]].concat()
    }

    pub fn my_merge_sort(unsorted: &[u32]) -> Vec<u32> {
        if unsorted.len() < 2 {
            return unsorted.to_vec();
        }

        let mid = unsorted.len() / 2;

        // merge individual parts
        let left = &unsorted[..mid];
        let left = my_merge_sort(left);

        let right = &unsorted[mid..];
        let right = my_merge_sort(right);

        // merge the two parts
        let mut l_index = 0;
        let mut r_index = 0;
        let mut sorted: Vec<u32> = Vec::new();

        while l_index < left.len() && r_index < right.len() {
            if left[l_index] < right[r_index] {
                sorted.push(left[l_index]);
                l_index += 1;
            } else {
                sorted.push(right[r_index]);
                r_index += 1;
            }
        }

        // check for remaining elements
        while l_index < left.len() {
            sorted.push(left[l_index]);
            l_index += 1;
        }
        while r_index < right.len() {
            sorted.push(right[r_index]);
            r_index += 1;
        }

        sorted
    }

    fn fib(n: u32) -> u128 {
        let mut a = 0;
        let mut b = 1;
        let mut c = 1;

        for i in 1..n {
            let res = u128::overflowing_add(a, b);

            if res.1 == true {
                println!("overflowed at {i}");
                break;
            }

            c = res.0;
            a = b;
            b = c;
        }

        c
    }

    fn first_word(s: &str) -> &str {
        let bytes = s.as_bytes();

        for (i, &item) in bytes.iter().enumerate() {
            if item == b' ' {
                return &s[0..i];
            }
        }

        s
    }
}
