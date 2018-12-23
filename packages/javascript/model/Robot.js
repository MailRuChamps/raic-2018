class Robot {
  constructor() {
    this.id = 0;
    this.player_id = 0;
    this.is_teammate = false;
    this.x = 0;
    this.y = 0;
    this.z = 0;
    this.velocity_x = 0;
    this.velocity_y = 0;
    this.velocity_z = 0;
    this.radius = 0;
    this.nitro_amount = 0;
    this.touch = false;
    this.touch_normal_x = null;
    this.touch_normal_y = null;
    this.touch_normal_z = null;
  }
}
module.exports = Robot;
