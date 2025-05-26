#!/bin/bash
set -e

echo "📁 Creating solution folder structure..."
# mkdir -p src/Services/{ApprovalService/{API,Application,Domain,Infrastructure},AttachmentService/API,NotificationService/API,LoggingService/API,UserService/API}
mkdir -p src/Services/{AttachmentService/API,NotificationService/API,LoggingService/API,UserService/API}
# mkdir -p src/BuildingBlocks/SharedKernel
mkdir -p tests

echo "🛠️ Initializing each microservice API project..."
# dotnet new webapi -n ApprovalService.API -o src/Services/ApprovalService/API
# dotnet new classlib -n ApprovalService.Application -o src/Services/ApprovalService/Application
# dotnet new classlib -n ApprovalService.Domain -o src/Services/ApprovalService/Domain
# dotnet new classlib -n ApprovalService.Infrastructure -o src/Services/ApprovalService/Infrastructure

dotnet new webapi -n AttachmentService.API -o src/Services/AttachmentService/API


# dotnet new webapi -n NotificationService.API -o src/Services/NotificationService/API
dotnet new webapi -n LoggingService.API -o src/Services/LoggingService/API
dotnet new webapi -n UserService.API -o src/Services/UserService/API

#dotnet new classlib -n SharedKernel -o src/BuildingBlocks/SharedKernel

echo "✅ All services and structure initialized."



dotnet add src/Services/ApprovalService/ApprovalService.API/ApprovalService.API.csproj reference \
    src/ApprovalService.Application/AttachmentService.Domain.csproj 
    