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

docker stop rabbitmq || true
docker rm rabbitmq || true
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:4.1-management

docker stop redis || true
docker rm redis || true
docker run -d --name redis -p 6379:6379 redis:latest

echo "⏳ Waiting for SQL Server to be ready..."
sleep 10

echo "🔄 Applying EF migrations..."
dotnet ef database update \
  --project ./src/Identity/Orionexx.Identity.Infrastructure/ \
  --startup-project ./src/Identity/Orionexx.Identity.Service/

dotnet ef database update --project ./src/Workers/Orionexx.Events

echo "✅ Setup complete."
