set -ex

KERAS_BACKEND=theano cd /output && python ./Runner.py "$@"
