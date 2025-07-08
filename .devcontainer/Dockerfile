FROM mcr.microsoft.com/devcontainers/dotnet:1-9.0

RUN apt-get update && apt-get install -y \
    curl \
    gnupg \
    unzip \
    && rm -rf /var/lib/apt/lists/*

ENV PATH="$PATH:/root/.dotnet/tools"

