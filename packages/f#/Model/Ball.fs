namespace FSharpCgdk.Model

type Ball = {
    x : float32
    y : float32
    z : float32
    velocity_x : float32
    velocity_y : float32
    velocity_z : float32
    radius : float32
}

open System.Numerics

module Ball = 
    let x ball = ball.x
    let y ball = ball.y
    let z ball = ball.z

    let position ball = Vector3(ball.x, ball.y, ball.z)
    let position2 ball = Vector2(ball.x, ball.z)

    let velocity_x ball = ball.velocity_x
    let velocity_y ball = ball.velocity_y
    let velocity_z ball = ball.velocity_z
    
    let velocity ball = Vector3(ball.velocity_x, ball.velocity_y, ball.velocity_z)
    let velocity2 ball = Vector2(ball.velocity_x, ball.velocity_z)

    let radius ball = ball.radius