class Robot {
  constructor({
    id,
    player_id,
    is_teammate,
    x,
    y,
    z,
    velocity_x,
    velocity_y,
    velocity_z,
    radius,
    nitro_amount,
    touch,
    touch_normal_x,
    touch_normal_y,
    touch_normal_z
  }) {
    this.id = id;
    this.player_id = player_id;
    this.is_teammate = is_teammate;
    this.x = x;
    this.y = y;
    this.z = z;
    this.velocity_x = velocity_x;
    this.velocity_y = velocity_y;
    this.velocity_z = velocity_z;
    this.radius = radius;
    this.nitro_amount = nitro_amount;
    this.touch = touch;
    this.touch_normal_x = touch_normal_x;
    this.touch_normal_y = touch_normal_y;
    this.touch_normal_z = touch_normal_z;
  }
}
module.exports = Robot;
