package model

class Rules {
    var max_tick_count: Int = 0
    lateinit var arena: Arena
    var team_size: Int = 0
    var seed: Long = 0
    var ROBOT_MIN_RADIUS: Double = 0.0
    var ROBOT_MAX_RADIUS: Double = 0.0
    var ROBOT_MAX_JUMP_SPEED: Double = 0.0
    var ROBOT_ACCELERATION: Double = 0.0
    var ROBOT_NITRO_ACCELERATION: Double = 0.0
    var ROBOT_MAX_GROUND_SPEED: Double = 0.0
    var ROBOT_ARENA_E: Double = 0.0
    var ROBOT_RADIUS: Double = 0.0
    var ROBOT_MASS: Double = 0.0
    var TICKS_PER_SECOND: Int = 0
    var MICROTICKS_PER_TICK: Int = 0
    var RESET_TICKS: Int = 0
    var BALL_ARENA_E: Double = 0.0
    var BALL_RADIUS: Double = 0.0
    var BALL_MASS: Double = 0.0
    var MIN_HIT_E: Double = 0.0
    var MAX_HIT_E: Double = 0.0
    var MAX_ENTITY_SPEED: Double = 0.0
    var MAX_NITRO_AMOUNT: Double = 0.0
    var START_NITRO_AMOUNT: Double = 0.0
    var NITRO_POINT_VELOCITY_CHANGE: Double = 0.0
    var NITRO_PACK_X: Double = 0.0
    var NITRO_PACK_Y: Double = 0.0
    var NITRO_PACK_Z: Double = 0.0
    var NITRO_PACK_RADIUS: Double = 0.0
    var NITRO_PACK_AMOUNT: Double = 0.0
    var NITRO_PACK_RESPAWN_TICKS: Int = 0
    var GRAVITY: Double = 0.0
}
