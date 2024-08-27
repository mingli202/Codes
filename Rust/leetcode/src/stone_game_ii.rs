enum Turn {
    Alice,
    Bob,
}

fn traverse(m: i32, start: i32, piles: &Vec<i32>, alice: &mut i32, bob: &mut i32, turn: Turn) {
    for i in m..=(2 * m) {
        let _start = start as usize;
        let _m = m as usize;

        if _start + _m >= piles.len() {
            return;
        }

        let s: i32 = piles[_start.._start + _m].iter().sum();

        match turn {
            Turn::Alice => {
                *alice += s;
                traverse(m, start + i, piles, alice, bob, Turn::Bob);
            }
            Turn::Bob => {
                *bob += s;
                traverse(m, start, piles, alice, bob, Turn::Alice);
            }
        }
    }
}

pub fn stone_game_ii(piles: Vec<i32>) -> i32 {
    let m: i32 = 1;
    let mut alice = -1;
    let mut bob = -1;

    traverse(m, 0, &piles, &mut alice, &mut bob, Turn::Alice);

    alice
}

pub fn test() {
    assert_eq!(stone_game_ii(vec![2, 7, 9, 4, 4]), 10);
    assert_eq!(stone_game_ii(vec![1, 2, 3, 4, 5, 100]), 104);
}
