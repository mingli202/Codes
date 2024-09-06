use crate::Solution;

use std::collections::{HashMap, HashSet};
use std::f64::consts::PI;

struct Position {
    x: i32,
    y: i32,
}

struct Robot {
    direction: i32,
    pos: Position,
    furthest: i32,
}

impl Robot {
    fn new() -> Robot {
        Robot {
            direction: 90,
            pos: Position { x: 0, y: 0 },
            furthest: 0,
        }
    }

    fn walk(&mut self, steps: f64, obs: &HashMap<i32, HashSet<i32>>) {
        let dx = f64::cos(2.0 * PI * self.direction as f64 / 360.0) as i32;
        let dy = f64::sin(2.0 * PI * self.direction as f64 / 360.0) as i32;

        for _ in 0..(steps as i32) {
            self.pos.x += dx;
            self.pos.y += dy;

            if let Some(ys) = obs.get(&self.pos.x) {
                if ys.contains(&self.pos.y) {
                    self.pos.x -= dx;
                    self.pos.y -= dy;
                    return;
                }
            }

            let current = self.dist_from_origin();
            if current > self.furthest {
                self.furthest = current;
            }
        }
    }

    fn dist_from_origin(&self) -> i32 {
        self.pos.x.pow(2) + self.pos.y.pow(2)
    }
}

impl Solution {
    pub fn robot_sim(commands: Vec<i32>, obstacles: Vec<Vec<i32>>) -> i32 {
        let mut robot = Robot::new();

        let mut obs: HashMap<i32, HashSet<i32>> = HashMap::new();

        for o in obstacles {
            obs.entry(o[0]).or_default().insert(o[1]);
        }

        for command in commands.iter() {
            if *command == -2 {
                robot.direction = (robot.direction + 90) % 360;
            } else if *command == -1 {
                robot.direction = (robot.direction - 90) % 360;
            } else {
                robot.walk(*command as f64, &obs);
            }
        }

        robot.furthest
    }
}

#[cfg(test)]
mod tests {
    use crate::Solution;

    #[test]
    fn example_1() {
        assert_eq!(25, Solution::robot_sim([4, -1, 3].to_vec(), [].to_vec()));
    }

    #[test]
    fn example_2() {
        assert_eq!(
            65,
            Solution::robot_sim(vec![4, -1, 4, -2, 4], vec![[2, 4].to_vec()])
        );
    }

    #[test]
    fn example_3() {
        assert_eq!(36, Solution::robot_sim(vec![6, -1, -1, 6], vec![]));
    }
}
