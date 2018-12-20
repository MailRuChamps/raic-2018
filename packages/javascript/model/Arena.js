/**
 * Created by Quake on 18.12.2018
 */
'use strict';

function Arena() {

}

Arena.prototype.read = function(json) {
    if (typeof json == 'string') {
        json = JSON.parse(json);
    }

    this.width = json.width;
    this.height = json.height;
    this.depth = json.depth;
    this.bottom_radius = json.bottom_radius;
    this.top_radius = json.top_radius;
    this.corner_radius = json.corner_radius;
    this.goal_top_radius = json.goal_top_radius;
    this.goal_width = json.goal_width;
    this.goal_height = json.goal_height;
    this.goal_depth = json.goal_depth;
    this.goal_side_radius = json.goal_side_radius;
}

module.exports = Arena;