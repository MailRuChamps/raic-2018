package model

case class Player(id: Int,
                  me: Boolean,
                  strategy_crashed: Boolean,
                  score: Int)
