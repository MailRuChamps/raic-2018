#[derive(Debug, Serialize, Deserialize, Clone)]
pub struct NitroPack {
    pub id: i32,
    pub x: f64,
    pub y: f64,
    pub z: f64,
    pub radius: f64,
    pub nitro_amount: f64,
    pub respawn_ticks: Option<i32>,
}
