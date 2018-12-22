namespace FSharpCgdk

open FSharpCgdk.Model
open FSharp.Json

// Данные, в которых будем хранить атакующего бота, чтобы не пересчитывать дважды
// forTick - для какого тика актуальна информация, botId - id атакующего бота
type StrategyData = KnownAttackBot of forTick: int * botId : int 


module MyStrategy = 
    // Эпсилон! :D
    let private EPS = 0.1

    // Краткое описание собственной веткорной алгебры.
    type Vector = V3 of X : float * Y : float * Z : float

    let vX (V3(x, y, z)) = x
    let vY (V3(x, y, z)) = y
    let vZ (V3(x, y, z)) = z

    let add (V3(x1, y1, z1)) (V3(x2, y2, z2)) = 
        V3(x1 + x2, y1 + y2, z1 + z2)

    let sub (V3(x1, y1, z1)) (V3(x2, y2, z2)) = 
        V3(x1 - x2, y1 - y2, z1 - z2)

    let mulScalar (V3(x, y, z)) mul = V3(x * mul, y * mul, z * mul)
    let divScalar vector div = mulScalar vector (1.0/div)

    let length (V3(x, y, z)) = sqrt (x * x + y * y + z * z)

    let projectionXZ (V3(x, y, z)) = V3(x, 0.0, z)

    let positionBall (ball : Ball) = V3(ball.x, ball.y, ball.z)    
    let positionRobot (robot : Robot) = V3(robot.x, robot.y, robot.z)   

    let velocityBall (ball : Ball) = V3(ball.velocity_x, ball.velocity_y, ball.velocity_z)    
    let velocityRobot (robot : Robot) = V3(robot.velocity_x, robot.velocity_y, robot.velocity_z)   


    // Та самая хранимая информация.
    // Инициализируем не существующим тиком.
    let mutable data : StrategyData = KnownAttackBot(-1, -1)

    // Дальше следует читать программу снизу вверх для лучшего понимания
    // Последовательность чтения:
    // act -> nextData ->  attackAct -> entryAttackPoint -> defeatAct


    let protectAct (me, rules, game, _) : Action =

        // Вычисляем куда встать как вратари на проекции оси OZ
        let target_pos_z = -(rules.arena.depth / 2.0) + rules.arena.bottom_radius
        // Вычисляем куда встать на проекции OX
        let time = (target_pos_z - game.ball.z) / game.ball.velocity_z  
        let x = game.ball.x + game.ball.velocity_x * time

        // Сама точка перехвата
        let interception = 
            if game.ball.velocity_z > -EPS then 
                V3(0.0, 0.0, target_pos_z)
            else
                V3(x, 0.0, target_pos_z)

                
        let max_speed = rules.ROBOT_MAX_GROUND_SPEED
        
        // Узнаем положение робота и мяча без высоты 
        let me_pos = 
            positionRobot me
            |> projectionXZ
        let ball_pos = 
            positionBall game.ball
            |> projectionXZ

        let target_delta = sub interception me_pos
        let target_velocity = divScalar target_delta max_speed

        let delta = sub ball_pos me_pos

        let dist_for_touch = rules.ROBOT_RADIUS + rules.BALL_RADIUS
        let max_jump_speed = rules.ROBOT_MAX_JUMP_SPEED
        // Если робот касается мяча и находится ниже его,
        // то прыгаем как можно сильнее, чтобы пнуть его.
        let jump_speed = 
            if length delta <= dist_for_touch && me.y < game.ball.y then 
                max_jump_speed
            else
                 0.0

        {
            target_velocity_x = vX target_velocity
            target_velocity_y = 0.0
            target_velocity_z = vZ target_velocity
            jump_speed = jump_speed
            use_nitro = true
        }


    let attackEntryPoint (me, rules, game, _) : Action option = 
        // Расстояние между мячом и роботом.
        let delta = sub (positionBall game.ball) (positionRobot me)

        let dist_for_touch = rules.ROBOT_RADIUS + rules.BALL_RADIUS
        let max_jump_speed = rules.ROBOT_MAX_JUMP_SPEED
        // Если робот касается мяча и находится ниже его,
        // то прыгаем как можно сильнее, чтобы пнуть его.
        let jump_speed = 
            if length delta <= dist_for_touch && me.y < game.ball.y then 
                max_jump_speed
            else
                 0.0
        
        // Узнаем положение робота и мяча и скорость мяча без высоты 
        let me_pos = 
            positionRobot me
            |> projectionXZ
        let ball_pos =
            positionBall game.ball
            |> projectionXZ
        let ball_vel =
            velocityBall game.ball
            |> projectionXZ

        // Провекрка возможности требуемой скорости
        let max_speed = rules.ROBOT_MAX_GROUND_SPEED
        let isPossibleSpeed speed = 
            0.5 * max_speed < speed && speed < max_speed

        // Узнаем положение мяча через определенное время
        let delta_ball_pos = mulScalar ball_vel 0.1
        let ballPosWithTime t  = 
            let time = float t * 0.1
            let new_delta_ball_pos = mulScalar delta_ball_pos time
            let new_pos = add ball_pos new_delta_ball_pos
            (new_pos, time)

        let ballInArena (ball_pos : Vector, _) = 
            abs(vX ball_pos) < rules.arena.width / 2.0 
            && abs(vZ ball_pos) < rules.arena.depth / 2.0

        // Для текущего положение мяча без высоты и времени
        // пытаемся узнать, сможем ли мы добежать до мяча чтобы пнуть его
        let tryAttack (ball_pos : Vector, time : float) = 
            // расстояние, требуемая скорость и вектор требуемой скорости
            let delta2 = sub ball_pos me_pos
            let need_speed = length delta2 / time
            let velocity = divScalar delta2 time
            if isPossibleSpeed need_speed then
                Some {                
                    target_velocity_x = vX velocity
                    target_velocity_y = vY velocity
                    target_velocity_z = vZ velocity
                    jump_speed = jump_speed
                    use_nitro = true
                }
            else
                None  

        // Перебираем положение мяча в будущем, 
        // до тех пор пока не выйдет за границы арены,
        // беря первый попавшийся вариант, в котором можем сможем добежать до мяча.
        Array.init 100 ballPosWithTime
        |> Array.takeWhile ballInArena
        |> Array.tryPick tryAttack


    let attackAct args : Action =
        // Если нашли коим образом можно атаковать,
        // то возвращаем это действие,
        // иначе переходим в режим защиты
        match attackEntryPoint args with
        | Some x -> x
        | None -> protectAct args


    let private tuple2 x y = (x, y)


    let nextData game : StrategyData = 
        let cur_tick = game.current_tick
        let tick = 
            match data with
            | KnownAttackBot(x, _) -> x 
        // Если информация актуальна то ничего не делаем,
        // иначе атакующим будет ближайший к воротам
        if cur_tick = tick then
            data
        else
            game.robots
            |> Array.filter (fun x -> x.is_teammate)
            |> Array.maxBy (fun x -> x.z)
            |> (fun x -> x.id)
            |> tuple2 cur_tick
            |> KnownAttackBot


    let assignFieldsAction toActoin fromAction = 
        toActoin.target_velocity_x <- fromAction.target_velocity_x
        toActoin.target_velocity_y <- fromAction.target_velocity_y
        toActoin.target_velocity_z <- fromAction.target_velocity_z
        toActoin.jump_speed <- fromAction.jump_speed
        toActoin.use_nitro <- fromAction.use_nitro

    let act (me : Robot, rules : Rules, game : Game, action : Action) =
        // Обновляем в случае необходимости id атакующего робота.
        data <- nextData game
        // Проверяем атакующий ли сейчас робот 
        let idAttackBot =
            match data with
            | KnownAttackBot(_, x) -> x 
        let newAction =         
            if me.id = idAttackBot then
                attackAct (me, rules, game, action)
            else
                protectAct (me, rules, game, action)
        assignFieldsAction action newAction
