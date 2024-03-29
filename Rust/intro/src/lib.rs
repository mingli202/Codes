#[allow(dead_code)]
pub mod my_fns {
    use std::{collections::HashMap, io};

    use rand::Rng;

    // Get an array of random ordered numbers from 0 to n - 1
    pub fn sample(min: i32, max: i32, replace: bool) -> Vec<i32> {
        match replace {
            true => (min..=max)
                .map(|_| rand::thread_rng().gen_range(min..=max))
                .collect(),

            false => {
                let mut new_arr: Vec<i32> = Vec::new();
                let mut v: Vec<i32> = (min..=max).collect();
                for _ in min..=max {
                    let random_index = rand::thread_rng().gen_range(0..v.len());
                    new_arr.push(v.remove(random_index))
                }

                new_arr
            }
        }
    }

    pub fn quick_sort(unsorted: &mut [i32]) {
        if unsorted.len() < 2 {
            return;
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
        quick_sort(&mut unsorted[..k]);

        quick_sort(&mut unsorted[k + 1..]);
    }

    pub fn merge_sort(unsorted: &[i32]) -> Vec<i32> {
        if unsorted.len() < 2 {
            return unsorted.to_vec();
        }

        let mid = unsorted.len() / 2;

        // merge individual parts
        let left = &unsorted[..mid];
        let left = merge_sort(left);

        let right = &unsorted[mid..];
        let right = merge_sort(right);

        // merge the two parts
        let mut l_index = 0;
        let mut r_index = 0;
        let mut sorted: Vec<i32> = Vec::new();

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

    pub fn fib(n: u32) -> u128 {
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

    pub fn first_word(s: &str) -> &str {
        let bytes = s.as_bytes();

        for (i, &item) in bytes.iter().enumerate() {
            if item == b' ' {
                return &s[0..i];
            }
        }

        s
    }

    #[derive(Debug)]
    struct Mode {
        values: Vec<u32>,
        count: u32,
    }

    #[derive(Debug)]
    pub struct MedianAndMode {
        median: u32,
        mode: Mode,
    }

    pub fn median_and_mode(mut rand_arr: Vec<u32>) -> MedianAndMode {
        rand_arr.sort();

        let mid = rand_arr.len() / 2 - 1;

        let median = match mid % 2 {
            0 => (rand_arr[mid] + rand_arr[mid + 1]) / 2,
            _ => rand_arr[mid],
        };

        let mut table: HashMap<u32, u32> = HashMap::new();

        for i in rand_arr.iter() {
            let entry = table.entry(*i).or_insert(0);
            *entry += 1;
        }

        let mut mode: Vec<u32> = Vec::new();
        let mut max = 0;
        for (k, v) in table.iter() {
            if *v > max {
                max = *v;
                mode = vec![*k];
            } else if *v == max {
                mode.push(*k);
            }
        }

        MedianAndMode {
            median,
            mode: Mode {
                values: mode,
                count: max,
            },
        }
    }

    pub fn office_simulation() {
        let mut employees: HashMap<&str, Vec<String>> = HashMap::from([
            ("Engineering", vec![]),
            ("Marketing", vec![]),
            ("HR", vec![]),
            ("IT", vec![]),
        ]);

        loop {
            let mut command = String::new();

            println!("\nEnter command (e.g. help): ");
            io::stdin()
                .read_line(&mut command)
                .expect("Error reading line");

            let words: Vec<&str> = command.split(' ').map(|w| w.trim()).collect();

            match words[0] {
                "add" | "a" | "remove" | "rm" => {
                    if words.len() != 3 {
                        println!("Invalid command");
                        continue;
                    }

                    let name = words[1];
                    let department = words[2];

                    match employees.get_mut(department) {
                        Some(v) => match words[0] {
                            "add" | "a" => {
                                v.push(name.to_string());
                                println!("Added {} to {}!", name, department)
                            }
                            _ => {
                                let mut found = false;
                                for (i, person) in v.iter().enumerate() {
                                    if person == name {
                                        v.remove(i);
                                        found = true;
                                        println!("Removed {} from {}!", name, department);
                                        break;
                                    }
                                }
                                if found == false {
                                    println!("Could not find {} in {}", name, department);
                                }
                            }
                        },
                        None => println!("Department does not exist"),
                    };
                }
                "list" | "ls" => {
                    if words.len() != 2 {
                        println!("Invalid command");
                        continue;
                    }
                    let department = words[1];

                    if department == "all" {
                        for (k, v) in employees.iter() {
                            println!("{}: {:?}", k, v);
                        }
                        continue;
                    }

                    match employees.get(department) {
                        Some(v) => println!("{}: {:?}", department, v),
                        None => println!("Department does not exist"),
                    }
                }
                "help" | "h" => {
                    println!("add <name> <department>");
                    println!("remove <name> <department>");
                    println!("list [<department> | all]");
                    println!("help");
                    println!("quit");
                }
                "quit" | "q" => break,
                _ => println!("Invalid command"),
            }
        }
    }
}
