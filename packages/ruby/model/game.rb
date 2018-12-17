require './model/player'
require './model/ball'
require './model/nitro_pack'
require './model/robot'

class Game
    attr_reader :current_tick
    attr_reader :players
    attr_reader :robots
    attr_reader :nitro_packs
    attr_reader :ball
    def initialize(json)
        @current_tick = json["current_tick"]
        @players = json["players"].map { |o| Player.new(o) }
        @robots = json["robots"].map { |o| Robot.new(o) }
        @nitro_packs = json["nitro_packs"].map { |o| NitroPack.new(o) }
        @ball = Ball.new(json["ball"])
    end
end