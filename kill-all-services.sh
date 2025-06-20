#!/bin/bash

echo "Stopping all dotnet services..."

# dotnet 프로세스를 찾아서 kill
ps aux | grep dotnet | grep -v grep | awk '{print $2}' | xargs kill -9

echo "=== All dotnet services stopped. ==="

# chmod +x kill-all-services.sh
# ./kill-all-services.sh