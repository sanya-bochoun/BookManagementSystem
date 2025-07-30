#!/usr/bin/env bash
# exit on error
set -o errexit

# Build the application
dotnet build --configuration Release

# Publish the application
dotnet publish --configuration Release --output ./publish

# Copy the published files to the output directory
cp -r ./publish/* ./ 