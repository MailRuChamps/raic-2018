package model

final class Game(val current_tick: Int,
                 val players: Array[Player],
                 val robots: Array[Robot],
                 val nitro_packs: Array[NitroPack],
                 val ball: Ball)
