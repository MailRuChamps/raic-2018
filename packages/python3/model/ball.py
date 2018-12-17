class Ball:
    def __init__(self, json):
        self.x = json.get("x")
        self.y = json.get("y")
        self.z = json.get("z")
        self.velocity_x = json.get("velocity_x")
        self.velocity_y = json.get("velocity_y")
        self.velocity_z = json.get("velocity_z")
        self.radius = json.get("radius")
