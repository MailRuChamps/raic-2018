/**
 * Created by Quake on 18.12.2018
 */
'use strict';

function goToSafeMode() {
    if (process.argv[6] === 'disable-fs') {
        process.kill = null;

        var path = require('path');

        var Module = require('module');
        var originalRequire = Module.prototype.require;
        Module.prototype.require = function (moduleName) {

            if (moduleName.indexOf(__dirname) !== 0) {
                moduleName = path.resolve(__dirname, moduleName);
            } else {
                moduleName = path.resolve(moduleName);
            }

            if (moduleName.indexOf(__dirname) !== 0) {
                throw moduleName + '; Modules import is restricted. Use require(__dirname+"/module.js") to import any modules. Ипорт модулей ограничен. Используйте (__dirname+"/module.js") для импорта модулей.';
            }

            return originalRequire(moduleName);

        };
        console.log('"fs" module disabled');
    }
}

var token = process.argv[4] || "0000000000000000";
var RemoteProcessClient = require(__dirname + '/remote-process-client.js');
var remoteProcessClient = new RemoteProcessClient.connect(process.argv[2] || '127.0.0.1', process.argv[3] || 31001, function onServerConnect() {
    if (MyStrategy.onLocalRunnerConnected) {
        MyStrategy.onLocalRunnerConnected();
    }
    if (process.env.DEBUG) {
        run();
    } else {
        try {
            run();
        } catch (e) {
            console.log('INITIALIZATION ERROR: ' + e.message);
            process.exit(1);
        }
    }
});
var strategy = null;
var rules;
var game;
var Action = require('./model/Action.js');

goToSafeMode();

var MyStrategy = require(__dirname + '/' + (process.argv[5] || './my-strategy.js'));

var isCallbackedStrategy = false;
var actions;
var callBackCount;
var _action;

function run() {
    remoteProcessClient.writeTokenMessage(token, function() {
        remoteProcessClient.readRules(function (v) {
            rules = v;

            remoteProcessClient.readGame(handleGameFrame)
        });
    });
    
    //isCallbackedStrategy = strategy.length === 5; //http proxy strategy with callback
}

function handleGameFrame(v) {
    game = v;
    strategy = MyStrategy.getInstance();
    isCallbackedStrategy = strategy.length === 5; //http proxy strategy with callback

    if (!game) {
        remoteProcessClient.close();
        process.exit(1);
    }

    callBackCount = 0;
    actions = [];

    for (let robot of game.robots) {
        if (robot.is_teammate) {
            actions[robot.id] = new Action();
            callStrategy(robot, rules, game, actions[robot.id]);
        }
    }
    if (!isCallbackedStrategy) {
        afterAllStrategyProcessed();
    }
}

function callStrategy(robot, rules, game, action) {
    if (isCallbackedStrategy) {
        callBackCount++;
        strategy(robot, rules, game, action, function (returnedAction) {
            _action = returnedAction;
            callBackCount--;
            if (callBackCount === 0) {
                afterAllStrategyProcessed();
            }
        });
    } else {
        strategy(robot, rules, game, action);
    }
}

function afterAllStrategyProcessed() {
    remoteProcessClient.write(actions);
    remoteProcessClient.readGame(handleGameFrame);
}