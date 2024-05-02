use std::error::Error;

fn main() -> Result<(), Box<dyn Error>> {
    let input_file = "/Users/vincentliu/github repo/Codes/Rust/pdf-parser/assets/W23_-_Schedule_of_classes_January_4.pdf";
    let output_file = "/Users/vincentliu/github repo/Codes/Rust/pdf-parser/assets/all_classes.json";

    pdf_parser::write_to_file(input_file, output_file)?;

    Ok(())
}
