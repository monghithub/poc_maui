#!/bin/bash

# Build Weather App APK using Docker
# This script compiles the Android APK inside a Docker container

set -e

echo "🐳 Building Weather App APK with Docker..."
echo ""

# Check if Docker is installed
if ! command -v docker &> /dev/null; then
    echo "❌ Docker is not installed"
    echo "Install Docker from: https://docs.docker.com/get-docker/"
    exit 1
fi

# Build Docker image
echo "📦 Building Docker image..."
docker build -t weatherapp-builder:latest .

# Create output directory
mkdir -p output

# Run builder container and extract APK
echo "🔨 Compiling APK inside Docker..."
docker run --rm \
    -v $(pwd)/output:/output \
    weatherapp-builder:latest \
    bash -c "find /src/bin -name '*.apk' -type f -exec cp {} /output/ \; && ls -lh /output/"

echo ""
echo "✅ Build completed!"
echo ""
echo "APK location: $(pwd)/output/com.monghit.weatherapp.apk"
echo ""
echo "Install with:"
echo "  adb install -r output/com.monghit.weatherapp.apk"
