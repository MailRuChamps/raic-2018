set -e

cd /output && java -Xmx250m -jar ./java-cgdk-jar-with-dependencies.jar "$@"