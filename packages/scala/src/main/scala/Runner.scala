import model._

object Runner extends App {
  override def main(args : Array[String]): Unit = {
    val newArgs = if (args.length == 3) args else Array( "127.0.0.1", "31001", "0000000000000000" )
    new Runner(newArgs).run()
  }
}

class Runner(args : Array[String])  {
  val remoteProcessClient = new RemoteProcessClient(args(0), args(1).toInt)
  val token = args(2)

  def run(): Unit = {
    val strategy = new MyStrategy()
    var actions: Map[Int, Action] = Map()
    remoteProcessClient.writeToken(token)
    val rules: Rules = remoteProcessClient.readRules()
    var game: Game = remoteProcessClient.readGame()
    while (game != null) {
      actions = Map()
      for (robot <- game.robots) {
        if (robot.is_teammate) {
          val action = strategy.act(robot, rules, game)
          actions = actions.updated(robot.id, action)
        }
      }
      remoteProcessClient.write(actions)
      game = remoteProcessClient.readGame()
    }
  }
}
