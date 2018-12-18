package model

final class Robot(val id: Int,
                  val player_id: Int,
                  val is_teammate: Boolean,
                  val x: Double,
                  val y: Double,
                  val z: Double,
                  val velocity_x: Double,
                  val velocity_y: Double,
                  val velocity_z: Double,
                  val radius: Double,
                  val nitro_amount: Double,
                  val touch: Boolean,
                  val touch_normal_x: Double,
                  val touch_normal_y: Double,
                  val touch_normal_z: Double)