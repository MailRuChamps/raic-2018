namespace FSharpCgdk.Model

type Action = {
    mutable target_velocity_x : float
    mutable target_velocity_y : float
    mutable target_velocity_z : float
    mutable jump_speed : float
    mutable use_nitro : bool }
