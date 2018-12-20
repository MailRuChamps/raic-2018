/**
 * Created by Quake on 18.12.2018
 */
"use strict";

module.exports.getInstance = function () {
    //private strategy variables here;
    var __r;

    /**
     * Основной метод стратегии. Вызывается каждый тик.
     *
     * @param me  Информация о вашем роботе.
     * @param rules Правила.
     * @param game  Различные игровые константы.
     * @param action  Результатом работы метода является изменение полей данного объекта.
     */
    var initialized;
    var me, rules, game, action;

    var actionFunction = function (me, rules, game, action) {
        if (!initialized) {
            initializeStrategy(rules, game);
            initialized = true;
        }
        initializeTick(me, rules, game, action);

        actionFunc();
    };

    /**
     * Инциализируем стратегию.
     */
    var initializeStrategy = function (rules, game) {
        __r = rules.seed;
    };
    /**
     * Сохраняем все входные данные в полях замыкания для упрощения доступа к ним.
     */
    var initializeTick = function (me_, rules_, game_, action_) {
        me = me_;
        rules = rules_;
        game = game_;
        action = action_;
    };

    /**
     * Основная логика нашей стратегии.
     */
    var actionFunc = function () {
        //test
        //action.target_velocity_x = rules.ROBOT_MAX_GROUND_SPEED;
    }

    return actionFunction; //возвращаем функцию move, чтобы runner мог ее вызывать
};