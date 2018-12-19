namespace FSharpCgdk

open FSharpCgdk.Model
open System.Numerics
open FSharp.Json


type StrategyData = 
    | EmptyData
    | AttackBotData of idRobot : int 


type ActData = Robot * Rules * Game * StrategyData


type Tmp3d = {
    x : float
    y : float
    z : float
}

type Tmp2d = {
    x : float
    y : float
}

module MyStrategy = 


    let private _log game name msg = 
        printfn "%5d|%6s|%s" game.current_tick name msg

    let mutable log = fun name msg -> ()

    let logJson name_obj x =
        Json.serializeU x
        |> sprintf "%s is %s" name_obj
        |> log "json"

    let logFloat name_obj x =
        string x
        |> sprintf "%s is %s" name_obj
        |> log "json"


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
                log "sim" "break"
                None
            else 
                match func state with
                | None -> 
                    log "sim" "not found, go next"
                    iter (ticks - 1) (nextState state)
                | Some x -> 
                    log "sim" "find"
                    Some x
        iter ticks startState  
    

    let attackEntry (me, arena, ball) : Action option = 

        log "Robot" "Need find attack entry point."

        let me_pos = Robot.position me
        logJson "me_pos" { x = float me_pos.X; y = float me_pos.Y; z = float me_pos.Z }
        let ball_pos = Ball.position ball
        logJson "ball_pos" { x = float ball_pos.X; y = float ball_pos.Y; z = float ball_pos.Z }

        let delta = ball_pos - me_pos
        logJson "delta" { x = float delta.X; y = float delta.Y; z = float delta.Z }

        let jump = float (delta.Length()) < (BALL_RADIUS + ROBOT_MAX_RADIUS) && me.y < ball.y
        let jump_speed = if jump then ROBOT_MAX_JUMP_SPEED else 0.   
        
        let me_pos2d = Robot.position2 me
        let inSim (pos : Vector3, time : float) = 
            logJson "pos" { x = float pos.X; y = float pos.Y; z = float pos.Z }
            logFloat "time" time
            let pos2d = Vector2(pos.X, pos.Z)
            logJson "pos2d" { x = float pos2d.X; y = float pos2d.Y }
            let delta = pos2d - me_pos2d
            let need_speed = float (delta.Length()) / time
            let mx_speed = ROBOT_MAX_GROUND_SPEED
            logJson "delta" { x = float delta.X; y = float delta.Y }
            logFloat "need_speed" need_speed
            logFloat "mx_speed" mx_speed
            if 0.5 * mx_speed < need_speed && need_speed < mx_speed then
                let velocity_target = Vector2.Normalize(delta) * (float32 need_speed)
                Action.fromVector2 velocity_target jump_speed true
                |> Some
            else 
                None      

        let ball_pos_delta = Ball.velocity ball * 0.1f
        logJson "ball_pos_delta" { x = float ball_pos_delta.X; y = float ball_pos_delta.Y; z = float ball_pos_delta.Z }
        let nextState (pos : Vector3, time) = (pos + ball_pos_delta, time + 0.1)

        let notInArena (pos : Vector3) = 
            abs(float pos.X) > arena.width / 2. 
            || abs(float pos.Z) > arena.depth / 2.

        simulationLinear inSim nextState (fun (pos,_) -> notInArena pos) (ball_pos, 0.) 100


    let attackAct (args : ActData) : Action =
        let me, rules, game, _ = args
        match attackEntry (me, rules.arena, game.ball) with
        | Some x ->
            log "Robot" "I known how attack!!!"
            x
        | None -> 
            log "Robot" "I don't known how attack! QwQ"
            defenseAct args
        

    let private tuple2 x y = (x, y)
    

    let (|AttackBot|DefenseBot|) (robot : Robot, data : StrategyData) =
        match data with
        | EmptyData -> 
            failwith "EmptyData at (|AttackBot|DefenseBot|). Error!!!"
        | AttackBotData id when id = robot.id -> 
            log "Robot" "I am AttackBot"
            AttackBot
        | _ -> 
            log "Robot" "I am DefenseBot"
            DefenseBot


    let nextData (args : ActData) : StrategyData = 
        let _, _, game, _ = args
        game.robots
        |> Array.filter (fun x -> x.is_teammate)
        |> Array.maxBy (fun x -> x.z)
        |> Robot.id 
        |> AttackBotData


    let act (me : Robot, rules : Rules, game : Game, data : StrategyData) : Action * StrategyData =
        log <- _log game
        
        sprintf "My id is %d." me.id
        |> log "Robot" 

        let data = nextData (me, rules, game, data)
        let args = me, rules, game, data
        let action = 
            match me, data with
            | AttackBot ->
                log "Robot" "I will attack!"
                attackAct args
            | DefenseBot -> 
                log "Robot" "I will defend!"
                defenseAct args
        action, data
