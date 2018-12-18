import model._

final class MyStrategy extends Strategy {

    def act(me: Robot, rules: Rules, game: Game, action: Action): Unit = {
        action.target_velocity_x = 100
        action.target_velocity_y = 100
        action.target_velocity_z = 100
    }
}
