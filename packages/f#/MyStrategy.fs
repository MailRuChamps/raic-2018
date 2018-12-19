namespace FSharpCgdk

open FSharpCgdk.Model
open System.Numerics


type StrategyData = 
    | EmptyData
    | AttackBotData of tick : int * idRobot : int 


type ActData = Robot * Rules * Game * StrategyData


module MyStrategy = 


    let private EPS = 0.1


    let defenseAct (args : ActData) : Action =
        let me, rules, game, _ = args
        let ball, arena = game.ball, rules.arena
        let target_pos_z = -(arena.depth / 2.) + arena.bottom_radius

        let interception = 
            if ball.velocity_z > -EPS then 
                Vector3(0.f, 0.f, float32 target_pos_z)
            else
                let t = (target_pos_z - ball.z) / ball.velocity_z;
                let x = ball.x + ball.velocity_x * t;
                Vector3(float32 x, 0.f, float32 target_pos_z)

        let target_velocity = 
            Vector3(interception.X - (float32 me.x), 0.f, interception.Z - (float32 me.z))
            |> (*) (float32 ROBOT_MAX_GROUND_SPEED)

        let me_pos = Robot.position me
        let ball_pos = Ball.position ball

        let delta = ball_pos - me_pos

        let jump = float32 (delta.Length()) < float32 (BALL_RADIUS + ROBOT_MAX_RADIUS) && me.y < ball.y
        let jump_speed = if jump then ROBOT_MAX_JUMP_SPEED else 0.  

        Action.fromVector3 target_velocity jump_speed true


    let simulationLinear func nextState breaker startState ticks = 
        let rec iter ticks state = 
            if ticks = 0 || breaker state then
                None
            else 
                match func state with
                | None -> iter (ticks - 1) state
                | Some x -> Some x
        iter ticks startState  
    

    let attackEntry (me, arena, ball) : Action option = 
        let me_pos = Robot.position me
        let ball_pos = Ball.position ball

        let delta = ball_pos - me_pos

        let jump = float (delta.Length()) < (BALL_RADIUS + ROBOT_MAX_RADIUS) && me.y < ball.y
        let jump_speed = if jump then ROBOT_MAX_JUMP_SPEED else 0.   
        
        let me_pos2d = Robot.position2 me
        let inSim (pos : Vector3, time : float) = 
            let pos2d = Vector2(pos.X, pos.Z)
            let delta = pos2d - me_pos2d
            let need_speed = float (delta.Length()) / time
            let mx_speed = ROBOT_MAX_GROUND_SPEED
            if 0.5 * mx_speed < need_speed && need_speed < mx_speed then
                let velocity_target = Vector2.Normalize(delta) * (float32 need_speed)
                Action.fromVector2 velocity_target jump_speed true
                |> Some
            else 
                None      

        let ball_pos_delta = Ball.velocity ball * 0.1f
        let nextState (pos : Vector3, time) = (pos + ball_pos_delta, time + 0.1)

        let notInArena (pos : Vector3) = 
            abs(float pos.X) > arena.width / 2. 
            || abs(float pos.Z) > arena.depth / 2.

        simulationLinear inSim nextState (fun (pos,_) -> notInArena pos) (ball_pos, 0.) 100


    let attackAct (args : ActData) : Action =
        let me, rules, game, _ = args
        match attackEntry (me, rules.arena, game.ball) with
        | Some x -> x
        | None -> defenseAct args
        

    let private tuple2 x y = (x, y)
    

    let (|AttackBot|DefenseBot|) (robot : Robot, data : StrategyData) =
        match data with
        | EmptyData -> DefenseBot
        | AttackBotData(_, id) when id <> robot.id -> DefenseBot
        | _ -> AttackBot


    let nextData (args : ActData) : StrategyData = 
        let _, _, game, data = args
        let dataTick = 
            match data with
            | EmptyData -> -1
            | AttackBotData (tick, _) -> tick
        if dataTick = game.current_tick then
            data
        else
            game.robots
            |> Array.filter (fun x -> x.is_teammate)
            |> Array.maxBy (fun x -> x.z)
            |> Robot.id 
            |> tuple2 game.current_tick
            |> AttackBotData


    let act (me : Robot, rules : Rules, game : Game, data : StrategyData) : Action * StrategyData =
        let data = nextData (me, rules, game, data)
        let args = me, rules, game, data
        let action = 
            match me, data with
            | AttackBot -> attackAct args
            | DefenseBot -> defenseAct args
        action, data
