class Action
    attr_accessor :target_velocity_x, :target_velocity_y, :target_velocity_z, :jump_speed, :use_nitro
    def initialize()
        @target_velocity_x = 0.0
        @target_velocity_y = 0.0
        @target_velocity_z = 0.0
        @jump_speed = 0.0
        @use_nitro = false
    end
    def to_json(arg)
        {
            'target_velocity_x' => @target_velocity_x,
            'target_velocity_y' => @target_velocity_y,
            'target_velocity_z' => @target_velocity_z,
            'jump_speed' => @jump_speed,
            'use_nitro' => @use_nitro
        }.to_json
    end
end