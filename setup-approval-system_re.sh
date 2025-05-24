#!/bin/bash

echo "🧹 기존 폴더 정리 중..."
rm -rf src BuildingBlocks tests ApprovalSystem.sln

echo "🛠️ 솔루션 생성"
dotnet new sln -n ApprovalSystem

echo "📁 디렉토리 생성"
mkdir -p src/Services/ApprovalService/API
mkdir -p src/Services/ApprovalService/Application/{Commands,Queries,Validators,Interfaces}
mkdir -p src/Services/ApprovalService/Domain/{Entities,Enums,ValueObjects,Events}
mkdir -p src/Services/ApprovalService/Infrastructure/{Repositories,Persistence,Messaging}

mkdir -p src/Services/NotificationService/API
mkdir -p src/Services/NotificationService/Application
mkdir -p src/Services/NotificationService/Domain
mkdir -p src/Services/NotificationService/Infrastructure

mkdir -p BuildingBlocks/{SharedKernel,Contracts/Events,EventBus}
mkdir -p tests

echo "📦 프로젝트 생성"
# ApprovalService
dotnet new webapi -n ApprovalService.API -o src/Services/ApprovalService/API
dotnet new classlib -n ApprovalService.Application -o src/Services/ApprovalService/Application
dotnet new classlib -n ApprovalService.Domain -o src/Services/ApprovalService/Domain
dotnet new classlib -n ApprovalService.Infrastructure -o src/Services/ApprovalService/Infrastructure

# NotificationService
dotnet new webapi -n NotificationService.API -o src/Services/NotificationService/API
dotnet new classlib -n NotificationService.Application -o src/Services/NotificationService/Application
dotnet new classlib -n NotificationService.Domain -o src/Services/NotificationService/Domain
dotnet new classlib -n NotificationService.Infrastructure -o src/Services/NotificationService/Infrastructure

# BuildingBlocks
dotnet new classlib -n SharedKernel -o BuildingBlocks/SharedKernel
dotnet new classlib -n Contracts -o BuildingBlocks/Contracts
dotnet new classlib -n EventBus -o BuildingBlocks/EventBus

# Tests
dotnet new xunit -n ApprovalService.Tests -o tests/ApprovalService.Tests

echo "➕ 프로젝트를 솔루션에 추가"
dotnet sln ApprovalSystem.sln add \
    src/Services/ApprovalService/API/ApprovalService.API.csproj \
    src/Services/ApprovalService/Application/ApprovalService.Application.csproj \
    src/Services/ApprovalService/Domain/ApprovalService.Domain.csproj \
    src/Services/ApprovalService/Infrastructure/ApprovalService.Infrastructure.csproj \
    src/Services/NotificationService/API/NotificationService.API.csproj \
    src/Services/NotificationService/Application/NotificationService.Application.csproj \
    src/Services/NotificationService/Domain/NotificationService.Domain.csproj \
    src/Services/NotificationService/Infrastructure/NotificationService.Infrastructure.csproj \
    BuildingBlocks/SharedKernel/SharedKernel.csproj \
    BuildingBlocks/Contracts/Contracts.csproj \
    BuildingBlocks/EventBus/EventBus.csproj \
    tests/ApprovalService.Tests/ApprovalService.Tests.csproj

echo "🔗 참조 연결"

# ApprovalService.API → Application, Domain, Infrastructure, Contracts, EventBus
dotnet add src/Services/ApprovalService/API/ApprovalService.API.csproj reference \
    src/Services/ApprovalService/Application/ApprovalService.Application.csproj \
    src/Services/ApprovalService/Domain/ApprovalService.Domain.csproj \
    src/Services/ApprovalService/Infrastructure/ApprovalService.Infrastructure.csproj \
    BuildingBlocks/Contracts/Contracts.csproj \
    BuildingBlocks/EventBus/EventBus.csproj

# 📦 Application → Domain + SharedKernel
dotnet add src/Services/ApprovalService/Application/ApprovalService.Application.csproj reference \
    src/Services/ApprovalService/Domain/ApprovalService.Domain.csproj \
    BuildingBlocks/SharedKernel/SharedKernel.csproj

# 📦 Infrastructure → Domain + SharedKernel
dotnet add src/Services/ApprovalService/Infrastructure/ApprovalService.Infrastructure.csproj reference \
    src/Services/ApprovalService/Application/ApprovalService.Application.csproj \
    src/Services/ApprovalService/Domain/ApprovalService.Domain.csproj \
    BuildingBlocks/SharedKernel/SharedKernel.csproj

# 📦 Tests → Application + Domain
dotnet add tests/ApprovalService.Tests/ApprovalService.Tests.csproj reference \
    src/Services/ApprovalService/Application/ApprovalService.Application.csproj \
    src/Services/ApprovalService/Domain/ApprovalService.Domain.csproj

echo "✅ 구성 완료! 폴더 및 프로젝트가 정상적으로 생성되었습니다."