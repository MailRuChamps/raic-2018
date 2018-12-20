/**
 * Created by Quake on 18.12.2018
 */
'use strict';

let Player = require('./Player')
    , Arena = require('./Arena')
    , Robot = require('./Robot')
    , NitroPack = require('./NitroPack')
    , Ball = require('./Ball')
    ;

function Game() {

}

Game.prototype.read = function(json) {
    if (typeof json == 'string') {
        json = JSON.parse(json);
    }

    this.current_tick = json.current_tick;
    this.players = this.players || [];
    this.players = this.players.slice(0, json.players.length - 1);

    for (let i = 0; i < json.players.length; ++i) {
        if (!this.players[i]) {
            this.players[i] = new Player();
        }
        this.players[i].read(json.players[i]);
    }

    this.robots = this.robots || [];
    this.robots = this.robots.slice(0, json.robots.length - 1);

    for (let i = 0; i < json.robots.length; ++i) {
        if (!this.robots[i]) {
            this.robots[i] = new Robot();
        }
        this.robots[i].read(json.robots[i]);
    }

    this.nitro_packs = this.nitro_packs || [];
    this.nitro_packs = this.nitro_packs.slice(0, json.nitro_packs.length - 1);

    for (let i = 0; i < json.nitro_packs.length; ++i) {
        if (!this.nitro_packs[i]) {
            this.nitro_packs[i] = new NitroPack();
        }
        this.nitro_packs[i].read(json.nitro_packs[i]);
    }

    this.ball = this.ball || new Ball();
    this.ball.read(json.ball);
}

module.exports = Game;