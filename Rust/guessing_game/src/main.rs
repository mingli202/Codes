use rand::Rng;
use std::cmp::Ordering;
use std::io;

fn main() {
    println!("You are playing a guess the number game!");

    let secret_number: u32 = rand::thread_rng().gen_range(1..100);
    println!("The secret number is {secret_number}");

    loop {
        println!("Enter a number between 1 and 100");
        let mut guess: String = String::new();

        io::stdin()
            .read_line(&mut guess)
            .expect("Error reading line");

        let guess: u32 = match guess.trim().parse() {
            Ok(num) => num,
            Err(_) => {
                println!("Please enter a number");
                continue;
            }
        };

        match guess.cmp(&secret_number) {
            Ordering::Equal => break,
            Ordering::Less => println!("Too low!"),
            Ordering::Greater => println!("Too high!"),
        }
    }

    println!("You guessed it right!");
}
