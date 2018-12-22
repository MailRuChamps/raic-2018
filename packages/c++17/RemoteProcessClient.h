#if defined(_MSC_VER) && (_MSC_VER >= 1200)
#pragma once
#endif

#ifndef _REMOTE_PROCESS_CLIENT_H_
#define _REMOTE_PROCESS_CLIENT_H_

#include <unordered_map>
#include <memory>
#include <string>

#include "csimplesocket/ActiveSocket.h"

#include "model/Action.h"
#include "model/Game.h"
#include "model/Rules.h"

class RemoteProcessClient {
    CActiveSocket socket;
    std::string buffer;
    std::string readline();
    void writeline(std::string line);
public:
    RemoteProcessClient(std::string host, int port);
    std::unique_ptr<model::Rules> read_rules();
    std::unique_ptr<model::Game> read_game();
    void write(const std::unordered_map<int, model::Action>& actions, const std::string& custom_rendering);
    void write_token(const std::string& token);
};

#endif