#!/usr/bin/env bash
# exit on error
set -o errexit

# Build the application
dotnet restore
dotnet build -c Release
dotnet publish -c Release -o out

# Copy the published app to the output directory
cp -r out/* . 