from .player import *
from .ball import *
from .nitro_pack import *
from .robot import *


class Game:
    def __init__(self, json):
        self.current_tick = json.get("current_tick")
        self.players = list(map(Player, json.get("players")))
        self.robots = list(map(Robot, json.get("robots")))
        self.nitro_packs = list(map(NitroPack, json.get("nitro_packs")))
        self.ball = Ball(json.get("ball"))
