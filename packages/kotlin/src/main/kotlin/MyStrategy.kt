import model.*

class MyStrategy : Strategy {
    override fun act(me: Robot, rules: Rules, game: Game, action: Action) {}
    override fun customRendering(): String {
        return ""
    }
}
