package model

class Robot {
    var id: Int = 0
    var player_id: Int = 0
    var is_teammate: Boolean = false
    var x: Double = 0.0
    var y: Double = 0.0
    var z: Double = 0.0
    var velocity_x: Double = 0.0
    var velocity_y: Double = 0.0
    var velocity_z: Double = 0.0
    var radius: Double = 0.0
    var nitro_amount: Double = 0.0
    var touch: Boolean = false
    var touch_normal_x: Double? = null
    var touch_normal_y: Double? = null
    var touch_normal_z: Double? = null
}
