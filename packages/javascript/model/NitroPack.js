class NitroPack {
  constructor({ id, x, y, z, radius, nitro_amount, respawn_ticks }) {
    this.id = id;
    this.x = x;
    this.y = y;
    this.z = z;
    this.radius = radius;
    this.nitro_amount = nitro_amount;
    this.respawn_ticks = respawn_ticks;
  }
}
module.exports = NitroPack;
