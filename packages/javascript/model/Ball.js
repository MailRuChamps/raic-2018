/**
 * Created by Quake on 18.12.2018
 */
'use strict';

function Ball() {

}

Ball.prototype.read = function(json) {
    if (typeof json == 'string') {
        json = JSON.parse(json);
    }

    this.x = json.x;
    this.y = json.y;
    this.z = json.z;
    this.velocity_x = json.velocity_x;
    this.velocity_y = json.velocity_y;
    this.velocity_z = json.velocity_z;
    this.radius = json.radius;
}

module.exports = Ball;