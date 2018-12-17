package model

class Game {
    var current_tick: Int = 0
    lateinit var players: Array<Player>
    lateinit var robots: Array<Robot>
    lateinit var nitro_packs: Array<NitroPack>
    lateinit var ball: Ball
}
