const net = require("net");
const MyStrategy = require("./MyStrategy");
const Action = require("./model/Action");

const config = {
  host: process.argv[2] || "127.0.0.1",
  port: Number(process.argv[3]) || 31001
};
const token = process.argv[4] || "0000000000000000";

const runner = net.createConnection(config).on("connect", () => {
  const DELIMITER_CHAR = "\n";

  runner.write("json");
  runner.write(DELIMITER_CHAR);
  runner.write(token);
  runner.write(DELIMITER_CHAR);

  let message = [];

  runner.once("data", data => {
    const strategy = new MyStrategy();
    const lines = data.toString().split(DELIMITER_CHAR);
    const rules = JSON.parse(lines[0]);

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
        const game = JSON.parse(input);
        const actions = {};
        for (const robot of game.robots) {
          if (robot.is_teammate) {
            const action = new Action();
            strategy.act(robot, rules, game, action);
            actions[robot.id] = action;
          }
        }
        runner.write(JSON.stringify(actions));
        runner.write("|");
        runner.write(strategy.customRendering());
        runner.write(DELIMITER_CHAR);
        runner.write("<end>");
        runner.write(DELIMITER_CHAR);
      }
    }

    runner.on("data", data => {
      handler(data.toString());
    });
  });
});
