namespace FSharpCgdk

open System.Collections.Generic
open FSharpCgdk.Model

module Runner = 

    let private iteration (actions : IDictionary<int, Action>) acc rules game =
        let processRobot acc robot = 
            let action, acc = MyStrategy.act(robot, rules, game, acc)
            actions.Add(robot.id, action) 
            acc 
        game.robots
        |> Array.filter (fun x -> x.is_teammate)
        |> Array.fold processRobot EmptyData

    let startRunner rpc token = 
        RemoteProcessClient.writeToken rpc token

        let rules = RemoteProcessClient.readRules rpc |> Option.get
        let gameOpt = RemoteProcessClient.readGame rpc 
        let actions = new Dictionary<int, Action>()
        
        let rec iterRunner = function
            | _, None -> ()
            | acc, Some game -> 
                actions.Clear()
                let acc = iteration actions acc rules game
                RemoteProcessClient.write rpc actions
                let gameOpt = RemoteProcessClient.readGame rpc
                iterRunner (acc, gameOpt)

        iterRunner (EmptyData, gameOpt)
    
    [<EntryPoint>]
    let main(args : string array) = 
        if Array.length args = 3 then
            startRunner 
            <| RemoteProcessClient.create args.[0] (int args.[1])
            <| args.[2]
        else
            startRunner
            <| RemoteProcessClient.create "127.0.0.1" 31001
            <| "0000000000000000"
        0        
    
