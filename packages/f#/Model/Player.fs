namespace FSharpCgdk.Model

type Player = {
    id : int
    me : bool
    strategy_crashed : bool
    score : int
}

module Player = 
    let id player = player.id

    let me player = player.me

    let strategy_crashed player = player.strategy_crashed
    
    let score player = player.score