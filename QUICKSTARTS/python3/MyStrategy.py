from model.action import Action
from model.game import Game
from model.robot import Robot
from model.rules import Rules

EPS = 1e-5

BALL_RADIUS = 2.0
ROBOT_MAX_RADIUS = 1.05
MAX_ENTITY_SPEED = 100.0
ROBOT_MAX_GROUND_SPEED = 30.0
ROBOT_MAX_JUMP_SPEED = 15.0

JUMP_TIME = 0.2
MAX_JUMP_HEIGHT = 3.0


class Point2D:
    def __init__(self, x=0.0, y=0.0):
        self.x = x
        self.y = y

    def len(self):
        return ((self.x * self.x) + (self.y * self.y))**0.5

    def __sub__(self, other):
        return Point2D(self.x - other.x, self.y - other.y)

    def __add__(self, other):
        return Point2D(self.x + other.x, self.y + other.y)

    def __mul__(self, num: float):
        return Point2D(self.x * num, self.y * num)

    def normalize(self):
        return Point2D(self.x/self.len(), self.y/self.len())


class MyStrategy:
    def act(self, me: Robot, rules: Rules, game: Game, action: Action):
        if not me.touch:
            action.target_velocity_x = 0.0
            action.target_velocity_y = -MAX_ENTITY_SPEED
            action.target_velocity_z = 0.0
            action.jump_speed = 0.0
            action.use_nitro = True
            return

        dist_to_ball = ((me.x - game.ball.x) ** 2
                        + (me.y - game.ball.y) ** 2
                        + (me.z - game.ball.z) ** 2
                        ) ** 0.5

        jump = (dist_to_ball < BALL_RADIUS +
                ROBOT_MAX_RADIUS and me.z < game.ball.z)

        is_attacker = len(game.robots) == 2

        for robot in game.robots:
            robot: Robot = robot
            if robot.is_teammate and robot.id != me.id:
                if robot.z < me.z:
                    is_attacker = True

        if is_attacker:
            for i in range(1, 101):
                t = i * 0.1
                ball_x = game.ball.x
                ball_y = game.ball.z
                ball_vel_x = game.ball.velocity_x
                ball_vel_y = game.ball.velocity_z
                ball_pos = Point2D(ball_x, ball_y) + \
                    Point2D(ball_vel_x, ball_vel_y) * t

                if ball_pos.y > me.z \
                        and abs(ball_pos.x) < (rules.arena.width / 2.0) \
                        and abs(ball_pos.y) < (rules.arena.depth / 2.0):

                    delta_pos = Point2D(
                        ball_pos.x, ball_pos.y) - Point2D(me.x, me.z)
                    
                    need_speed = delta_pos.len() / t

                    if 0.5 * ROBOT_MAX_GROUND_SPEED < need_speed \
                            and need_speed < ROBOT_MAX_GROUND_SPEED:
                        target_velocity = delta_pos.normalize() * need_speed
                        action.target_velocity_x = target_velocity.x
                        action.target_velocity_y = 0.0
                        action.target_velocity_z = target_velocity.y
                        action.jump_speed = ROBOT_MAX_JUMP_SPEED if jump else 0.0
                        action.use_nitro = False
                        return

        target_pos = Point2D(
            0.0, -(rules.arena.depth / 2.0) + rules.arena.bottom_radius)

        if game.ball.velocity_z < -EPS:
            t = (target_pos.y - game.ball.z) / game.ball.velocity_z
            x = game.ball.x + game.ball.velocity_x * t

            if abs(x) < rules.arena.goal_width / 2.0:
                target_pos.x = x

        target_velocity = Point2D(
            target_pos.x - me.x, target_pos.y - me.z) * ROBOT_MAX_GROUND_SPEED

        action.target_velocity_x = target_velocity.x
        action.target_velocity_y = 0.0
        action.target_velocity_z = target_velocity.y
        action.jump_speed = ROBOT_MAX_JUMP_SPEED if jump else 0.0
        action.use_nitro = False
