if [ "$1" == "" ]; then
    echo This script is used for compiling on the server
    exit 1
fi

set -ex

if [ "$1" != "base" ]; then
    rm MyStrategy.py
    cp -rn /src/* ./
fi
find . -name '*.pyx' -exec cythonize -i {} \;
python -m py_compile Runner.py
cp -r * /output/
