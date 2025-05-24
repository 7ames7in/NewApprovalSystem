#!/bin/bash

echo "🔧 ApprovalService 관련 NuGet 패키지 설치 중..."

# ApprovalService.Application.csproj
dotnet add src/Services/ApprovalService/Application/ApprovalService.Application.csproj package MediatR
dotnet add src/Services/ApprovalService/Application/ApprovalService.Application.csproj package FluentValidation
dotnet add src/Services/ApprovalService/Application/ApprovalService.Application.csproj package AutoMapper.Extensions.Microsoft.DependencyInjection
dotnet add src/Services/ApprovalService/Application/ApprovalService.Application.csproj package MediatR
dotnet add src/Services/ApprovalService/Application/ApprovalService.Application.csproj package Microsoft.Extensions.Logging

# ApprovalService.Infrastructure.csproj
dotnet add src/Services/ApprovalService/Infrastructure/ApprovalService.Infrastructure.csproj package Microsoft.EntityFrameworkCore
dotnet add src/Services/ApprovalService/Infrastructure/ApprovalService.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Design
dotnet add src/Services/ApprovalService/Infrastructure/ApprovalService.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Sqlite

# ApprovalService.API.csproj
dotnet add src/Services/ApprovalService/API/ApprovalService.API.csproj package Microsoft.EntityFrameworkCore
dotnet add src/Services/ApprovalService/API/ApprovalService.API.csproj package Microsoft.EntityFrameworkCore.Sqlite
dotnet add src/Services/ApprovalService/API/ApprovalService.API.csproj package MediatR
dotnet add src/Services/ApprovalService/API/ApprovalService.API.csproj package MediatR.Extensions.Microsoft.DependencyInjection
dotnet add src/Services/ApprovalService/API/ApprovalService.API.csproj package FluentValidation.AspNetCore
dotnet add src/Services/ApprovalService/API/ApprovalService.API.csproj package Serilog.AspNetCore
dotnet add src/Services/ApprovalService/API/ApprovalService.API.csproj package Swashbuckle.AspNetCore
dotnet add src/Services/ApprovalService/API/ApprovalService.API.csproj package SwashBuckle

# Shared library들이 의존할 수 있는 경우를 위해 SharedKernel에도 추가해도 무방
dotnet add BuildingBlocks/SharedKernel/SharedKernel.csproj package Microsoft.Extensions.Logging.Abstractions

echo "✅ 모든 필수 NuGet 패키지가 설치되었습니다!"

dotnet add src/Services/ApprovalService/API/ApprovalService.API.csproj package MediatR.Extensions.Microsoft.DependencyInjection
dotnet add src/Services/ApprovalService/API/ApprovalService.API.csproj package FluentValidation.AspNetCore
dotnet add src/Services/ApprovalService/API/ApprovalService.API.csproj package Microsoft.EntityFrameworkCore
dotnet add src/Services/ApprovalService/API/ApprovalService.API.csproj package Microsoft.EntityFrameworkCore.Sqlite
dotnet add src/Services/ApprovalService/API/ApprovalService.API.csproj package Serilog.AspNetCore
