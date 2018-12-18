namespace FSharpCgdk.Model

type Action = {
    target_velocity_x : float32
    target_velocity_y : float32
    target_velocity_z : float32
    jump_speed : float32
    use_nitro : bool
}

open System.Numerics

module Action = 
    let target_velocity_x act = act.target_velocity_x
    let target_velocity_y act = act.target_velocity_y
    let target_velocity_z act = act.target_velocity_z

    let target_velocity act = Vector3(act.target_velocity_x, act.target_velocity_y, act.target_velocity_z)
    let target_velocity2 act = Vector2(act.target_velocity_x, act.target_velocity_z)

    let fromVector3 (v3 : Vector3) jump_speed use_nitro = {
        target_velocity_x = v3.X
        target_velocity_y = v3.Y
        target_velocity_z = v3.Z
        jump_speed = jump_speed
        use_nitro = use_nitro
    }

    let fromVector2 (v2 : Vector2) jump_speed use_nitro = {
        target_velocity_x = v2.X
        target_velocity_y = 0.f
        target_velocity_z = v2.Y
        jump_speed = jump_speed
        use_nitro = use_nitro
    }

    let jump_speed act = act.jump_speed

    let use_nitro act = act.use_nitro