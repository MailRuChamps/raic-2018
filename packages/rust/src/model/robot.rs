#[derive(Debug, Serialize, Deserialize, Clone)]
pub struct Robot {
    pub id: i32,
    pub player_id: i32,
    pub is_teammate: bool,
    pub x: f64,
    pub y: f64,
    pub z: f64,
    pub velocity_x: f64,
    pub velocity_y: f64,
    pub velocity_z: f64,
    pub radius: f64,
    pub nitro_amount: f64,
    pub touch: bool,
    pub touch_normal_x: Option<f64>,
    pub touch_normal_y: Option<f64>,
    pub touch_normal_z: Option<f64>,
}
