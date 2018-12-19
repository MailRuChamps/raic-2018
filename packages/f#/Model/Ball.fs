namespace FSharpCgdk.Model

type Ball = {
    x : float
    y : float
    z : float
    velocity_x : float
    velocity_y : float
    velocity_z : float
    radius : float
}

open System.Numerics

module Ball = 
    let x ball = ball.x
    let y ball = ball.y
    let z ball = ball.z

    let position ball = Vector3(float32 ball.x, float32 ball.y, float32 ball.z)
    let position2 ball = Vector2(float32 ball.x, float32 ball.z)

    let velocity_x ball = ball.velocity_x
    let velocity_y ball = ball.velocity_y
    let velocity_z ball = ball.velocity_z
    
    let velocity ball = Vector3(float32 ball.velocity_x, float32 ball.velocity_y, float32 ball.velocity_z)
    let velocity2 ball = Vector2(float32 ball.velocity_x, float32 ball.velocity_z)

    let radius ball = ball.radius