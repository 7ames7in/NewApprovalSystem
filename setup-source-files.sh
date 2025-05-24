#!/bin/bash

echo "📁 파일 및 폴더 생성 시작..."

# === API Layer ===
mkdir -p src/Services/ApprovalService/API/Controllers

touch src/Services/ApprovalService/API/Controllers/ApprovalController.cs
touch src/Services/ApprovalService/API/Program.cs

# === Application Layer ===
mkdir -p src/Services/ApprovalService/Application/Commands
mkdir -p src/Services/ApprovalService/Application/Queries
mkdir -p src/Services/ApprovalService/Application/Validators
mkdir -p src/Services/ApprovalService/Application/Interfaces

touch src/Services/ApprovalService/Application/Commands/SubmitApprovalCommand.cs
touch src/Services/ApprovalService/Application/Commands/SubmitApprovalCommandHandler.cs
touch src/Services/ApprovalService/Application/Interfaces/IApprovalRepository.cs

# === Domain Layer ===
mkdir -p src/Services/ApprovalService/Domain/Entities
mkdir -p src/Services/ApprovalService/Domain/Enums
mkdir -p src/Services/ApprovalService/Domain/ValueObjects
mkdir -p src/Services/ApprovalService/Domain/Events

touch src/Services/ApprovalService/Domain/Entities/Approval.cs
touch src/Services/ApprovalService/Domain/Enums/ApprovalStatus.cs
touch src/Services/ApprovalService/Domain/ValueObjects/Approver.cs

# === Infrastructure Layer ===
mkdir -p src/Services/ApprovalService/Infrastructure/Repositories
mkdir -p src/Services/ApprovalService/Infrastructure/Persistence
mkdir -p src/Services/ApprovalService/Infrastructure/Messaging

touch src/Services/ApprovalService/Infrastructure/Repositories/ApprovalRepository.cs
touch src/Services/ApprovalService/Infrastructure/Persistence/ApprovalDbContext.cs

# === 테스트 프로젝트 ===
mkdir -p tests/ApprovalService.Tests
touch tests/ApprovalService.Tests/ApprovalTests.cs

echo "✅ 폴더 및 파일 구조가 성공적으로 생성되었습니다!"
