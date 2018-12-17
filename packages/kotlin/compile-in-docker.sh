if [ "$1" == "" ]; then
    echo This script is used for compiling on the server
    exit 1
fi

set -ex

if [ "$1" != "base" ]; then
    rm src/main/kotlin/MyStrategy.kt
    cp -rn /src/* src/main/kotlin/
fi
mvn package --batch-mode
cp target/kotlin-cgdk-jar-with-dependencies.jar /output/