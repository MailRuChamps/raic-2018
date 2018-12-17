require './model/arena'

class Rules
    attr_reader :max_tick_count
    attr_reader :arena
    def initialize(json)
        @max_tick_count = json["max_tick_count"]
        @arena = Arena.new(json["arena"])
    end
end