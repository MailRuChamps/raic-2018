if [ "$1" == "" ]; then
    echo This script is used for compiling on the server
    exit 1
fi

set -ex

if [ "$1" != "base" ]; then
    rm src/my_strategy.rs
    cp -rn /src/* src/
fi

cargo build --release --frozen
cp target/release/my-strategy /output/