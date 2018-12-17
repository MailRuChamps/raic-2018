#if defined(_MSC_VER) && (_MSC_VER >= 1200)
#pragma once
#endif

#ifndef _MODEL_ROBOT_H_
#define _MODEL_ROBOT_H_

#include "../rapidjson/document.h"

namespace model {
    struct Robot {
        int id;
        int player_id;
        bool is_teammate;
        double x;
        double y;
        double z;
        double velocity_x;
        double velocity_y;
        double velocity_z;
        double radius;
        double nitro_amount;
        bool touch;
        double touch_normal_x;
        double touch_normal_y;
        double touch_normal_z;

        void read(const rapidjson::Value& json) {
            id = json["id"].GetInt();
            player_id = json["player_id"].GetInt();
            is_teammate = json["is_teammate"].GetBool();
            x = json["x"].GetDouble();
            y = json["y"].GetDouble();
            z = json["z"].GetDouble();
            velocity_x = json["velocity_x"].GetDouble();
            velocity_y = json["velocity_y"].GetDouble();
            velocity_z = json["velocity_z"].GetDouble();
            radius = json["radius"].GetDouble();
            nitro_amount = json["nitro_amount"].GetDouble();
            touch = json["touch"].GetBool();
            if (touch) {
                touch_normal_x = json["touch_normal_x"].GetDouble();
                touch_normal_y = json["touch_normal_y"].GetDouble();
                touch_normal_z = json["touch_normal_z"].GetDouble();
            }
        }
    };
}

#endif