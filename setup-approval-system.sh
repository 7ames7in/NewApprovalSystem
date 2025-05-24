#!/bin/bash

# 솔루션 생성
dotnet new sln -n ApprovalSystem

# src 구조
mkdir -p src/Services/ApprovalService
cd src/Services/ApprovalService

# API 프로젝트
dotnet new webapi -n ApprovalService.API
mkdir -p ApprovalService.API/Controllers

# Application Layer
dotnet new classlib -n ApprovalService.Application
mkdir -p ApprovalService.Application/Commands
mkdir -p ApprovalService.Application/Queries
mkdir -p ApprovalService.Application/Validators
mkdir -p ApprovalService.Application/Interfaces

# Domain Layer
dotnet new classlib -n ApprovalService.Domain
mkdir -p ApprovalService.Domain/Entities
mkdir -p ApprovalService.Domain/ValueObjects
mkdir -p ApprovalService.Domain/Enums
mkdir -p ApprovalService.Domain/Events

# Infrastructure Layer
dotnet new classlib -n ApprovalService.Infrastructure
mkdir -p ApprovalService.Infrastructure/Repositories
mkdir -p ApprovalService.Infrastructure/Persistence
mkdir -p ApprovalService.Infrastructure/Messaging

# 루트로 이동
cd ../../..
mkdir -p BuildingBlocks

# SharedKernel
cd BuildingBlocks
dotnet new classlib -n SharedKernel

# Contracts
dotnet new classlib -n Contracts
mkdir -p Contracts/Events

# EventBus
dotnet new classlib -n EventBus
cd ..

# 테스트 프로젝트
mkdir -p tests
cd tests
dotnet new xunit -n ApprovalService.Tests
cd ..

# 솔루션에 프로젝트 추가
dotnet sln ApprovalSystem.sln add \
    src/Services/ApprovalService/ApprovalService.API/ApprovalService.API.csproj \
    src/Services/ApprovalService/ApprovalService.Application/ApprovalService.Application.csproj \
    src/Services/ApprovalService/ApprovalService.Domain/ApprovalService.Domain.csproj \
    src/Services/ApprovalService/ApprovalService.Infrastructure/ApprovalService.Infrastructure.csproj \
    BuildingBlocks/SharedKernel/SharedKernel.csproj \
    BuildingBlocks/Contracts/Contracts.csproj \
    BuildingBlocks/EventBus/EventBus.csproj \
    tests/ApprovalService.Tests/ApprovalService.Tests.csproj

# 프로젝트 참조 연결
dotnet add src/Services/ApprovalService/ApprovalService.API/ApprovalService.API.csproj reference \
    ../ApprovalService.Application/ApprovalService.Application.csproj \
    ../ApprovalService.Domain/ApprovalService.Domain.csproj \
    ../ApprovalService.Infrastructure/ApprovalService.Infrastructure.csproj \
    ../../../BuildingBlocks/Contracts/Contracts.csproj \
    ../../../BuildingBlocks/EventBus/EventBus.csproj

dotnet add src/Services/ApprovalService/ApprovalService.Application/ApprovalService.Application.csproj reference \
    ../ApprovalService.Domain/ApprovalService.Domain.csproj \
    ../../../BuildingBlocks/SharedKernel/SharedKernel.csproj

dotnet add src/Services/ApprovalService/ApprovalService.Infrastructure/ApprovalService.Infrastructure.csproj reference \
    ../ApprovalService.Domain/ApprovalService.Domain.csproj \
    ../../../BuildingBlocks/SharedKernel/SharedKernel.csproj

dotnet add tests/ApprovalService.Tests/ApprovalService.Tests.csproj reference \
    ../src/Services/ApprovalService/ApprovalService.Application/ApprovalService.Application.csproj \
    ../src/Services/ApprovalService/ApprovalService.Domain/ApprovalService.Domain.csproj

echo "✅ ApprovalSystem 프로젝트 구조가 성공적으로 생성되었습니다!"
