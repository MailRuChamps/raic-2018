namespace FSharpCgdk.Model

type Robot = {
    id : int
    player_id : int
    is_teammate : bool
    x : float
    y : float
    z : float
    velocity_x : float
    velocity_y : float
    velocity_z : float
    radius : float
    nitro_amount : float
    touch : bool
    touch_normal_x : float option
    touch_normal_y : float option
    touch_normal_z : float option }
