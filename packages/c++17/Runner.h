#if defined(_MSC_VER) && (_MSC_VER >= 1200)
#pragma once
#endif

#ifndef _RUNNER_H_
#define _RUNNER_H_

#include <string>

#include "RemoteProcessClient.h"

class Runner {
private:
    RemoteProcessClient remoteProcessClient;
    std::string token;
public:
    Runner(const char*, const char*, const char*);

    void run();
};

#endif
