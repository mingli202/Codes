#[allow(unused)]
use intro::my_fns;

use std::error::Error;

fn main() -> Result<(), Box<dyn Error>> {
    let v = vec!["Hello", "world", "this", "is", "rust"];

    println!("{:?}", v);

    for i in &v {
        println!("{}", i);
    }

    println!("{:?}", v);

    Ok(())
}
