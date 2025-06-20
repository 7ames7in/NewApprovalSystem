#!/bin/bash

# 서비스명 (이미지 이름)
SERVICE_NAME=user-service

# Dockerfile 경로
DOCKERFILE=src/Services/UserService/API/Dockerfile

# 빌드 context
CONTEXT_DIR=.

# 컨테이너 포트 설정
PORT=7129

# Step 1: Build Docker Image
echo "=== Building $SERVICE_NAME image ==="
docker build -f $DOCKERFILE -t $SERVICE_NAME:latest $CONTEXT_DIR

# Step 2: Run Docker Container
echo "=== Running $SERVICE_NAME container ==="
docker run -d --name $SERVICE_NAME -p $PORT:8080 $SERVICE_NAME:latest

echo "=== $SERVICE_NAME running at http://localhost:$PORT ==="


# chmod +x user-service-run.sh
# ./user-service-run.sh