#!/bin/bash
set -e

# github codespace setup script
sudo chmod -R 777 /home/vscode
mkdir -p /home/vscode/.vscode-remote/data/Machine
touch /home/vscode/.vscode-remote/data/Machine/settings.json
echo "{}" > /home/vscode/.vscode-remote/data/Machine/settings.json

export PATH="$PATH:/home/vscode/.dotnet/tools"

echo "📦 Installing EF Tools..."
dotnet tool install --global dotnet-ef || echo "dotnet-ef already installed"

echo "🔐 Trusting HTTPS dev certs..."
dotnet dev-certs https --trust || true

echo "🔁 Reinitializing Dapr..."
dapr uninstall --all || true
dapr init --slim

# Resolved this error when starting shell in codespace:
mkdir -p ~/.dapr
dapr completion bash > ~/.dapr/completion.bash.inc

echo "🐳 Restarting SQL Server container..."
docker stop sqlserver || true
docker rm sqlserver || true
docker run -e "ACCEPT_EULA=Y" \
  -e "SA_PASSWORD=#SqlHardPass1" \
  -p 1433:1433 \
  --name sqlserver \
  -d mcr.microsoft.com/mssql/server:2022-latest

echo "⏳ Waiting for SQL Server to be ready..."
sleep 10

echo "🔄 Applying EF migrations..."
dotnet ef database update \
  --project ./src/Identity/Orionexx.Identity.Infrastructure/ \
  --startup-project ./src/Identity/Orionexx.Identity.Service/

echo "✅ Setup complete."
