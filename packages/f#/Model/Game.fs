namespace FSharpCgdk.Model

type Game = {
    current_tick : int
    players : Player array
    robots : Robot array
    nitro_packs : NitroPack array
    ball : Ball
}

module Game = 
    let current_tick game = game.current_tick

    let players game = game.players
    let robots game = game.robots
    let nitro_packs game = game.nitro_packs

    let ball game = game.ball