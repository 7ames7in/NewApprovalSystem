#!/bin/bash
echo "🔧 Microservices NuGet 패키지 설치 중..."

# === AttachmentService ===
dotnet add src/Services/AttachmentService/Application/AttachmentService.Application.csproj package MediatR
dotnet add src/Services/AttachmentService/Application/AttachmentService.Application.csproj package FluentValidation
dotnet add src/Services/AttachmentService/Application/AttachmentService.Application.csproj package AutoMapper.Extensions.Microsoft.DependencyInjection
dotnet add src/Services/AttachmentService/Application/AttachmentService.Application.csproj package Microsoft.Extensions.Logging
dotnet add src/Services/AttachmentService/Infrastructure/AttachmentService.Infrastructure.csproj package Microsoft.EntityFrameworkCore
dotnet add src/Services/AttachmentService/Infrastructure/AttachmentService.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Design
dotnet add src/Services/AttachmentService/Infrastructure/AttachmentService.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Sqlite
dotnet add src/Services/AttachmentService/API/AttachmentService.API.csproj package Microsoft.EntityFrameworkCore
dotnet add src/Services/AttachmentService/API/AttachmentService.API.csproj package Microsoft.EntityFrameworkCore.Sqlite
dotnet add src/Services/AttachmentService/API/AttachmentService.API.csproj package MediatR
dotnet add src/Services/AttachmentService/API/AttachmentService.API.csproj package MediatR.Extensions.Microsoft.DependencyInjection
dotnet add src/Services/AttachmentService/API/AttachmentService.API.csproj package FluentValidation.AspNetCore
dotnet add src/Services/AttachmentService/API/AttachmentService.API.csproj package Serilog.AspNetCore
dotnet add src/Services/AttachmentService/API/AttachmentService.API.csproj package Swashbuckle.AspNetCore

# === NotificationService ===
... (동일한 패턴)

# SharedKernel
dotnet add BuildingBlocks/SharedKernel/SharedKernel.csproj package Microsoft.Extensions.Logging.Abstractions

echo "✅ 모든 서비스에 NuGet 패키지가 설치되었습니다!"
