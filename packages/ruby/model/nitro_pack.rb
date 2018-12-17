class NitroPack
    attr_reader :id
    attr_reader :x
    attr_reader :y
    attr_reader :z
    attr_reader :radius
    attr_reader :nitro_amount
    attr_reader :respawn_ticks
    def initialize(json)
        @id = json["id"]
        @x = json["x"]
        @y = json["y"]
        @z = json["z"]
        @radius = json["radius"]
        @nitro_amount = json["nitro_amount"]
        @respawn_ticks = json["respawn_ticks"]
    end
end