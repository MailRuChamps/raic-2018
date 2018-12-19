package model

case class Game(current_tick: Int,
                players: Array[Player],
                robots: Array[Robot],
                nitro_packs: Array[NitroPack],
                ball: Ball)
