class Player {
  constructor({ id, me, strategy_crashed, score }) {
    this.id = id;
    this.me = me;
    this.strategy_crashed = strategy_crashed;
    this.score = score;
  }
}
module.exports = Player;
