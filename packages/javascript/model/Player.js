/**
 * Created by Quake on 18.12.2018
 */
'use strict';

function Player() {

}

Player.prototype.read = function (json) {
    if (typeof json == 'string') {
        json = JSON.parse(json);
    }
    
    this.id = json.id;
    this.me = json.me;
    this.strategy_crashed = json.strategy_crashed;
    this.score = json.score;
}

module.exports = Player;