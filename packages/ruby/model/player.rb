class Player
    attr_reader :id
    attr_reader :me
    attr_reader :strategy_crashed
    attr_reader :score
    def initialize(json)
        @id = json["id"]
        @me = json["me"]
        @strategy_crashed = json["strategy_crashed"]
        @score = json["score"]
    end
end