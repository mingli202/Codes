fn find_complement(num: i32) -> i32 {
    let l = format!("{:b}", num).len();
    let mask = (1 << l) - 1;

    num ^ mask
}

pub fn test() {
    assert_eq!(find_complement(5), 2);
}
