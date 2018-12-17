class NitroPack:
    def __init__(self, json):
        self.id = json.get("id")
        self.x = json.get("x")
        self.y = json.get("y")
        self.z = json.get("z")
        self.radius = json.get("radius")
        self.nitro_amount = json.get("nitro_amount")
        self.respawn_ticks = json.get("respawn_ticks")
