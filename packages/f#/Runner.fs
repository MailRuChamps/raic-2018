namespace FSharpCgdk

open System.Collections.Generic
open FSharpCgdk.Model

module Runner = 
    let startRunner rpc token = 
        RemoteProcessClient.writeToken rpc token

        let rules = RemoteProcessClient.readRules rpc |> Option.get
        let actions = new Dictionary<int, Action>()
        let inIter game =
            actions.Clear()
            for robot in game.robots do
                if robot.is_teammate then
                    let action = MyStrategy.act robot rules game
                    actions.Add(robot.id, action) 
            RemoteProcessClient.write rpc actions 
            RemoteProcessClient.readGame rpc           

        let rec iterRunner = function
            | None -> ()
            | Some game -> iterRunner (inIter game)
        
        RemoteProcessClient.readGame rpc 
        |> iterRunner
    
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
    
