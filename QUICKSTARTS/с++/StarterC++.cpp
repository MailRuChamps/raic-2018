#include "MyStrategy.h"

using namespace model;

MyStrategy::MyStrategy() { }

void MyStrategy::act(const Robot& me, const Rules& rules, const Game& game, Action& action) {

    ball.set(game.ball.x, game.ball.z, game.ball.y);
    ball_v.set(game.ball.velocity_x, game.ball.velocity_z, game.ball.velocity_y);

    // Наша стратегия умеет играть только на земле
    // Поэтому, если мы не касаемся земли, будет использовать нитро
    // чтобы как можно быстрее попасть обратно на землю
    if ( !me.touch ) {
        action.target_velocity_x = 0.0;
        action.target_velocity_z = 0.0;
        action.target_velocity_y = -MAX_ENTITY_SPEED;
        action.jump_speed        = 0.0;
        action.use_nitro         = true;
        return;
    }

    // Если при прыжке произойдет столкновение с мячом, и мы находимся
    // с той же стороны от мяча, что и наши ворота, прыгнем, тем самым
    // ударив по мячу сильнее в сторону противника
    bool jump = (   ball.distTo(me.x, me.z, me.y) < (BALL_RADIUS + ROBOT_MAX_RADIUS)
                 && me.y < ball.y );

    // Так как роботов несколько, определим нашу роль - защитник, или нападающий
    // Нападающим будем в том случае, если есть дружественный робот,
    // находящийся ближе к нашим воротам
    bool is_attacker = false; // = (game.robots.size() == 2);
    for (const Robot &robot : game.robots) {
        if (   robot.is_teammate
            && robot.id != me.id) {
            if (robot.z < me.z) {
                is_attacker = true;
            }
        }
    }

    if (is_attacker) {
        // Стратегия нападающего:
        // Просимулирем примерное положение мяча в следующие 10 секунд, с точностью 0.1 секунда
        for (int i = 0; i < 100; ++i) {
            double t = i * 0.1;
            Point3D ball_pos = ball + ball_v*t;
            // Если мяч не вылетит за пределы арены
            // (произойдет столкновение со стеной, которое мы не рассматриваем),
            // и при этом мяч будет находится ближе к вражеским воротам, чем робот,
            if (   ball_pos.z > me.z
                && abs(ball.x) < (rules.arena.width / 2.0)
                && abs(ball.z) < (rules.arena.depth / 2.0) ) {
                // Посчитаем, с какой скоростью робот должен бежать,
                // Чтобы прийти туда же, где будет мяч, в то же самое время
                Point2D delta_pos(ball_pos.x - me.x, ball_pos.z - me.z);
                double delta_pos_dist = delta_pos.dist();
                double need_speed = delta_pos_dist / t;
                // Если эта скорость лежит в допустимом отрезке
                if (0.5 * ROBOT_MAX_GROUND_SPEED < need_speed
                    && need_speed < ROBOT_MAX_GROUND_SPEED ) {
                    // То это и будет наше текущее действие
                    Point2D target_velocity(delta_pos.normalize(delta_pos_dist)*need_speed);
                    action.target_velocity_x = target_velocity.x;
                    action.target_velocity_z = target_velocity.z;
                    action.target_velocity_y = 0.0;
                    action.jump_speed = jump ? ROBOT_MAX_JUMP_SPEED : 0.0;
                    action.use_nitro = false;
                    return;
                }
            }
        }
    }
    // Стратегия защитника (или атакующего, не нашедшего хорошего момента для удара):
    // Будем стоять посередине наших ворот
    Point2D target_pos(0.0, -(rules.arena.depth / 2.0) + rules.arena.bottom_radius);
    // Причем, если мяч движется в сторону наших ворот
    if (ball_v.z < -EPS) {
        // Найдем время и место, в котором мяч пересечет линию ворот
        double t = (target_pos.z - ball.z) / ball_v.z;
        double x = ball.x + ball_v.x * t;
        // Если это место - внутри ворот
        if (abs(x) < (rules.arena.goal_width / 2.0)) {
            // То пойдем защищать его
            target_pos.x = x;
        }
    }
    // Установка нужных полей для желаемого действия
    Point2D target_velocity(target_pos.x - me.x, target_pos.z - me.z);
    target_velocity *= ROBOT_MAX_GROUND_SPEED;

    action.target_velocity_x = target_velocity.x;
    action.target_velocity_z = target_velocity.z;
    action.target_velocity_y = 0.0;
    action.jump_speed = jump ? ROBOT_MAX_JUMP_SPEED : 0.0;
    action.use_nitro = false;
}
