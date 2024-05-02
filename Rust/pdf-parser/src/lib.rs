use lopdf::Document;
use regex::Regex;
use serde::{Deserialize, Serialize};
use std::{collections::HashMap, error::Error, fs, path::Path};

#[derive(Serialize, Deserialize)]
struct ClassInfo {
    prof: String,
    time: HashMap<String, String>,
}

impl ClassInfo {
    fn new(prof: String, time: HashMap<String, String>) -> Self {
        Self { prof, time }
    }
}

#[derive(Serialize, Deserialize)]
struct Class {
    course: String,
    code: String,
    code_header: Option<String>,
    title: String,
    section: String,
    lecture: ClassInfo,
    lab: ClassInfo,
    more: Option<String>,
    viewdata: HashMap<String, [i8; 2]>,
}

impl Class {
    fn new() -> Self {
        Self {
            course: String::new(),
            code: String::new(),
            code_header: None,
            title: String::new(),
            section: String::new(),
            lecture: ClassInfo::new(String::new(), HashMap::new()),
            lab: ClassInfo::new(String::new(), HashMap::new()),
            more: None,
            viewdata: HashMap::new(),
        }
    }
}

struct ClassRe {
    section: Regex,
    code: Regex,
    lecture: String,
    lab: String,
}

fn parse_pdf<P: AsRef<Path>>(input_file: P) -> lopdf::Result<HashMap<u32, Class>> {
    let unwanted = [
        "",
        "SCHEDULE OF CLASSES - WINTER 2023",
        "DAY/TIMES",
        "COURSE TITLE/TEACHER",
        "COURSE NUMBER",
        "DISC",
        "SECTION",
        "John Abbott College",
    ];

    let doc = Document::load(input_file)?;
    let mut all_classes = HashMap::new();

    let re: ClassRe = ClassRe {
        section: Regex::new(r"\d{5}").unwrap(),
        code: Regex::new(r"\d{3}-\w{3}-\d{2}").unwrap(),
        lab: "Lab".to_string(),
        lecture: "Lecture".to_string(),
    };

    for i in 1..=1 {
        let text = doc.extract_text(&[i])?;

        let mut class = Class::new();

        let mut content: Vec<&str> = text.split('\n').filter(|t| !unwanted.contains(t)).collect();
        class.course = content.pop().unwrap().to_string();
        content.reverse();
        content.pop();

        populate_class(content, &mut all_classes, &re);
    }

    Ok(all_classes)
}

fn populate_class(content: Vec<&str>, all_classes: &mut HashMap<u32, Class>, re: &ClassRe) {
    let mut class = Class::new();
    let mut class_id: u32 = 0;

    let mut it = content.iter();

    while let Some(word) = it.next() {
        if re.section.is_match(word) {
            all_classes.insert(class_id, class);
            class_id += 1;

            class = Class::new();
            class.section = word.to_string();
        } else if re.code.is_match(word) {
            class.code = word.to_string();
            class.title = word.to_string();
        } else if word.to_string() == re.lecture {
            class.lecture.prof = it.next().unwrap().to_string();
        } else if word.to_string() == re.lab {
            class.lab.prof = it.next().unwrap().to_string();
        }
    }
}

pub fn write_to_file(input_file: &str, output_file: &str) -> Result<(), Box<dyn Error>> {
    let text = parse_pdf(input_file)?;
    let text = serde_json::to_string_pretty(&text)?;

    fs::write(output_file, text)?;

    Ok(())
}
