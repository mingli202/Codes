pub fn _fib(n: u32) -> u128 {
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
pub fn _first_word(s: &str) -> &str {
    let bytes = s.as_bytes();

    for (i, &item) in bytes.iter().enumerate() {
        if item == b' ' {
            return &s[0..i];
        }
    }

    s
}
