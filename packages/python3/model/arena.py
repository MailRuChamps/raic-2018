class Arena:
    def __init__(self, json):
        self.width = json.get("width")
        self.width = json.get("width")
        self.height = json.get("height")
        self.depth = json.get("depth")
        self.bottom_radius = json.get("bottom_radius")
        self.top_radius = json.get("top_radius")
        self.corner_radius = json.get("corner_radius")
        self.goal_top_radius = json.get("goal_top_radius")
        self.goal_width = json.get("goal_width")
        self.goal_height = json.get("goal_height")
        self.goal_depth = json.get("goal_depth")
        self.goal_side_radius = json.get("goal_side_radius")
