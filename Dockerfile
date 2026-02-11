# Build stage - Compile MAUI app for Android
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS builder

# Install dependencies
RUN apt-get update && apt-get install -y \
    openjdk-17-jdk \
    wget \
    unzip \
    && rm -rf /var/lib/apt/lists/*

# Set Android SDK paths
ENV ANDROID_SDK_ROOT=/opt/android-sdk \
    ANDROID_HOME=/opt/android-sdk \
    PATH=${PATH}:/opt/android-sdk/cmdline-tools/latest/bin:/opt/android-sdk/platform-tools

# Create Android SDK directories
RUN mkdir -p ${ANDROID_SDK_ROOT}/cmdline-tools

# Download Android SDK Command-line Tools
RUN wget -q https://dl.google.com/android/repository/commandlinetools-linux-9477386_latest.zip -O /tmp/cmdline-tools.zip && \
    unzip -q /tmp/cmdline-tools.zip -d ${ANDROID_SDK_ROOT}/cmdline-tools && \
    mv ${ANDROID_SDK_ROOT}/cmdline-tools/cmdline-tools ${ANDROID_SDK_ROOT}/cmdline-tools/latest && \
    rm /tmp/cmdline-tools.zip

# Accept Android licenses
RUN yes | ${ANDROID_SDK_ROOT}/cmdline-tools/latest/bin/sdkmanager --licenses

# Install Android SDK packages
RUN ${ANDROID_SDK_ROOT}/cmdline-tools/latest/bin/sdkmanager \
    "platforms;android-34" \
    "build-tools;34.0.0" \
    "ndk;26.1.10909125" \
    "platform-tools"

# Install Android workload FIRST (before reading project)
RUN dotnet workload install android

# Copy project files
WORKDIR /src
COPY WeatherApp/ .

# Restore dependencies
RUN dotnet restore WeatherApp.csproj

# Build and Publish for Android
RUN dotnet publish WeatherApp.csproj -f net9.0-android -c Release -p:AndroidPackageFormat=apk

# Runtime stage - Just output the APK
FROM scratch AS runtime
COPY --from=builder /src/bin/Release/net9.0-android/publish/*.apk /

# Default stage - builder output
FROM builder
