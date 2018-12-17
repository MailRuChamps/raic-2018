#if defined(_MSC_VER) && (_MSC_VER >= 1200)
#pragma once
#endif

#ifndef _MODEL_ARENA_H_
#define _MODEL_ARENA_H_

#include "../rapidjson/document.h"

namespace model {
    struct Arena {
        double width;
        double height;
        double depth;
        double bottom_radius;
        double top_radius;
        double corner_radius;
        double goal_top_radius;
        double goal_width;
        double goal_height;
        double goal_depth;
        double goal_side_radius;

        void read(const rapidjson::Value& json) {
            width = json["width"].GetDouble();
            height = json["height"].GetDouble();
            depth = json["depth"].GetDouble();
            bottom_radius = json["bottom_radius"].GetDouble();
            top_radius = json["top_radius"].GetDouble();
            corner_radius = json["corner_radius"].GetDouble();
            goal_top_radius = json["goal_top_radius"].GetDouble();
            goal_width = json["goal_width"].GetDouble();
            goal_height = json["goal_height"].GetDouble();
            goal_depth = json["goal_depth"].GetDouble();
            goal_side_radius = json["goal_side_radius"].GetDouble();
        }
    };
}

#endif