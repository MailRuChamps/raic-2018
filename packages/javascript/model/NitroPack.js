/**
 * Created by Quake on 18.12.2018
 */
'use strict';

function NitroPack() {

}

NitroPack.prototype.read = function(json) {
    if (typeof json == 'string') {
        json = JSON.parse(json);
    }

    this.id = json.id;
    this.x = json.x;
    this.y = json.y;
    this.z = json.z;
    this.radius = json.radius;
    this.alive = json.respawn_ticks === null;
    if (!this.alive) {
        this.respawn_ticks = json.respawn_ticks;
    }
}

module.exports = NitroPack;