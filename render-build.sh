#!/usr/bin/env bash
# exit on error
set -o errexit

# Build the application
dotnet restore
dotnet build -c Release -o out
dotnet publish -c Release -o out

# Clean up build artifacts
rm -rf obj/
rm -rf bin/
