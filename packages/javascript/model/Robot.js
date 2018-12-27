/**
 * Created by Quake on 18.12.2018
 */
'use strict';

function Robot() {

}

Robot.prototype.read = function(json) {
    if (typeof json == 'string') {
        json = JSON.parse(json);
    }

    this.id = json.id;
    this.player_id = json.player_id;
    this.is_teammate = json.is_teammate;
    this.x = json.x;
    this.y = json.y;
    this.z = json.z;
    this.velocity_x = json.velocity_x;
    this.velocity_y = json.velocity_y;
    this.velocity_z = json.velocity_z;
    this.radius = json.radius;
    this.nitro_amount = json.nitro_amount;
    this.touch = json.touch;
    if (this.touch) {
        this.touch_normal_x = json.touch_normal_x;
        this.touch_normal_y = json.touch_normal_y;
        this.touch_normal_z = json.touch_normal_z;
    }
}

module.exports = Robot;