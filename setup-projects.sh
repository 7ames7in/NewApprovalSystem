#!/bin/bash
set -e

echo "📁 Creating full solution folder structure..."

# 주요 서비스 폴더
mkdir -p src/Services/ApprovalService/{API,Application,Domain,Infrastructure}
mkdir -p src/Services/AttachmentService/{API,Infrastructure}
mkdir -p src/Services/NotificationService/{API,Infrastructure}
mkdir -p src/Services/LoggingService/{API,Infrastructure}
mkdir -p src/Services/UserService/{API,Infrastructure,Domain}
mkdir -p src/BuildingBlocks/SharedKernel
mkdir -p tests

echo "🛠️ Initializing .NET projects..."

# ApprovalService Projects
dotnet new webapi -n ApprovalService.API -o src/Services/ApprovalService/API
dotnet new classlib -n ApprovalService.Application -o src/Services/ApprovalService/Application
dotnet new classlib -n ApprovalService.Domain -o src/Services/ApprovalService/Domain
dotnet new classlib -n ApprovalService.Infrastructure -o src/Services/ApprovalService/Infrastructure

# AttachmentService
dotnet new webapi -n AttachmentService.API -o src/Services/AttachmentService/API
dotnet new classlib -n AttachmentService.Infrastructure -o src/Services/AttachmentService/Infrastructure

# NotificationService
dotnet new webapi -n NotificationService.API -o src/Services/NotificationService/API
dotnet new classlib -n NotificationService.Infrastructure -o src/Services/NotificationService/Infrastructure

# LoggingService
dotnet new webapi -n LoggingService.API -o src/Services/LoggingService/API
dotnet new classlib -n LoggingService.Infrastructure -o src/Services/LoggingService/Infrastructure
dotnet new classlib -n LoggingService.Domain -o src/Services/LoggingService/Domain

# UserService
dotnet new webapi -n UserService.API -o src/Services/UserService/API
dotnet new classlib -n UserService.Domain -o src/Services/UserService/Domain
dotnet new classlib -n UserService.Infrastructure -o src/Services/UserService/Infrastructure

# SharedKernel
dotnet new classlib -n SharedKernel -o src/BuildingBlocks/SharedKernel

echo "✅ All projects and folders are created."
