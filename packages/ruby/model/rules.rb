require './model/arena'

class Rules
    attr_reader :max_tick_count
    attr_reader :arena
    attr_reader :team_size
    attr_reader :seed
    attr_reader :ROBOT_MIN_RADIUS
    attr_reader :ROBOT_MAX_RADIUS
    attr_reader :ROBOT_MAX_JUMP_SPEED
    attr_reader :ROBOT_ACCELERATION
    attr_reader :ROBOT_NITRO_ACCELERATION
    attr_reader :ROBOT_MAX_GROUND_SPEED
    attr_reader :ROBOT_ARENA_E
    attr_reader :ROBOT_RADIUS
    attr_reader :ROBOT_MASS
    attr_reader :TICKS_PER_SECOND
    attr_reader :MICROTICKS_PER_TICK
    attr_reader :RESET_TICKS
    attr_reader :BALL_ARENA_E
    attr_reader :BALL_RADIUS
    attr_reader :BALL_MASS
    attr_reader :MIN_HIT_E
    attr_reader :MAX_HIT_E
    attr_reader :MAX_ENTITY_SPEED
    attr_reader :MAX_NITRO_AMOUNT
    attr_reader :START_NITRO_AMOUNT
    attr_reader :NITRO_POINT_VELOCITY_CHANGE
    attr_reader :NITRO_PACK_X
    attr_reader :NITRO_PACK_Y
    attr_reader :NITRO_PACK_Z
    attr_reader :NITRO_PACK_RADIUS
    attr_reader :NITRO_PACK_AMOUNT
    attr_reader :NITRO_PACK_RESPAWN_TICKS
    attr_reader :GRAVITY
    def initialize(json)
        @max_tick_count = json["max_tick_count"]
        @arena = Arena.new(json["arena"])
        @team_size = json["team_size"]
        @seed = json["seed"]
        @ROBOT_MIN_RADIUS = json["ROBOT_MIN_RADIUS"]
        @ROBOT_MAX_RADIUS = json["ROBOT_MAX_RADIUS"]
        @ROBOT_MAX_JUMP_SPEED = json["ROBOT_MAX_JUMP_SPEED"]
        @ROBOT_ACCELERATION = json["ROBOT_ACCELERATION"]
        @ROBOT_NITRO_ACCELERATION = json["ROBOT_NITRO_ACCELERATION"]
        @ROBOT_MAX_GROUND_SPEED = json["ROBOT_MAX_GROUND_SPEED"]
        @ROBOT_ARENA_E = json["ROBOT_ARENA_E"]
        @ROBOT_RADIUS = json["ROBOT_RADIUS"]
        @ROBOT_MASS = json["ROBOT_MASS"]
        @TICKS_PER_SECOND = json["TICKS_PER_SECOND"]
        @MICROTICKS_PER_TICK = json["MICROTICKS_PER_TICK"]
        @RESET_TICKS = json["RESET_TICKS"]
        @BALL_ARENA_E = json["BALL_ARENA_E"]
        @BALL_RADIUS = json["BALL_RADIUS"]
        @BALL_MASS = json["BALL_MASS"]
        @MIN_HIT_E = json["MIN_HIT_E"]
        @MAX_HIT_E = json["MAX_HIT_E"]
        @MAX_ENTITY_SPEED = json["MAX_ENTITY_SPEED"]
        @MAX_NITRO_AMOUNT = json["MAX_NITRO_AMOUNT"]
        @START_NITRO_AMOUNT = json["START_NITRO_AMOUNT"]
        @NITRO_POINT_VELOCITY_CHANGE = json["NITRO_POINT_VELOCITY_CHANGE"]
        @NITRO_PACK_X = json["NITRO_PACK_X"]
        @NITRO_PACK_Y = json["NITRO_PACK_Y"]
        @NITRO_PACK_Z = json["NITRO_PACK_Z"]
        @NITRO_PACK_RADIUS = json["NITRO_PACK_RADIUS"]
        @NITRO_PACK_AMOUNT = json["NITRO_PACK_AMOUNT"]
        @NITRO_PACK_RESPAWN_TICKS = json["NITRO_PACK_RESPAWN_TICKS"]
        @GRAVITY = json["GRAVITY"]
    end
end