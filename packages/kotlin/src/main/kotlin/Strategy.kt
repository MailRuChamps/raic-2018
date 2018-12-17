import model.*

interface Strategy {
    fun act(me: Robot, rules: Rules, game: Game, action: Action)
}
