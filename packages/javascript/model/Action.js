/**
 * Created by Quake on 18.12.2018
 */
'use strict';

function Action() {
    this.target_velocity_x = 0;
    this.target_velocity_y = 0;
    this.target_velocity_z = 0;
    this.jump_speed = 0;
    this.use_nitro = false;
};

Action.prototype.toJson = function() {
    return JSON.stringify({
        target_velocity_x: this.target_velocity_x,
        target_velocity_y: this.target_velocity_y,
        target_velocity_z: this.target_velocity_z,
        jump_speed: this.jump_speed,
        use_nitro: this.use_nitro
    });
}

Action.prototype.toObject = function() {
    return {
        target_velocity_x: this.target_velocity_x,
        target_velocity_y: this.target_velocity_y,
        target_velocity_z: this.target_velocity_z,
        jump_speed: this.jump_speed,
        use_nitro: this.use_nitro
    };
}

module.exports = Action;