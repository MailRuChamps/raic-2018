if [ "$1" == "" ]; then
    echo This script is used for compiling on the server
    exit 1
fi

set -ex

if [ "$1" != "base" ]; then
    rm MyStrategy.cs
    cp -rn /src/* ./
fi

dotnet build -c Release
cp bin/Release/netcoreapp2.1/* /output/