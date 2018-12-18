namespace FSharpCgdk

open FSharpCgdk.Model

module MyStrategy =
    let act (me : Robot) (rules : Rules) (game : Game) : Action =
        {
            target_velocity_x = 0.
            target_velocity_y = 0.
            target_velocity_z = 0.
            jump_speed = 0.
            use_nitro = false
        }