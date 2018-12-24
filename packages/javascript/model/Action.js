class Action {
  constructor({
    target_velocity_x,
    target_velocity_y,
    target_velocity_z,
    jump_speed,
    use_nitro
  }) {
    this.target_velocity_x = target_velocity_x;
    this.target_velocity_y = target_velocity_y;
    this.target_velocity_z = target_velocity_z;
    this.jump_speed = jump_speed;
    this.use_nitro = use_nitro;
  }
}
module.exports = Action;
