#[derive(Debug, Serialize, Deserialize, Clone)]
pub struct Player {
    pub id: i32,
    pub me: bool,
    pub strategy_crashed: bool,
    pub score: i32,
}
