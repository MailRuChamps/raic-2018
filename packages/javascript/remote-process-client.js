/**
 * Created by Quake on 18.12.2018
 */
'use strict';

const Game = require('./model/Game');
const Action = require('./model/Action');
const Rules = require('./model/Rules');

let client = null;
let strBuffer = [];

module.exports.connect = function connect(host, port, onConnect) {
    const net = require('net');
    var request = [];
    var busy;

    client = net.connect(port, host, function connectHandler() {
        // 'connect' listener
        console.log('connected to server!');
        writeline("json");
        onConnect();
    });
    client.setNoDelay();

    client.on('data', dataHandler);
    client.on('error', function onError(e) {
        console.log("SOCKET ERROR: " + e.message);
        process.exit(1);
    });

    client.on('close', function onClose() {
        console.log('server closed connection.');
        client.unref();
        process.exit();
    });
    client.on('end', function onEnd() {
        console.log('disconnected from server');
        process.exit();
    });

    function dataHandler(data) {
        busy = true;
        if (data) {
            strBuffer = strBuffer.concat(data.toString().split("\n").filter((item) => { return item.length }));
        }
        while (request.length && strBuffer.length) {
            let cb = request.shift();
            let data = strBuffer.shift();
            cb(data);
        }
        busy = false;
    }

    function readline() {
        return strBuffer.shift();
    }
    
    function writeline(line) {
        line += "\n";
        client.write(line);
    }

    function readSequence(cb) {
        if (!cb) {
            throw 'Callback expected';
        }
        request.push(cb);
        if (!busy) {
            dataHandler();
        }
    }
    
    this.readRules = function(cb) {
        readSequence(function(data) {
            let rules = new Rules();
            rules.read(data);
            
            cb(rules);
        });
    }
    
    this.readGame = function(cb) {
        readSequence(function(data) {
            let game = new Game();
            game.read(data);
            
            cb(game);
        });
    }
    
    this.write = function(actions) {
        let map = {};
        for (let id in actions) {
            map[id] = actions[id].toObject();
        }
        let json = JSON.stringify(map);
        writeline(json);
    }
    
    this.writeTokenMessage = function(token, cb) {
        writeline(token);
        cb();
    }
}