#if defined(_MSC_VER) && (_MSC_VER >= 1200)
#pragma once
#endif

#ifndef _MODEL_ACTION_H_
#define _MODEL_ACTION_H_

#include <string>
#include "../rapidjson/document.h"

namespace model {
    struct Action {
        double target_velocity_x;
        double target_velocity_y;
        double target_velocity_z;
        double jump_speed;
        bool use_nitro;

        Action() {
            this->target_velocity_x = 0.0;
            this->target_velocity_y = 0.0;
            this->target_velocity_z = 0.0;
            this->jump_speed = 0.0;
            this->use_nitro = false;
        }

        rapidjson::Value to_json(rapidjson::Document::AllocatorType& allocator) const {
            rapidjson::Value json;
            json.SetObject();
            json.AddMember("target_velocity_x", target_velocity_x, allocator);
            json.AddMember("target_velocity_y", target_velocity_y, allocator);
            json.AddMember("target_velocity_z", target_velocity_z, allocator);
            json.AddMember("jump_speed", jump_speed, allocator);
            json.AddMember("use_nitro", use_nitro, allocator);
            return json;
        }
    };
}

#endif
