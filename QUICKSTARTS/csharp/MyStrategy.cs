using System;
using Com.CodeGame.CodeBall2018.DevKit.CSharpCgdk.Model;
using Action = Com.CodeGame.CodeBall2018.DevKit.CSharpCgdk.Model.Action;

namespace Com.CodeGame.CodeBall2018.DevKit.CSharpCgdk
{
    // Логика стратегии скопирована из базовой стратегии c++ (SannikovDmitry)
    public sealed class MyStrategy : IStrategy
    {
        private const double EPS = 1e-5;
        
        public void Act(Robot me, Rules rules, Game game, Action action)
        {
            var ball = game.ball;

            // Наша стратегия умеет играть только на земле
            // Поэтому, если мы не касаемся земли, будет использовать нитро
            // чтобы как можно быстрее попасть обратно на землю
            if (!me.touch)
            {
                action.target_velocity_x = 0.0;
                action.target_velocity_z = 0.0;
                action.target_velocity_y = -rules.MAX_ENTITY_SPEED;
                action.jump_speed = 0.0;
                action.use_nitro = true;
                return;
            }

            // Если при прыжке произойдет столкновение с мячом, и мы находимся
            // с той же стороны от мяча, что и наши ворота, прыгнем, тем самым
            // ударив по мячу сильнее в сторону противника
            bool jump = (DistBetweenBallAndRobot3D(ball, me) < (rules.BALL_RADIUS + rules.ROBOT_MAX_RADIUS) &&
                         me.y < ball.y);

            // Так как роботов несколько, определим нашу роль - защитник, или нападающий
            // Нападающим будем в том случае, если есть дружественный робот,
            // находящийся ближе к нашим воротам
            bool is_attacker = false;
            foreach (var robot in game.robots)
            {
                if (robot.is_teammate && robot.id != me.id)
                {
                    if (robot.z < me.z)
                    {
                        is_attacker = true;
                    }
                }
            }

            if (is_attacker)
            {
                // Стратегия нападающего:
                // Просимулирем примерное положение мяча в следующие 10 секунд, с точностью 0.1 секунда
                for (int i = 0; i < 100; ++i)
                {
                    double t = i * 0.1;
                    Ball ball_pos = CalcNewBallPos(ball, t);
                    // Если мяч не вылетит за пределы арены
                    // (произойдет столкновение со стеной, которое мы не рассматриваем),
                    // и при этом мяч будет находится ближе к вражеским воротам, чем робот,
                    if (ball_pos.z > me.z
                        && Math.Abs(ball.x) < (rules.arena.width / 2.0)
                        && Math.Abs(ball.z) < (rules.arena.depth / 2.0))
                    {
                        // Посчитаем, с какой скоростью робот должен бежать,
                        // Чтобы прийти туда же, где будет мяч, в то же самое время
                        double delta_pos_x = ball_pos.x - me.x;
                        double delta_pos_z = ball_pos.z - me.z;
                        double delta_pos_dist = Math.Sqrt(delta_pos_x*delta_pos_x + delta_pos_z*delta_pos_z);
                        double need_speed = delta_pos_dist / t;
                        // Если эта скорость лежит в допустимом отрезке
                        if (0.5 * rules.ROBOT_MAX_GROUND_SPEED < need_speed
                            && need_speed < rules.ROBOT_MAX_GROUND_SPEED)
                        {
                            // То это и будет наше текущее действие
                            action.target_velocity_x = delta_pos_x / delta_pos_dist * need_speed;
                            action.target_velocity_z = delta_pos_z / delta_pos_dist * need_speed;
                            action.target_velocity_y = 0.0;
                            action.jump_speed = jump ? rules.ROBOT_MAX_JUMP_SPEED : 0.0;
                            action.use_nitro = false;
                            return;
                        }
                    }
                }
            }
            
            // Стратегия защитника (или атакующего, не нашедшего хорошего момента для удара):
            // Будем стоять посередине наших ворот
            double target_pos_x = 0.0;
            double target_pos_z = -(rules.arena.depth / 2.0) + rules.arena.bottom_radius;
            // Причем, если мяч движется в сторону наших ворот
            if (ball.velocity_z < -EPS) {
                // Найдем время и место, в котором мяч пересечет линию ворот
                double t = (target_pos_z - ball.z) / ball.velocity_z;
                double x = ball.x + ball.velocity_x * t;
                // Если это место - внутри ворот
                if (Math.Abs(x) < (rules.arena.goal_width / 2.0)) {
                    // То пойдем защищать его
                    target_pos_x = x;
                }
            }
            
            // Установка нужных полей для желаемого действия
            double target_velocity_x = (target_pos_x - me.x)*rules.ROBOT_MAX_GROUND_SPEED;
            double target_velocity_z = (target_pos_z - me.z)*rules.ROBOT_MAX_GROUND_SPEED;
            
            action.target_velocity_x = target_velocity_x;
            action.target_velocity_z = target_velocity_z;
            action.target_velocity_y = 0.0;
            action.jump_speed = jump ? rules.ROBOT_MAX_JUMP_SPEED : 0.0;
            action.use_nitro = false;
        }

        public string CustomRendering()
        {
            return "";
        }
        
        private static double DistBetweenBallAndRobot3D(Ball b, Robot r)
        {
            return Math.Sqrt((b.x - r.x) * (b.x - r.x) + (b.y - r.y) * (b.y - r.y) + (b.z - r.z) * (b.z - r.z));
        }

        private static double DistBetweenBallAndRobot2D(Ball b, Robot r)
        {
            return Math.Sqrt((b.x - r.x) * (b.x - r.x) + (b.z - r.z) * (b.z - r.z));
        }

        private static Ball CalcNewBallPos(Ball b, double t)
        {
            return new Ball() {x = b.x + b.velocity_x * t, y = b.y + b.velocity_y * t, z = b.z + b.velocity_z * t};
        }
    }
}
