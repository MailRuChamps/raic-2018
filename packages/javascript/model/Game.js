const Player = require("./Player");
const Robot = require("./Robot");
const NitroPack = require("./NitroPack");
const Ball = require("./Ball");

class Game {
  constructor({ current_tick, players, robots, nitro_packs, ball }) {
    this.current_tick = current_tick;
    this.players = players.map(player => new Player(player));
    this.robots = robots.map(robot => new Robot(robot));
    this.nitro_packs = nitro_packs.map(nitro_pack => new NitroPack(nitro_pack));
    this.ball = new Ball(ball);
  }
}
module.exports = Game;
