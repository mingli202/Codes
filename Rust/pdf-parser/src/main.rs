use std::{error::Error, path::Path};

use lopdf::Document;

fn extract_text_from_pdf<P: AsRef<Path>>(pdf_path: P) -> lopdf::Result<()> {
    let doc = Document::load(pdf_path)?;

    let text = doc.extract_text(&[1])?;
    println!("{}", text);
    Ok(())
}

fn main() -> Result<(), Box<dyn Error>> {
    let filename = "/Users/vincentliu/github repo/Codes/Rust/pdf-parser/assets/W23_-_Schedule_of_classes_January_4.pdf";
    extract_text_from_pdf(filename)?;

    Ok(())
}
