use model::*;
use strategy::Strategy;

const EPS: f64 = 1e-5;

// Константы, взятые из документации
const BALL_RADIUS: f64 = 2.0;
const ROBOT_MAX_RADIUS: f64 = 1.05;
const MAX_ENTITY_SPEED: f64 = 100.0;
const ROBOT_MAX_GROUND_SPEED: f64 = 30.0;
const ROBOT_MAX_JUMP_SPEED: f64 = 15.0;

#[derive(Default)]
pub struct MyStrategy;

// Нам понадобится работа с 2d векторами
#[derive(Copy, Clone, Debug)]
struct Vec2 {
    x: f64,
    y: f64,
}

impl Vec2 {
    fn new(x: f64, y: f64) -> Self {
        Self { x, y }
    }
    // Нахождение длины вектора
    fn len(&self) -> f64 {
        (self.x * self.x + self.y * self.y).sqrt()
    }
    // Нормализация вектора (приведение длины к 1)
    fn normalize(self) -> Self {
        self * (1.0 / self.len())
    }
}

// Операция - для векторов
impl std::ops::Sub for Vec2 {
    type Output = Self;
    fn sub(self, b: Self) -> Self {
        Self::new(self.x - b.x, self.y - b.y)
    }
}

// Операция + для векторов
impl std::ops::Add for Vec2 {
    type Output = Self;
    fn add(self, b: Self) -> Self {
        Self::new(self.x + b.x, self.y + b.y)
    }
}

// Операция умножения вектора на число
impl std::ops::Mul<f64> for Vec2 {
    type Output = Self;
    fn mul(self, k: f64) -> Self {
        Self::new(self.x * k, self.y * k)
    }
}

// Определение общего интерфейса для робота и мяча
trait Entity {
    // Нахождение позиции объекта в плоскости XZ
    fn position(&self) -> Vec2;
    // Нахождение скорости объекта в плоскости XZ
    fn velocity(&self) -> Vec2;
}

// Реализация интерфейса Entity для робота
impl Entity for Robot {
    fn position(&self) -> Vec2 {
        Vec2::new(self.x, self.z)
    }
    fn velocity(&self) -> Vec2 {
        Vec2::new(self.velocity_x, self.velocity_z)
    }
}

// Реализация интерфейса Entity для мяча
impl Entity for Ball {
    fn position(&self) -> Vec2 {
        Vec2::new(self.x, self.z)
    }
    fn velocity(&self) -> Vec2 {
        Vec2::new(self.velocity_x, self.velocity_z)
    }
}

// Код стратегии
impl Strategy for MyStrategy {
    fn act(&mut self, me: &Robot, rules: &Rules, game: &Game, action: &mut Action) {

        // Наша стратегия умеет играть только на земле
        // Поэтому, если мы не касаемся земли, будет использовать нитро
        // чтобы как можно быстрее попасть обратно на землю
        if !me.touch {
            *action = Action {
                target_velocity_x: 0.0,
                target_velocity_y: -MAX_ENTITY_SPEED,
                target_velocity_z: 0.0,
                jump_speed: 0.0,
                use_nitro: true,
            };
            return;
        }

        const JUMP_TIME: f64 = 0.2;
        const MAX_JUMP_HEIGHT: f64 = 3.0;

        // Если при прыжке произойдет столкновение с мячом, и мы находимся
        // с той же стороны от мяча, что и наши ворота, прыгнем, тем самым
        // ударив по мячу сильнее в сторону противника
        let mut jump = ((me.x - game.ball.x).powi(2)
            + (me.y - game.ball.y).powi(2)
            + (me.z - game.ball.z).powi(2))
        .sqrt()
            < BALL_RADIUS + ROBOT_MAX_RADIUS
            && me.z < game.ball.z;

        // Так как роботов несколько, определим нашу роль - защитник, или нападающий
        // Нападающим будем в том случае, если есть дружественный робот,
        // находящийся ближе к нашим воротам
        let mut is_attacker = game.robots.len() == 2;
        for robot in &game.robots {
            if robot.is_teammate && robot.id != me.id {
                if robot.position().y < me.position().y {
                    is_attacker = true;
                }
            }
        }

        if is_attacker {
            // Стратегия нападающего:
            // Просимулирем примерное положение мяча в следующие 10 секунд, с точностью 0.1 секунда
            for i in 1..100 {
                let t = i as f64 * 0.1;
                let ball_pos = game.ball.position() + game.ball.velocity() * t;
                // Если мяч не вылетит за пределы арены
                // (произойдет столкновение со стеной, которое мы не рассматриваем),
                // и при этом мяч будет находится ближе к вражеским воротам, чем робот,
                if ball_pos.y > me.position().y
                    && ball_pos.x.abs() < (rules.arena.width / 2.0)
                    && ball_pos.y.abs() < (rules.arena.depth / 2.0)
                {
                    // Посчитаем, с какой скоростью робот должен бежать,
                    // Чтобы прийти туда же, где будет мяч, в то же самое время
                    let delta_pos = Vec2::new(ball_pos.x, ball_pos.y)
                        - Vec2::new(me.position().x, me.position().y);
                    let need_speed = delta_pos.len() / t;
                    // Если эта скорость лежит в допустимом отрезке
                    if 0.5 * ROBOT_MAX_GROUND_SPEED < need_speed
                        && need_speed < ROBOT_MAX_GROUND_SPEED
                    {
                        // То это и будет наше текущее действие
                        let target_velocity =
                            Vec2::new(delta_pos.x, delta_pos.y).normalize() * need_speed;
                        *action = Action {
                            target_velocity_x: target_velocity.x,
                            target_velocity_y: 0.0,
                            target_velocity_z: target_velocity.y,
                            jump_speed: if jump { ROBOT_MAX_JUMP_SPEED } else { 0.0 },
                            use_nitro: false,
                        };
                        return;
                    }
                }
            }
        }
        // Стратегия защитника (или атакующего, не нашедшего хорошего момента для удара):
        // Будем стоять посередине наших ворот
        let mut target_pos = Vec2::new(0.0, -(rules.arena.depth / 2.0) + rules.arena.bottom_radius);
        // Причем, если мяч движется в сторону наших ворот
        if game.ball.velocity().y < -EPS {
            // Найдем время и место, в котором мяч пересечет линию ворот
            let t = (target_pos.y - game.ball.position().y) / game.ball.velocity().y;
            let x = game.ball.position().x + game.ball.velocity().x * t;
            // Если это место - внутри ворот
            if x.abs() < (rules.arena.goal_width / 2.0) {
                // То пойдем защищать его
                target_pos.x = x;
            }
        }

        // Установка нужных полей для желаемого действия
        let target_velocity = Vec2::new(
            target_pos.x - me.position().x,
            target_pos.y - me.position().y,
        ) * ROBOT_MAX_GROUND_SPEED;
        *action = Action {
            target_velocity_x: target_velocity.x,
            target_velocity_y: 0.0,
            target_velocity_z: target_velocity.y,
            jump_speed: if jump { ROBOT_MAX_JUMP_SPEED } else { 0.0 },
            use_nitro: false,
        }
    }
}