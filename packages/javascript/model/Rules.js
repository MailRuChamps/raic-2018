class Rules {
  constructor() {
    this.max_tick_count = 20000;
    this.arena = null;
    this.team_size = 2;
    this.seed = 0;
    this.ROBOT_MIN_RADIUS = 1;
    this.ROBOT_MAX_RADIUS = 1.05;
    this.ROBOT_MAX_JUMP_SPEED = 15;
    this.ROBOT_ACCELERATION = 100;
    this.ROBOT_NITRO_ACCELERATION = 30;
    this.ROBOT_MAX_GROUND_SPEED = 30;
    this.ROBOT_ARENA_E = 0;
    this.ROBOT_RADIUS = 1;
    this.ROBOT_MASS = 2;
    this.TICKS_PER_SECOND = 60;
    this.MICROTICKS_PER_TICK = 100;
    this.RESET_TICKS = 2 * this.TICKS_PER_SECOND;
    this.BALL_ARENA_E = 0.7;
    this.BALL_RADIUS = 2;
    this.BALL_MASS = 1;
    this.MIN_HIT_E = 0.4;
    this.MAX_HIT_E = 0.5;
    this.MAX_ENTITY_SPEED = 100;
    this.MAX_NITRO_AMOUNT = 100;
    this.START_NITRO_AMOUNT = 50;
    this.NITRO_POINT_VELOCITY_CHANGE = 0.6;
    this.NITRO_PACK_X = 20;
    this.NITRO_PACK_Y = 1;
    this.NITRO_PACK_Z = 30;
    this.NITRO_PACK_RADIUS = 0.5;
    this.NITRO_PACK_AMOUNT = 100;
    this.NITRO_PACK_RESPAWN_TICKS = 10 * this.TICKS_PER_SECOND;
    this.GRAVITY = 30;
  }
}
module.exports = Rules;
