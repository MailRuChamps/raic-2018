set -e

cd /output && java -Xmx250m -jar ./kotlin-cgdk-jar-with-dependencies.jar "$@"