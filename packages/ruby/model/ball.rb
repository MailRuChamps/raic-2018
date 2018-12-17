class Ball
    attr_reader :x
    attr_reader :y
    attr_reader :z
    attr_reader :velocity_x
    attr_reader :velocity_y
    attr_reader :velocity_z
    attr_reader :radius
    def initialize(json)
        @x = json["x"]
        @y = json["y"]
        @z = json["z"]
        @velocity_x = json["velocity_x"]
        @velocity_y = json["velocity_y"]
        @velocity_z = json["velocity_z"]
        @radius = json["radius"]
    end
end