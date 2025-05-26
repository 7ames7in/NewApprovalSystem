#!/bin/bash
set -e

SOLUTION_NAME="ApprovalSystem.sln"

echo "📦 Adding all projects to $SOLUTION_NAME while preserving folder structure..."

# ApprovalService
dotnet sln $SOLUTION_NAME add src/Services/ApprovalService/API/ApprovalService.API.csproj
dotnet sln $SOLUTION_NAME add src/Services/ApprovalService/Application/ApprovalService.Application.csproj
dotnet sln $SOLUTION_NAME add src/Services/ApprovalService/Domain/ApprovalService.Domain.csproj
dotnet sln $SOLUTION_NAME add src/Services/ApprovalService/Infrastructure/ApprovalService.Infrastructure.csproj

# AttachmentService
dotnet sln $SOLUTION_NAME add src/Services/AttachmentService/API/AttachmentService.API.csproj
dotnet sln $SOLUTION_NAME add src/Services/AttachmentService/Domain/AttachmentService.Domain.csproj
dotnet sln $SOLUTION_NAME add src/Services/AttachmentService/Infrastructure/AttachmentService.Infrastructure.csproj

# NotificationService
dotnet sln $SOLUTION_NAME add src/Services/NotificationService/API/NotificationService.API.csproj
dotnet sln $SOLUTION_NAME add src/Services/NotificationService/Infrastructure/NotificationService.Infrastructure.csproj

# LoggingService
dotnet sln $SOLUTION_NAME add src/Services/LoggingService/API/LoggingService.API.csproj
dotnet sln $SOLUTION_NAME add src/Services/LoggingService/Infrastructure/LoggingService.Infrastructure.csproj

# UserService
dotnet sln $SOLUTION_NAME add src/Services/UserService/API/UserService.API.csproj
dotnet sln $SOLUTION_NAME add src/Services/UserService/Domain/UserService.Domain.csproj
dotnet sln $SOLUTION_NAME add src/Services/UserService/Infrastructure/UserService.Infrastructure.csproj

# SharedKernel / Core
dotnet sln $SOLUTION_NAME add BuildingBlocks/Core/Infrastructure/Infrastructure.csproj

echo "✅ All projects added to $SOLUTION_NAME successfully."
