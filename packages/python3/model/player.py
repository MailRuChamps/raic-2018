class Player:
    def __init__(self, json):
        self.id = json.get("id")
        self.me = json.get("me")
        self.strategy_crashed = json.get("strategy_crashed")
        self.score = json.get("score")
