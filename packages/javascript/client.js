const net = require("net");
const MyStrategy = require("./MyStrategy");
const Action = require("./model/Action");
const Rules = require("./model/Rules");
const Game = require("./model/Game");

const config = {
  host: process.argv[2] || "127.0.0.1",
  port: Number(process.argv[3]) || 31001
};
const token = process.argv[4] || "0000000000000000";

const runner = net.createConnection(config).on("connect", () => {
  const DELIMITER_CHAR = "\n";
  const DELIMITER_END = `${DELIMITER_CHAR}<end>${DELIMITER_CHAR}`;

  runner.write(`json${DELIMITER_CHAR}${token}${DELIMITER_CHAR}`);

  let message = [];

  runner.once("data", data => {
    const strategy = new MyStrategy();
    const lines = data.toString().split(DELIMITER_CHAR);
    const rules = new Rules(JSON.parse(lines[0]));

    handler(lines.slice(1).join(DELIMITER_CHAR));

    function handler(input) {
      if (input) {
        for (const line of input.split(DELIMITER_CHAR)) {
          if (line) {
            message.push(line);
          } else {
            const text = message.join("");
            message = [];
            update(text);
          }
        }
      }
    }

    function update(input) {
      if (input) {
        const game = new Game(JSON.parse(input));

        const actions = {};
        for (const robot of game.robots) {
          if (robot.is_teammate) {
            const action = new Action({
              target_velocity_x: 0,
              target_velocity_y: 0,
              target_velocity_z: 0,
              jump_speed: 0,
              use_nitro: false
            });
            strategy.act(robot, rules, game, action);
            actions[robot.id] = action;
          }
        }
        runner.write(JSON.stringify(actions));
        runner.write("|");
        runner.write(strategy.customRendering());
        runner.write(DELIMITER_END);
      }
    }

    runner.on("data", data => {
      handler(data.toString());
    });
  });
});
