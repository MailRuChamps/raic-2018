class Ball {
  constructor({ x, y, z, velocity_x, velocity_y, velocity_z, radius }) {
    this.x = x;
    this.y = y;
    this.z = z;
    this.velocity_x = velocity_x;
    this.velocity_y = velocity_y;
    this.velocity_z = velocity_z;
    this.radius = radius;
  }
}
module.exports = Ball;
