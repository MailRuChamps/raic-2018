class Robot
    attr_reader :id
    attr_reader :player_id
    attr_reader :is_teammate
    attr_reader :x
    attr_reader :y
    attr_reader :z
    attr_reader :velocity_x
    attr_reader :velocity_y
    attr_reader :velocity_z
    attr_reader :radius
    attr_reader :nitro_amount
    attr_reader :touch
    attr_reader :touch_normal_x
    attr_reader :touch_normal_y
    attr_reader :touch_normal_z
    def initialize(json)
        @id = json["id"]
        @player_id = json["player_id"]
        @is_teammate = json["is_teammate"]
        @x = json["x"]
        @y = json["y"]
        @z = json["z"]
        @velocity_x = json["velocity_x"]
        @velocity_y = json["velocity_y"]
        @velocity_z = json["velocity_z"]
        @radius = json["radius"]
        @nitro_amount = json["nitro_amount"]
        @touch = json["touch"]
        @touch_normal_x = json["touch_normal_x"]
        @touch_normal_y = json["touch_normal_y"]
        @touch_normal_z = json["touch_normal_z"]
    end
end