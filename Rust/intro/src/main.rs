use intro::my_fns;

fn main() {
    let mut arr = my_fns::randomize(10);
    println!("{:?}", arr);

    let arr = my_fns::my_quick_sort(&mut arr);
    println!("{:?}", arr);
}
