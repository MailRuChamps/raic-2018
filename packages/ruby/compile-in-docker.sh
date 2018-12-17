if [ "$1" == "" ]; then
    echo This script is used for compiling on the server
    exit 1
fi

set -ex

if [ "$1" != "base" ]; then
    rm my_strategy.rb
    cp -rn /src/* ./
fi
ruby -c runner.rb
cp -r * /output/
