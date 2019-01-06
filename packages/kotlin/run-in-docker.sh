set -e

cd /output && java -Xmx250m -XX:+DoEscapeAnalysis -jar ./kotlin-cgdk-jar-with-dependencies.jar "$@"
