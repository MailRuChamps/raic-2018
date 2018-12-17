class Arena
    attr_reader :width
    attr_reader :height
    attr_reader :depth
    attr_reader :bottom_radius
    attr_reader :top_radius
    attr_reader :corner_radius
    attr_reader :goal_top_radius
    attr_reader :goal_width
    attr_reader :goal_height
    attr_reader :goal_depth
    attr_reader :goal_side_radius
    def initialize(json)
        @width = json["width"]
        @width = json["width"]
        @height = json["height"]
        @depth = json["depth"]
        @bottom_radius = json["bottom_radius"]
        @top_radius = json["top_radius"]
        @corner_radius = json["corner_radius"]
        @goal_top_radius = json["goal_top_radius"]
        @goal_width = json["goal_width"]
        @goal_height = json["goal_height"]
        @goal_depth = json["goal_depth"]
        @goal_side_radius = json["goal_side_radius"]
    end
end