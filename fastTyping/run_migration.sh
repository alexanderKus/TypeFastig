#!/bin/bash

if [ $# -eq 1 ]; then
  migration_name="$1"
  echo "dotnet ef migrations add $migration_name -p src/Infrastructure/Infrastructure.csproj -s src/Api/Api.csproj"
  dotnet ef migrations add $migration_name -p src/Infrastructure/Infrastructure.csproj -s src/Api/Api.csproj
else 
  echo "USAGE: program <migration_name>"
fi