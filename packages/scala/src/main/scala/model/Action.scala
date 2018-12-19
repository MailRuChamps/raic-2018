package model

case class Action(target_velocity_x: Double,
                  target_velocity_y: Double,
                  target_velocity_z: Double,
                  jump_speed: Double,
                  use_nitro: Boolean)
