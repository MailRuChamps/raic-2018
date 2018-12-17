if [ "$1" == "" ]; then
    echo This script is used for compiling on the server
    exit 1
fi

set -ex

if [ "$1" != "base" ]; then
    rm src/main/java/MyStrategy.java
    cp -rn /src/* src/main/java/
fi
mvn package --batch-mode
cp target/java-cgdk-jar-with-dependencies.jar /output/