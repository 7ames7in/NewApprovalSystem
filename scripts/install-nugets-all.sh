#!/bin/bash
echo "🔧 전체 서비스 NuGet 패키지 설치 중..."

# === ApprovalService ===
dotnet add src/Services/ApprovalService/Application/ApprovalService.Application.csproj package MediatR
dotnet add src/Services/ApprovalService/Application/ApprovalService.Application.csproj package FluentValidation
dotnet add src/Services/ApprovalService/Application/ApprovalService.Application.csproj package AutoMapper.Extensions.Microsoft.DependencyInjection
dotnet add src/Services/ApprovalService/Application/ApprovalService.Application.csproj package Microsoft.Extensions.Logging
dotnet add src/Services/ApprovalService/Infrastructure/ApprovalService.Infrastructure.csproj package Microsoft.EntityFrameworkCore
dotnet add src/Services/ApprovalService/Infrastructure/ApprovalService.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Design
dotnet add src/Services/ApprovalService/Infrastructure/ApprovalService.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Sqlite
dotnet add src/Services/ApprovalService/API/ApprovalService.API.csproj package Microsoft.EntityFrameworkCore
dotnet add src/Services/ApprovalService/API/ApprovalService.API.csproj package Microsoft.EntityFrameworkCore.Sqlite
dotnet add src/Services/ApprovalService/API/ApprovalService.API.csproj package MediatR
dotnet add src/Services/ApprovalService/API/ApprovalService.API.csproj package MediatR.Extensions.Microsoft.DependencyInjection
dotnet add src/Services/ApprovalService/API/ApprovalService.API.csproj package FluentValidation.AspNetCore
dotnet add src/Services/ApprovalService/API/ApprovalService.API.csproj package Serilog.AspNetCore
dotnet add src/Services/ApprovalService/API/ApprovalService.API.csproj package Swashbuckle.AspNetCore

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
dotnet add src/Services/NotificationService/Application/NotificationService.Application.csproj package MediatR
dotnet add src/Services/NotificationService/Application/NotificationService.Application.csproj package FluentValidation
dotnet add src/Services/NotificationService/Application/NotificationService.Application.csproj package AutoMapper.Extensions.Microsoft.DependencyInjection
dotnet add src/Services/NotificationService/Application/NotificationService.Application.csproj package Microsoft.Extensions.Logging
dotnet add src/Services/NotificationService/Infrastructure/NotificationService.Infrastructure.csproj package Microsoft.EntityFrameworkCore
dotnet add src/Services/NotificationService/Infrastructure/NotificationService.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Design
dotnet add src/Services/NotificationService/Infrastructure/NotificationService.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Sqlite
dotnet add src/Services/NotificationService/API/NotificationService.API.csproj package Microsoft.EntityFrameworkCore
dotnet add src/Services/NotificationService/API/NotificationService.API.csproj package Microsoft.EntityFrameworkCore.Sqlite
dotnet add src/Services/NotificationService/API/NotificationService.API.csproj package MediatR
dotnet add src/Services/NotificationService/API/NotificationService.API.csproj package MediatR.Extensions.Microsoft.DependencyInjection
dotnet add src/Services/NotificationService/API/NotificationService.API.csproj package FluentValidation.AspNetCore
dotnet add src/Services/NotificationService/API/NotificationService.API.csproj package Serilog.AspNetCore
dotnet add src/Services/NotificationService/API/NotificationService.API.csproj package Swashbuckle.AspNetCore
# === LoggingService ===
dotnet add src/Services/LoggingService/Application/LoggingService.Application.csproj package MediatR
dotnet add src/Services/LoggingService/Application/LoggingService.Application.csproj package FluentValidation
dotnet add src/Services/LoggingService/Application/LoggingService.Application.csproj package AutoMapper.Extensions.Microsoft.DependencyInjection
dotnet add src/Services/LoggingService/Application/LoggingService.Application.csproj package Microsoft.Extensions.Logging
dotnet add src/Services/LoggingService/Infrastructure/LoggingService.Infrastructure.csproj package Microsoft.EntityFrameworkCore
dotnet add src/Services/LoggingService/Infrastructure/LoggingService.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Design
dotnet add src/Services/LoggingService/Infrastructure/LoggingService.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Sqlite
dotnet add src/Services/LoggingService/API/LoggingService.API.csproj package Microsoft.EntityFrameworkCore
dotnet add src/Services/LoggingService/API/LoggingService.API.csproj package Microsoft.EntityFrameworkCore.Sqlite
dotnet add src/Services/LoggingService/API/LoggingService.API.csproj package MediatR
dotnet add src/Services/LoggingService/API/LoggingService.API.csproj package MediatR.Extensions.Microsoft.DependencyInjection
dotnet add src/Services/LoggingService/API/LoggingService.API.csproj package FluentValidation.AspNetCore
dotnet add src/Services/LoggingService/API/LoggingService.API.csproj package Serilog.AspNetCore
dotnet add src/Services/LoggingService/API/LoggingService.API.csproj package Swashbuckle.AspNetCore
# === UserService ===
dotnet add src/Services/UserService/Application/UserService.Application.csproj package MediatR
dotnet add src/Services/UserService/Application/UserService.Application.csproj package FluentValidation
dotnet add src/Services/UserService/Application/UserService.Application.csproj package AutoMapper.Extensions.Microsoft.DependencyInjection
dotnet add src/Services/UserService/Application/UserService.Application.csproj package Microsoft.Extensions.Logging
dotnet add src/Services/UserService/Infrastructure/UserService.Infrastructure.csproj package Microsoft.EntityFrameworkCore
dotnet add src/Services/UserService/Infrastructure/UserService.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Design
dotnet add src/Services/UserService/Infrastructure/UserService.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Sqlite
dotnet add src/Services/UserService/API/UserService.API.csproj package Microsoft.EntityFrameworkCore
dotnet add src/Services/UserService/API/UserService.API.csproj package Microsoft.EntityFrameworkCore.Sqlite 
dotnet add src/Services/UserService/API/UserService.API.csproj package MediatR
dotnet add src/Services/UserService/API/UserService.API.csproj package MediatR.Extensions.Microsoft.DependencyInjection
dotnet add src/Services/UserService/API/UserService.API.csproj package FluentValidation.AspNetCore
dotnet add src/Services/UserService/API/UserService.API.csproj package Serilog.AspNetCore
dotnet add src/Services/UserService/API/UserService.API.csproj package Swashbuckle.AspNetCore
# === SharedKernel ===
dotnet add BuildingBlocks/SharedKernel/SharedKernel.csproj package Microsoft.Extensions.Logging.Abstractions

echo "✅ 전체 서비스 NuGet 패키지 설치가 완료되었습니다!"
