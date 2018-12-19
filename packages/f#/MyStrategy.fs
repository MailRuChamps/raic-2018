namespace FSharpCgdk

open FSharpCgdk.Model
open System.Numerics
open FSharp.Json


type StrategyData = 
    | EmptyData


module MyStrategy = 
    
    let act (me : Robot, rules : Rules, game : Game, data : StrategyData) : Action * StrategyData =
        failwith "code me"
