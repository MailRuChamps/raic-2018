namespace FSharpCgdk.Model

type Action = {
    target_velocity_x : float
    target_velocity_y : float
    target_velocity_z : float
    jump_speed : float
    use_nitro : bool
}

open System.Numerics

module Action = 
    let target_velocity_x act = act.target_velocity_x
    let target_velocity_y act = act.target_velocity_y
    let target_velocity_z act = act.target_velocity_z

    let target_velocity act = Vector3(float32 act.target_velocity_x, float32 act.target_velocity_y, float32 act.target_velocity_z)
    let target_velocity2 act = Vector2(float32 act.target_velocity_x, float32 act.target_velocity_z)

    let fromVelocity (v3 : Vector3) jump_speed use_nitro = {
        target_velocity_x = float v3.X
        target_velocity_y = float v3.Y
        target_velocity_z = float v3.Z
        jump_speed = jump_speed
        use_nitro = use_nitro
    }

    let fromVelocity2 (v2 : Vector2) jump_speed use_nitro = {
        target_velocity_x = float v2.X
        target_velocity_y = 0.0
        target_velocity_z = float v2.Y
        jump_speed = jump_speed
        use_nitro = use_nitro
    }

    let jump_speed act = act.jump_speed

    let use_nitro act = act.use_nitro