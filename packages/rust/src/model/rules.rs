use super::*;

#[derive(Debug, Serialize, Deserialize, Clone)]
pub struct Rules {
    pub max_tick_count: i32,
    pub arena: Arena,
}
