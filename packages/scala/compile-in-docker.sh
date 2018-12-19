if [ "$1" == "" ]; then
    echo This script is used for compiling on the server
    exit 1
fi

set -ex

if [ "$1" != "base" ]; then
    rm src/main/scala/MyStrategy.scala
    cp -rn /src/* src/main/scala/
fi
mvn package --batch-mode
cp target/scala-cgdk-jar-with-dependencies.jar /output/