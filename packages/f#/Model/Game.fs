namespace FSharpCgdk.Model

type Game = {
    current_tick : int
    players : Player array
    robots : Robot array
    nitro_packs : NitroPack array
    ball : Ball
}