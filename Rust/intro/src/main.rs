use std::collections::HashSet;

#[allow(unused)]
use intro::*;

use tokio::task::JoinSet;

#[tokio::main]
async fn main() {
    let ignore_words: HashSet<&str> = HashSet::from([]);
    let all_words = vec![
        "asdfwer",
        "asdfasdf",
        "qweroiu234",
        "asdfasdf",
        "qweroiu234",
        "asdfasdf",
        "qweroiu234",
        "asdfasdf",
        "qweroiu234",
        "asdfasdf",
        "qweroiu234",
        "asdfasdf",
        "qweroiu234",
        "asdfasdf",
        "qweroiu234",
        "asdfasdf",
        "qweroiu234",
        "asdfasdf",
        "qweroiu234",
        "asdfasdf",
        "qweroiu234",
        "asdfasdf",
        "qweroiu234",
        "asdfasdf",
        "qweroiu234",
        "asdfasdf",
        "qweroiu234",
        "asdfasdf",
        "qweroiu234",
        "asdfasdf",
        "qweroiu234",
        "asdfasdf",
        "qweroiu234",
        "asdfasdf",
        "qweroiu234",
        "asdfasdf",
        "qweroiu234",
        "asdfasdf",
        "qweroiu234",
        "asdfasdf",
        "qweroiu234",
        "asdfasdf",
        "qweroiu234",
        "asdfasdf",
        "qweroiu234",
        "asdfasdf",
        "qweroiu234",
        "asdfasdf",
        "qweroiu234",
        "asdfasdf",
        "qweroiu234",
        "asdfasdf",
        "qweroiu234",
        "asdfasdf",
        "qweroiu234",
        "asdfasdf",
        "qweroiu234",
        "asdfasdf",
        "qweroiu234",
        "asdfasdf",
        "qweroiu234",
        "asdfasdf",
        "qweroiu234",
        "asdfasdf",
        "qweroiu234",
        "asdfasdf",
        "qweroiu234",
        "asdfasdf",
        "qweroiu234",
        "asdfasdf",
        "qweroiu234",
    ];

    println!("All words: {:#?}", all_words.len());

    let mut set = JoinSet::new();

    for word in all_words {
        if ignore_words.contains(word) {
            continue;
        }

        // println!("Word: {:#?}", word);

        set.spawn(async move {
            let mut res = defs::exists(word.to_string()).await;

            while res.is_none() {
                // println!("some problem");
                // tokio::time::sleep(tokio::time::Duration::from_secs(1)).await;
                res = defs::exists(word.to_string()).await;
            }

            if let Some(false) = res {
                Some(word)
            } else {
                None
            }
        });
    }

    let errors = set
        .join_all()
        .await
        .into_iter()
        .flatten()
        .collect::<Vec<_>>();

    println!("Errors: {:#?}", errors);
    println!("Errors len: {:#?}", errors.len());
    println!("Errors len actully: {:#?}", errors.len());
}

#[allow(unused, non_snake_case)]
mod defs {
    use serde::Deserialize;

    #[derive(Deserialize, Debug)]
    struct NotFound {
        title: String,
        message: String,
        resolution: String,
    }

    pub enum Error {
        NotFound,
        RateLimit,
    }

    pub async fn exists(word: String) -> Option<bool> {
        let re = reqwest::get(format!(
            "https://api.dictionaryapi.dev/api/v2/entries/en/{}",
            word
        ))
        .await;

        if re.is_err() {
            return None;
        }

        let re = re.unwrap().text().await;

        if re.is_err() {
            return None;
        }

        let txt = re.unwrap();

        if txt.contains("1015") {
            return None;
        }

        match serde_json::from_str::<NotFound>(&txt) {
            Ok(_) => Some(false),
            Err(_) => Some(true),
        }
    }
}
