namespace FSharpCgdk

open FSharpCgdk.Model
open System.Numerics
open FSharp.Json


type StrategyData = 
    | EmptyData
    | KnownAttackBot of forTick: int * botId : int 


type ActData = Robot * Rules * Game * Action


module MyStrategy = 
    let private EPS = 0.1

    let private vector2 x y = Vector2 (x, y) 

    let private posFromRobot (m : Robot) = Vector3 (float32 m.x, float32 m.y, float32 m.z) 
    let private velFromRobot (m : Robot) = Vector3 (float32 m.velocity_x, float32 m.velocity_y, float32 m.velocity_z) 
    
    let private posFromBall (m : Ball) = Vector3 (float32 m.x, float32 m.y, float32 m.z) 
    let private velFromBall (m : Ball) = Vector3 (float32 m.velocity_x, float32 m.velocity_y, float32 m.velocity_z) 
    
    let private pos2FromRobot (m : Robot) = Vector2 (float32 m.x, float32 m.z) 
    let private vel2FromRobot (m : Robot) = Vector2 (float32 m.velocity_x, float32 m.velocity_z) 

    let private pos2FromBall (m : Ball) = Vector2 (float32 m.x, float32 m.z) 
    let private vel2FromBall (m : Ball) = Vector2 (float32 m.velocity_x, float32 m.velocity_z) 


    let private getJumpSpeed (dist_for_jump : float) (jump_speed : float) (robot : Robot) (ball : Ball) (dist : float) : float = 
        let need_jump = dist < dist_for_jump && robot.y < ball.y
        if need_jump then jump_speed else 0.0

    let private isNormalSpeed max_speed speed = 
        0.5 * max_speed < speed && speed < max_speed


    let mutable data : StrategyData = EmptyData


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

        let me_pos2 = pos2FromRobot me
        let max_speed32 = float32 rules.ROBOT_MAX_GROUND_SPEED
        let target_velocity = (interception - me_pos2) * max_speed32

        let delta = (pos2FromBall ball) - me_pos2

        let dist_for_jump = rules.ROBOT_RADIUS + rules.BALL_RADIUS
        let max_jump_speed = rules.ROBOT_MAX_JUMP_SPEED
        let jump_speed = 
            delta.Length() 
            |> float 
            |> getJumpSpeed dist_for_jump max_jump_speed me ball

        {
            target_velocity_x = float target_velocity.X
            target_velocity_y = 0.0
            target_velocity_z = float target_velocity.Y
            jump_speed = jump_speed
            use_nitro = true
        }


    let attackEntryPoint (me, rules, game, action) : Action option = 
        let delta = (posFromBall game.ball) - (posFromRobot me)

        let dist_for_jump = rules.ROBOT_RADIUS + rules.BALL_RADIUS
        let max_jump_speed = rules.ROBOT_MAX_JUMP_SPEED
        let jump_speed = 
            delta.Length() 
            |> float 
            |> getJumpSpeed dist_for_jump max_jump_speed me game.ball
        
        let me_pos2 = pos2FromRobot me

        let max_speed = rules.ROBOT_MAX_GROUND_SPEED
        let isPossibleSpeed : float -> bool = isNormalSpeed max_speed

        let tryAttack (pos2 : Vector2, time : float32) = 
            let delta2 = pos2 - me_pos2
            let need_speed = delta.Length() / time
            let target_velocity = delta2 / time
            match isPossibleSpeed (float need_speed) with
            | true -> 
                Some {                
                    target_velocity_x = float target_velocity.X
                    target_velocity_y = 0.0
                    target_velocity_z = float target_velocity.Y
                    jump_speed = jump_speed
                    use_nitro = true
                }
            | false -> None      

        let ball_pos2 = pos2FromBall game.ball
        let ball_pos2_delta = vel2FromBall game.ball * 0.1f
        let ballPosAfterTicksWithTime ticks  = 
            let new_pos2 = ball_pos2 + ball_pos2_delta * float32 ticks
            let new_time = 0.1f * float32 ticks
            (new_pos2, new_time)

        let ballInArena (pos2 : Vector2, _) = 
            abs(float pos2.X) < rules.arena.width / 2. 
            && abs(float pos2.Y) < rules.arena.depth / 2.

        Array.init 100 ballPosAfterTicksWithTime
        |> Array.takeWhile ballInArena
        |> Array.tryPick tryAttack


    let attackAct (args : ActData) : Action =
        match attackEntryPoint args with
        | Some x -> x
        | None -> protectAct args
    

    let (|AttackBot|ProtectBot|) (robot : Robot) =
        match data with
        | EmptyData -> failwith "EmptyData at (|AttackBot|ProtectBot|). Error!!!"
        | KnownAttackBot (tick, id) when id = robot.id -> AttackBot
        | otherwise -> ProtectBot


    let private tuple2 x y = (x, y)


    let nextData (args : ActData) : StrategyData = 
        let _, _, game, _ = args
        let cur_tick = game.current_tick
        match data with
        | KnownAttackBot(tick, _) when tick = cur_tick -> data
        | otherwise ->
            game.robots
            |> Array.filter (fun x -> x.is_teammate)
            |> Array.maxBy (fun x -> x.z)
            |> (fun x -> x.id)
            |> tuple2 cur_tick
            |> KnownAttackBot


    let assignFieldsAction toActoin fromAction = 
        toActoin.target_velocity_x <- fromAction.target_velocity_x
        toActoin.target_velocity_y <- fromAction.target_velocity_y
        toActoin.target_velocity_z <- fromAction.target_velocity_z
        toActoin.jump_speed <- fromAction.jump_speed
        toActoin.use_nitro <- fromAction.use_nitro

    let act (me : Robot, rules : Rules, game : Game, action : Action) =
        data <- nextData (me, rules, game, action)
        let args = me, rules, game, action
        let newAction =         
            match me with
            | AttackBot  -> attackAct args
            | ProtectBot -> protectAct args
        assignFieldsAction action newAction
