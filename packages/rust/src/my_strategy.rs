use model::*;
use strategy::Strategy;

pub struct MyStrategy;

impl Default for MyStrategy {
    fn default() -> Self {
        Self {}
    }
}

impl Strategy for MyStrategy {
    fn act(&mut self, me: &Robot, rules: &Rules, game: &Game, action: &mut Action) {}
}
