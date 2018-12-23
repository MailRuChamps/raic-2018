namespace FSharpCgdk

open FSharpCgdk.Model
open System.Numerics
open FSharp.Json


module MyStrategy = 
    let act (me : Robot, rules : Rules, game : Game, action : Action) =
        ()

    let customRendering () : string =
        ""
