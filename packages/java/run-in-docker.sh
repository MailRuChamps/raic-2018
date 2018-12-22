set -e

cd /output && java -Xmx256m -jar ./java-cgdk-jar-with-dependencies.jar "$@"
