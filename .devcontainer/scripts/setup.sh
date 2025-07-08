#!/bin/bash
set -e

echo "📦 Installing EF Tools..."
dotnet tool install --global dotnet-ef
dotnet dev-certs https --trust

# Install Dapr CLI
dapr uninstall
dapr init

# Migrate the database
echo "🔄 Applying database migrations..."
dotnet ef database update --project ./src/Identity/Orionexx.Identity.Infrastructure/ --startup-project ./src/Identity/Orionexx.Identity.Grpc/

export PATH="$PATH:/home/vscode/.dotnet/tools"