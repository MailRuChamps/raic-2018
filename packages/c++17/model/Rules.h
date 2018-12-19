#if defined(_MSC_VER) && (_MSC_VER >= 1200)
#pragma once
#endif

#ifndef _MODEL_RULES_H_
#define _MODEL_RULES_H_

#include "../rapidjson/document.h"
#include "Arena.h"

namespace model {
    struct Rules {
        int max_tick_count;
        Arena arena;
        int team_size;
        long long seed;
        double ROBOT_MIN_RADIUS;
        double ROBOT_MAX_RADIUS;
        double ROBOT_MAX_JUMP_SPEED;
        double ROBOT_ACCELERATION;
        double ROBOT_NITRO_ACCELERATION;
        double ROBOT_MAX_GROUND_SPEED;
        double ROBOT_ARENA_E;
        double ROBOT_RADIUS;
        double ROBOT_MASS;
        int TICKS_PER_SECOND;
        int MICROTICKS_PER_TICK;
        int RESET_TICKS;
        double BALL_ARENA_E;
        double BALL_RADIUS;
        double BALL_MASS;
        double MIN_HIT_E;
        double MAX_HIT_E;
        double MAX_ENTITY_SPEED;
        double MAX_NITRO_AMOUNT;
        double START_NITRO_AMOUNT;
        double NITRO_POINT_VELOCITY_CHANGE;
        double NITRO_PACK_X;
        double NITRO_PACK_Y;
        double NITRO_PACK_Z;
        double NITRO_PACK_RADIUS;
        double NITRO_PACK_AMOUNT;
        int NITRO_PACK_RESPAWN_TICKS;
        double GRAVITY;

        void read(const rapidjson::Value& json) {
            max_tick_count = json["max_tick_count"].GetInt();
            arena.read(json["arena"]);
            team_size = json["team_size"].GetInt();
            seed = json["seed"].GetInt64();
            ROBOT_MIN_RADIUS = json["ROBOT_MIN_RADIUS"].GetDouble();
            ROBOT_MAX_RADIUS = json["ROBOT_MAX_RADIUS"].GetDouble();
            ROBOT_MAX_JUMP_SPEED = json["ROBOT_MAX_JUMP_SPEED"].GetDouble();
            ROBOT_ACCELERATION = json["ROBOT_ACCELERATION"].GetDouble();
            ROBOT_NITRO_ACCELERATION = json["ROBOT_NITRO_ACCELERATION"].GetDouble();
            ROBOT_MAX_GROUND_SPEED = json["ROBOT_MAX_GROUND_SPEED"].GetDouble();
            ROBOT_ARENA_E = json["ROBOT_ARENA_E"].GetDouble();
            ROBOT_RADIUS = json["ROBOT_RADIUS"].GetDouble();
            ROBOT_MASS = json["ROBOT_MASS"].GetDouble();
            TICKS_PER_SECOND = json["TICKS_PER_SECOND"].GetInt();
            MICROTICKS_PER_TICK = json["MICROTICKS_PER_TICK"].GetInt();
            RESET_TICKS = json["RESET_TICKS"].GetInt();
            BALL_ARENA_E = json["BALL_ARENA_E"].GetDouble();
            BALL_RADIUS = json["BALL_RADIUS"].GetDouble();
            BALL_MASS = json["BALL_MASS"].GetDouble();
            MIN_HIT_E = json["MIN_HIT_E"].GetDouble();
            MAX_HIT_E = json["MAX_HIT_E"].GetDouble();
            MAX_ENTITY_SPEED = json["MAX_ENTITY_SPEED"].GetDouble();
            MAX_NITRO_AMOUNT = json["MAX_NITRO_AMOUNT"].GetDouble();
            START_NITRO_AMOUNT = json["START_NITRO_AMOUNT"].GetDouble();
            NITRO_POINT_VELOCITY_CHANGE = json["NITRO_POINT_VELOCITY_CHANGE"].GetDouble();
            NITRO_PACK_X = json["NITRO_PACK_X"].GetDouble();
            NITRO_PACK_Y = json["NITRO_PACK_Y"].GetDouble();
            NITRO_PACK_Z = json["NITRO_PACK_Z"].GetDouble();
            NITRO_PACK_RADIUS = json["NITRO_PACK_RADIUS"].GetDouble();
            NITRO_PACK_AMOUNT = json["NITRO_PACK_AMOUNT"].GetDouble();
            NITRO_PACK_RESPAWN_TICKS = json["NITRO_PACK_RESPAWN_TICKS"].GetInt();
            GRAVITY = json["GRAVITY"].GetDouble();
        }
    };
}

#endif