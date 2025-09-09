use std::error::Error;

use lopdf::Document;

fn main() -> Result<(), Box<dyn Error>> {
    let curricula_path = "/Users/vincentliu/Downloads/schedule_of_classes_june_6.pdf";
    let document = Document::load(curricula_path)?;

    let pages = document.get_pages().into_iter().take(1).map(
        |(page_number, _page_id)| -> Result<Vec<String>, Box<dyn Error>> {
            let text = document.extract_text(&[page_number])?;

            Ok(text.split('\n').map(|s| s.trim_end().to_string()).collect())
        },
    );

    for page in pages.rev().flatten() {
        println!("{:?}", page);
    }

    Ok(())
}
