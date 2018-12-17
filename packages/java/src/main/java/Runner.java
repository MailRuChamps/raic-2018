import model.*;

import java.io.IOException;
import java.util.HashMap;
import java.util.Map;

public final class Runner {
    private final RemoteProcessClient remoteProcessClient;
    private final String token;

    public static void main(String[] args) throws IOException {
        new Runner(args.length == 3 ? args : new String[] { "127.0.0.1", "31001", "0000000000000000" }).run();
    }

    private Runner(String[] args) throws IOException {
        remoteProcessClient = new RemoteProcessClient(args[0], Integer.parseInt(args[1]));
        token = args[2];
    }

    public void run() throws IOException {
        Strategy strategy = new MyStrategy();
        Map<Integer, Action> actions = new HashMap<>();
        Game game;
        remoteProcessClient.writeToken(token);
        Rules rules = remoteProcessClient.readRules();
        while ((game = remoteProcessClient.readGame()) != null) {
            actions.clear();
            for (Robot robot : game.robots) {
                if (robot.is_teammate) {
                    Action action = new Action();
                    strategy.act(robot, rules, game, action);
                    actions.put(robot.id, action);
                }
            }
            remoteProcessClient.write(actions);
        }
    }
}
