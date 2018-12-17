#[derive(Debug, Serialize, Deserialize, Clone)]
pub struct Ball {
    pub x: f64,
    pub y: f64,
    pub z: f64,
    pub velocity_x: f64,
    pub velocity_y: f64,
    pub velocity_z: f64,
    pub radius: f64,
}
