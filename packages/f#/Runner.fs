namespace FSharpCgdk

open System.Collections.Generic
open FSharpCgdk.Model

module Runner = 
    let private iteration (acc : StrategyData) (rules : Rules) (game : Game)  =
        let processRobot (acc, actions) robot = 
            let action, acc = MyStrategy.act(robot, rules, game, acc)
            acc, Map.add (string robot.id) action actions
        game.robots
        |> Array.filter (fun x -> x.is_teammate)
        |> Array.fold processRobot (acc, Map.empty)


    let startRunner rpc token = 
        RemoteProcessClient.writeToken rpc token

        let rules = RemoteProcessClient.readRules rpc |> Option.get
        let gameOpt = RemoteProcessClient.readGame rpc 
        
        let rec iterRunner = function
            | _, None -> ()
            | acc, Some game -> 
                let acc, actions = iteration acc rules game
                RemoteProcessClient.write rpc actions
                let gameOpt = RemoteProcessClient.readGame rpc
                iterRunner (acc, gameOpt)

        iterRunner (EmptyData, gameOpt)

    
    let templateArgs = "127.0.0.1", "31001", "0000000000000000"
    
    [<EntryPoint>]
    let main(args : string array) = 
        let host, port, token = 
            match args with
            | [| host; port; token |] -> host, port, token
            | otherwise -> templateArgs
        let rpc = RemoteProcessClient.create host (int port)
        startRunner rpc token   
        0  
