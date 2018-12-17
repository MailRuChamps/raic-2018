use super::*;

#[derive(Debug, Serialize, Deserialize, Clone)]
pub struct Game {
    pub current_tick: i32,
    pub players: Vec<Player>,
    pub robots: Vec<Robot>,
    pub nitro_packs: Vec<NitroPack>,
    pub ball: Ball,
}
