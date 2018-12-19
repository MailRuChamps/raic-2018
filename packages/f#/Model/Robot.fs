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
    touch_normal_z : float option
}

open System.Numerics

module Robot = 
    let id robot = robot.id
    let player_id robot = robot.player_id

    let is_teammate robot = robot.is_teammate

    let x robot = robot.x
    let y robot = robot.y
    let z robot = robot.z

    let position robot = Vector3(float32 robot.x, float32 robot.y, float32 robot.z)
    let position2 robot = Vector2(float32 robot.x, float32 robot.z)

    let velocity_x robot = robot.velocity_x
    let velocity_y robot = robot.velocity_y
    let velocity_z robot = robot.velocity_z
    
    let velocity robot = Vector3(float32 robot.velocity_x, float32 robot.velocity_y, float32 robot.velocity_z)
    let velocity2 robot = Vector2(float32 robot.velocity_x, float32 robot.velocity_z)

    let radius robot = robot.radius

    let nitro_amount robot = robot.nitro_amount

    let touch robot = robot.touch
    
    let touch_normal_x robot = robot.touch_normal_x
    let touch_normal_y robot = robot.touch_normal_y
    let touch_normal_z robot = robot.touch_normal_z