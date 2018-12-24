/**
 * Created by Quake on 18.12.2018
 */
"use strict";

module.exports.getInstance = function () {
    //private strategy variables here;

    /**
     * Основной метод стратегии. Вызывается каждый тик.
     *
     * @param me  Информация о вашем роботе.
     * @param rules Игровые константы.
     * @param game  Текущее состояние игровых объектов.
     * @param action  Результатом работы метода является изменение полей данного объекта.
     */

    var act = function (me, rules, game, action) {
        
    };

    var customRendering = function () {
        return "";
    };

    return {
        act: act, //возвращаем функцию act, чтобы runner мог ее вызывать
        customRendering: customRendering //фукнция customRendering для отрисовки отладочной информации
    }
};