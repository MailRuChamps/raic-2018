namespace FSharpCgdk

open System.Collections.Generic
open FSharpCgdk.Model

module Runner = 

    let private log strAction msg = 
        printfn "%6s | %6s |> %s" "Runner" strAction msg


    let private iteration acc rules game =
        let processRobot (acc, actions) robot = 
            let action, acc = MyStrategy.act(robot, rules, game, acc)
            acc, Map.add (string robot.id) action actions
        game.robots
        |> Array.filter (fun x -> x.is_teammate)
        |> Array.fold processRobot (EmptyData, Map.empty)

    let startRunner rpc token = 
        RemoteProcessClient.writeToken rpc token
        log "startRunner" token

        log "startRunner" "read rules"
        let rules = RemoteProcessClient.readRules rpc |> Option.get
        log "startRunner" "read game"
        let gameOpt = RemoteProcessClient.readGame rpc 
        
        let rec iterRunner = function
            | _, None -> 
                log "iterRunner" "None game"
                ()
            | acc, Some game -> 
                log "iterRunner" ("tick = " + (string game.current_tick))
                let acc, actions = iteration acc rules game
                RemoteProcessClient.write rpc actions
                let gameOpt = RemoteProcessClient.readGame rpc
                iterRunner (acc, gameOpt)

        iterRunner (EmptyData, gameOpt)
    
    [<EntryPoint>]
    let main(args : string array) = 
        log "main" "start"
        if Array.length args = 3 then
            startRunner 
            <| RemoteProcessClient.create args.[0] (int args.[1])
            <| args.[2]
        else
            startRunner
            <| RemoteProcessClient.create "127.0.0.1" 31001
            <| "0000000000000000"
        log "main" "end"
        0        
    
