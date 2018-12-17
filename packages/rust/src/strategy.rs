use model::*;

pub trait Strategy {
    fn act(&mut self, me: &Robot, rules: &Rules, game: &Game, action: &mut Action);
}
