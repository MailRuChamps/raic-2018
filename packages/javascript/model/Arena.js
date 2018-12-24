class Arena {
  constructor({
    width,
    height,
    depth,
    bottom_radius,
    top_radius,
    corner_radius,
    goal_top_radius,
    goal_width,
    goal_height,
    goal_depth,
    goal_side_radius
  }) {
    this.width = width;
    this.height = height;
    this.depth = depth;
    this.bottom_radius = bottom_radius;
    this.top_radius = top_radius;
    this.corner_radius = corner_radius;
    this.goal_top_radius = goal_top_radius;
    this.goal_width = goal_width;
    this.goal_height = goal_height;
    this.goal_depth = goal_depth;
    this.goal_side_radius = goal_side_radius;
  }
}
module.exports = Arena;
