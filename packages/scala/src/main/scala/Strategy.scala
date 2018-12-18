import model._

trait Strategy {
    def act(me: Robot, rules: Rules, game: Game, action: Action)
}