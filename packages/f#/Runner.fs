namespace FSharpCgdk

open System.Collections.Generic
open FSharpCgdk.Model

module Runner = 
    let emptyAction() = {
        target_velocity_x = 0.0
        target_velocity_y = 0.0
        target_velocity_z = 0.0
        jump_speed = 0.0
        use_nitro = false }


    let startRunner (rpc : RemoteProcessClient) (token : string) = 
        rpc.writeToken token

        let rules = rpc.readRules() |> Option.get
        let mutable gameOpt = rpc.readGame() 
        let mutable actions = Map.empty

        while gameOpt <> None do 
            actions <- Map.empty
            let game = Option.get gameOpt
            let teammates = game.robots |> Array.filter (fun x -> x.is_teammate)        
            for robot in teammates do 
                let action = emptyAction()
                MyStrategy.act(robot, rules, game, action)
                actions <- Map.add (string robot.id) action actions
            rpc.write (actions, MyStrategy.customRendering())
            gameOpt <- rpc.readGame()

    
    let templateArgs = "127.0.0.1", "31001", "0000000000000000"
    
    [<EntryPoint>]
    let main(args : string array) = 
        let host, port, token = 
            match args with
            | [| host; port; token |] -> host, port, token
            | otherwise -> templateArgs
        let rpc = RemoteProcessClient(host, int port)
        startRunner rpc token   
        0  
