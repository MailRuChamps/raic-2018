namespace FSharpCgdk.Model

type Robot = {
    id : int
    player_id : int
    is_teammate : bool
    x : float32
    y : float32
    z : float32
    velocity_x : float32
    velocity_y : float32
    velocity_z : float32
    radius : float32
    nitro_amount : float32
    touch : bool
    touch_normal_x : float32 option
    touch_normal_y : float32 option
    touch_normal_z : float32 option
}

open System.Numerics

module Robot = 
    let id robot = robot.id
    let player_id robot = robot.player_id

    let is_teammate robot = robot.is_teammate

    let x robot = robot.x
    let y robot = robot.y
    let z robot = robot.z

    let position robot = Vector3(robot.x, robot.y, robot.z)
    let position2 robot = Vector2(robot.x, robot.z)

    let velocity_x robot = robot.velocity_x
    let velocity_y robot = robot.velocity_y
    let velocity_z robot = robot.velocity_z
    
    let velocity robot = Vector3(robot.velocity_x, robot.velocity_y, robot.velocity_z)
    let velocity2 robot = Vector2(robot.velocity_x, robot.velocity_z)

    let radius robot = robot.radius

    let nitro_amount robot = robot.nitro_amount

    let touch robot = robot.touch
    
    let touch_normal_x robot = robot.touch_normal_x
    let touch_normal_y robot = robot.touch_normal_y
    let touch_normal_z robot = robot.touch_normal_z