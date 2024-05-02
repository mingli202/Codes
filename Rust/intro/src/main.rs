#[allow(unused)]
use intro::my_fns;

use std::error::Error;

fn main() -> Result<(), Box<dyn Error>> {
    let foo = "bar";
    println!("{}", foo);

    Ok(())
}
