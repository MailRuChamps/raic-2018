set -e

cd /output && java -Xmx250m -jar ./scala-cgdk-jar-with-dependencies.jar "$@"