package model

case class Robot(id: Int,
                 player_id: Int,
                 is_teammate: Boolean,
                 x: Double,
                 y: Double,
                 z: Double,
                 velocity_x: Double,
                 velocity_y: Double,
                 velocity_z: Double,
                 radius: Double,
                 nitro_amount: Double,
                 touch: Boolean,
                 touch_normal_x: Double,
                 touch_normal_y: Double,
                 touch_normal_z: Double)
