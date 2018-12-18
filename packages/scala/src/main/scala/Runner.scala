import java.util

import model.Action
import model.Game

object Runner extends App {

    val remoteProcessClient = initClient(args)

    val strategy = new MyStrategy()
    val actions: util.Map[Integer, Action] = new util.HashMap[Integer, Action]
    val rules = remoteProcessClient.readRules()
    var gameOpt: Option[Game] = Option(remoteProcessClient.readGame())

    while (gameOpt.nonEmpty) {
        for {
            game <- gameOpt
            _ = actions.clear()
            robot <- game.robots if robot.is_teammate
        } {
            val action = new Action()
            strategy.act(robot, rules, game, action)
            actions.put(robot.id, action)
        }
        remoteProcessClient.write(actions)
        gameOpt = Option(remoteProcessClient.readGame())
    }

    def initClient(args: Array[String]): RemoteProcessClient = {
        val argz = if (args.length == 3) {
            args
        } else {
            Array("127.0.0.1", "31001", "0000000000000000")
        }
        val remoteProcessClient = new RemoteProcessClient(argz(0), Integer.parseInt(argz(1)))
        val token = argz(2)
        remoteProcessClient.writeToken(token)
        remoteProcessClient
    }
}