use std::env;

fn main() {
    let text = parse_args(env::args());
    println!("{}", text);
}

fn parse_args(mut args: impl Iterator<Item = String>) -> String {
    args.next();

    args.collect::<Vec<String>>().join(" ")
}
