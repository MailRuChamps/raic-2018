if [ "$1" == "" ]; then
    echo This script is used for compiling on the server
    exit 1
fi

set -ex

if [ "$1" != "base" ]; then
    rm MyStrategy.js
    cp -rn /src/* ./
fi
