namespace FSharpCgdk.Model

type Rules = {
    max_tick_count : int
    arena : Arena
}

module Rules = 
    let max_tick_count rules = rules.max_tick_count

    let arena rules = rules.arena