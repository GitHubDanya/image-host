#!/bin/bash
docker run -d \
  --restart unless-stopped \
  -p 9000:8080 \
  -v "$(pwd)/ImageStorage:/app/ImageStorage" \
  -e ASPNETCORE_ENVIRONMENT=Development \
  --name image-host image-host
