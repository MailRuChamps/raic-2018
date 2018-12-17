import model.*

import java.io.IOException
import java.util.HashMap

class Runner @Throws(IOException::class)
private constructor(args: Array<String>) {
    private val remoteProcessClient: RemoteProcessClient
    private val token: String

    init {
        remoteProcessClient = RemoteProcessClient(args[0], Integer.parseInt(args[1]))
        token = args[2]
    }

    @Throws(IOException::class)
    fun run() {
        val strategy = MyStrategy()
        val actions = HashMap<Int, Action>()
        var game: Game?
        remoteProcessClient.writeToken(token)
        val rules = remoteProcessClient.readRules()
        while (true) {
            game = remoteProcessClient.readGame()
            if (game == null) {
                break
            }
            actions.clear()
            for (robot in game.robots) {
                if (robot.is_teammate) {
                    val action = Action()
                    strategy.act(robot, rules, game, action)
                    actions[robot.id] = action
                }
            }
            remoteProcessClient.write(actions)
        }
    }

    companion object {

        @Throws(IOException::class)
        @JvmStatic
        fun main(args: Array<String>) {
            Runner(if (args.size == 3) args else arrayOf("127.0.0.1", "31001", "0000000000000000")).run()
        }
    }
}
