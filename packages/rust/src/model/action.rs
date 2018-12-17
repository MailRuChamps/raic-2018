#[derive(Debug, Serialize, Deserialize, Copy, Clone)]
pub struct Action {
    pub target_velocity_x: f64,
    pub target_velocity_y: f64,
    pub target_velocity_z: f64,
    pub jump_speed: f64,
    pub use_nitro: bool,
}

impl Default for Action {
    fn default() -> Self {
        Self {
            target_velocity_x: 0.0,
            target_velocity_y: 0.0,
            target_velocity_z: 0.0,
            jump_speed: 0.0,
            use_nitro: false,
        }
    }
}
