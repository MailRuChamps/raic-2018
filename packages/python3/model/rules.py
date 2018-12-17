from .arena import *


class Rules:
    def __init__(self, json):
        self.max_tick_count = json.get("max_tick_count")
        self.arena = Arena(json.get("arena"))
