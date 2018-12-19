namespace FSharpCgdk

open FSharpCgdk.Model
open System.Numerics
open FSharp.Json


type StrategyData = 
    | EmptyData
    | KnownAttackBot of forTick: int * botId : int 


type ActData = Robot * Rules * Game * StrategyData


module MyStrategy = 
    let private EPS = 0.1
    let private max_speed = ROBOT_MAX_GROUND_SPEED
    let private max_speed32 = float32 ROBOT_MAX_GROUND_SPEED
    let private max_jump = ROBOT_MAX_JUMP_SPEED

    let private vector2 x y = Vector2 (x, y) 


    let getJumpSpeed (robot : Robot) (ball : Ball) (dist  : float) = 
        let need_jump = dist < (BALL_RADIUS + ROBOT_MAX_RADIUS) && robot.y < ball.y
        if need_jump then max_jump else 0.   


    let protectAct (args : ActData) : Action =
        let me, rules, game, _ = args
        let ball, arena = game.ball, rules.arena

        let target_pos_z = -(arena.depth / 2.) + arena.bottom_radius
        let time = (target_pos_z - ball.z) / ball.velocity_z  
        let x = ball.x + ball.velocity_x * time

        let interception = 
            if ball.velocity_z > -EPS then 
                vector2 0.f (float32 target_pos_z)
            else
                vector2 (float32 x) (float32 target_pos_z)

        let target_velocity = (interception - Robot.position2 me) * max_speed32

        let delta = (Ball.position ball) - (Robot.position me)

        let jump_speed = delta.Length() |> float |> getJumpSpeed me ball

        Action.fromVelocity2 target_velocity jump_speed true
    

    let isNormalSpeed speed = 
        0.5 * max_speed < speed && speed < max_speed


    let attackEntryPoint (me, arena, ball) : Action option = 
        let delta = (Ball.position ball) - (Robot.position me)
        let jump_speed = delta.Length() |> float |> getJumpSpeed me ball
        
        let me_pos2 = Robot.position2 me

        let tryAttack (pos2 : Vector2, time : float32) = 
            let delta2 = pos2 - me_pos2
            let need_speed = delta.Length() / time
            let velocity2 = delta2 / time
            match isNormalSpeed (float need_speed) with
            | true -> Some <| Action.fromVelocity2 velocity2 jump_speed true
            | false -> None      

        let ball_pos2 = Ball.position2 ball
        let ball_pos2_delta = Ball.velocity2 ball * 0.1f
        let ballPosAfterTicksWithTime ticks  = 
            let new_pos2 = ball_pos2 + ball_pos2_delta * float32 ticks
            let new_time = 0.1f * float32 ticks
            (new_pos2, new_time)

        let ballInArena (pos2 : Vector2, _) = 
            abs(float pos2.X) < arena.width / 2. 
            && abs(float pos2.Y) < arena.depth / 2.

        Array.init 100 ballPosAfterTicksWithTime
        |> Array.takeWhile ballInArena
        |> Array.tryPick tryAttack


    let attackAct (args : ActData) : Action =
        let me, rules, game, _ = args
        match attackEntryPoint (me, rules.arena, game.ball) with
        | Some x -> x
        | None -> protectAct args
    

    let (|AttackBot|ProtectBot|) (robot : Robot, data : StrategyData) =
        match data with
        | EmptyData -> failwith "EmptyData at (|AttackBot|ProtectBot|). Error!!!"
        | KnownAttackBot (tick, id) when id = robot.id -> AttackBot
        | otherwise -> ProtectBot


    let private tuple2 x y = (x, y)


    let nextData (args : ActData) : StrategyData = 
        let _, _, game, data = args
        let cur_tick = game.current_tick
        match data with
        | KnownAttackBot(tick, _) when tick = cur_tick -> data
        | otherwise ->
            game.robots
            |> Array.filter Robot.is_teammate
            |> Array.maxBy Robot.z
            |> Robot.id 
            |> tuple2 cur_tick
            |> KnownAttackBot


    let act (me : Robot, rules : Rules, game : Game, data : StrategyData) : Action * StrategyData =
        let data = nextData (me, rules, game, data)
        let args = me, rules, game, data
        let action = 
            match me, data with
            | AttackBot  -> attackAct args
            | ProtectBot -> protectAct args
        action, data
