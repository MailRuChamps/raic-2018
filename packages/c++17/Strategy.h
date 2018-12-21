#if defined(_MSC_VER) && (_MSC_VER >= 1200)
#pragma once
#endif

#ifndef _STRATEGY_H_
#define _STRATEGY_H_

#include "model/Rules.h"
#include "model/Game.h"
#include "model/Action.h"
#include "model/Robot.h"

class Strategy {
public:
    virtual void act(const model::Robot& me, const model::Rules& rules, const model::Game& game, model::Action& action) = 0;
    virtual std::string custom_rendering() { return ""; }

    virtual ~Strategy();
};

#endif
