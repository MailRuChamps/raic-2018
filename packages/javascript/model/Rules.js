/**
 * Created by Quake on 18.12.2018
 */
'use strict';

let Arena = require('./Arena')
    ;

function Rules() {
    this.arena = new Arena();
}

Rules.prototype.read = function(json) {
    if (typeof json == 'string') {
        json = JSON.parse(json);
    }

    this.max_tick_count = json.max_tick_count;
    this.arena.read(json.arena);
    this.team_size = json.team_size;
    this.seed = json.seed;
    this.ROBOT_MIN_RADIUS = json.ROBOT_MIN_RADIUS;
    this.ROBOT_MAX_RADIUS = json.ROBOT_MAX_RADIUS;
    this.ROBOT_MAX_JUMP_SPEED = json.ROBOT_MAX_JUMP_SPEED;
    this.ROBOT_ACCELERATION = json.ROBOT_ACCELERATION;
    this.ROBOT_NITRO_ACCELERATION = json.ROBOT_NITRO_ACCELERATION;
    this.ROBOT_MAX_GROUND_SPEED = json.ROBOT_MAX_GROUND_SPEED;
    this.ROBOT_ARENA_E = json.ROBOT_ARENA_E;
    this.ROBOT_RADIUS = json.ROBOT_RADIUS;
    this.ROBOT_MASS = json.ROBOT_MASS;
    this.TICKS_PER_SECOND = json.TICKS_PER_SECOND;
    this.MICROTICKS_PER_TICK = json.MICROTICKS_PER_TICK;
    this.RESET_TICKS = json.RESET_TICKS;
    this.BALL_ARENA_E = json.BALL_ARENA_E;
    this.BALL_RADIUS = json.BALL_RADIUS;
    this.BALL_MASS = json.BALL_MASS;
    this.MIN_HIT_E = json.MIN_HIT_E;
    this.MAX_HIT_E = json.MAX_HIT_E;
    this.MAX_ENTITY_SPEED = json.MAX_ENTITY_SPEED;
    this.MAX_NITRO_AMOUNT = json.MAX_NITRO_AMOUNT;
    this.START_NITRO_AMOUNT = json.START_NITRO_AMOUNT;
    this.NITRO_POINT_VELOCITY_CHANGE = json.NITRO_POINT_VELOCITY_CHANGE;
    this.NITRO_PACK_X = json.NITRO_PACK_X;
    this.NITRO_PACK_Y = json.NITRO_PACK_Y;
    this.NITRO_PACK_Z = json.NITRO_PACK_Z;
    this.NITRO_PACK_RADIUS = json.NITRO_PACK_RADIUS;
    this.NITRO_PACK_AMOUNT = json.NITRO_PACK_AMOUNT;
    this.NITRO_PACK_RESPAWN_TICKS = json.NITRO_PACK_RESPAWN_TICKS;
    this.GRAVITY = json.GRAVITY;
}

module.exports = Rules;