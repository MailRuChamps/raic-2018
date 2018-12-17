#[derive(Debug, Serialize, Deserialize, Clone)]
pub struct Arena {
    pub width: f64,
    pub height: f64,
    pub depth: f64,
    pub bottom_radius: f64,
    pub top_radius: f64,
    pub corner_radius: f64,
    pub goal_top_radius: f64,
    pub goal_width: f64,
    pub goal_height: f64,
    pub goal_depth: f64,
    pub goal_side_radius: f64,
}
