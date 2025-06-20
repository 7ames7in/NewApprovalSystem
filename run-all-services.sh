#!/bin/bash

# 서비스별 실행
echo "Starting ApprovalService..."
dotnet run --project src/Services/ApprovalService/API/Volvo.Service.Approval.API.csproj --launch-profile https &

echo "Starting AttachmentService..."
dotnet run --project src/Services/AttachmentService/API/Volvo.Service.Attachment.API.csproj --launch-profile https &

echo "Starting LoggingService..."
dotnet run --project src/Services/LoggingService/API/Volvo.Service.Logging.API.csproj --launch-profile https &

echo "Starting NotificationService..."
dotnet run --project src/Services/NotificationService/API/Volvo.Service.Notification.API.csproj --launch-profile https &

echo "Starting UserService..."
dotnet run --project src/Services/UserService/API/Volvo.Service.User.API.csproj --launch-profile https &

echo "Starting EmailService..."
dotnet run --project src/Services/EmailService/API/Volvo.Service.Email.API.csproj --launch-profile https &

echo "Starting ApprovalWeb..."
dotnet run --project src/UI/ApprovalWeb/Volvo.Web.Approval.csproj --launch-profile https &


echo "=== All services are starting... ==="

# 현재 프로세스 보여주기 (선택사항)
ps aux | grep dotnet

# chmod +x run-all-services.sh
# ./run-all-services.sh