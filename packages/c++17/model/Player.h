#if defined(_MSC_VER) && (_MSC_VER >= 1200)
#pragma once
#endif

#ifndef _MODEL_PLAYER_H_
#define _MODEL_PLAYER_H_

#include "../rapidjson/document.h"

namespace model {
    struct Player {
        int id;
        bool me;
        bool strategy_crashed;
        int score;

        void read(const rapidjson::Value& json) {
            id = json["id"].GetInt();
            me = json["me"].GetBool();
            strategy_crashed = json["strategy_crashed"].GetBool();
            score = json["score"].GetInt();
        }
    };
}

#endif