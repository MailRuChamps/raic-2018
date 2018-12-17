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

        void read(const rapidjson::Value& json) {
            max_tick_count = json["max_tick_count"].GetInt();
            arena.read(json["arena"]);
        }
    };
}

#endif