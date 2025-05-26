#!/bin/bash
set -e

echo "📁 Create Domain folders for each microservice..."
mkdir -p src/Services/ApprovalService/Domain/Entities
mkdir -p src/Services/UserService/Domain/Entities
mkdir -p src/Services/AttachmentService/Domain/Entities
mkdir -p src/Services/NotificationService/Domain/Entities
mkdir -p src/Services/LoggingService/Domain/Entities

echo "📄 Generating model class files..."

# ApprovalService
cat <<EOF > src/Services/ApprovalService/Domain/Entities/ApprovalRequest.cs
public class ApprovalRequest
{
    public Guid ApprovalId { get; set; }
    public string RequestTitle { get; set; }
    public string? RequestContent { get; set; }
    public string ApplicantEmployeeNumber { get; set; }
    public string ApplicantName { get; set; }
    public string? ApplicantPosition { get; set; }
    public string? ApplicantDepartment { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime RequestedAt { get; set; }
    public DateTime? RespondedAt { get; set; }
    public string? ApproverComment { get; set; }
    public string? ApprovalType { get; set; }
    public string? MisKey { get; set; }
}
EOF

cat <<EOF > src/Services/ApprovalService/Domain/Entities/ApprovalStep.cs
public class ApprovalStep
{
    public Guid StepId { get; set; }
    public Guid ApprovalId { get; set; }
    public string ApproverEmployeeNumber { get; set; }
    public int Sequence { get; set; } = 1;
    public bool IsFinalApprover { get; set; }
    public string? ActionStatus { get; set; }
    public DateTime? ActionDate { get; set; }
    public string? Comment { get; set; }
}
EOF

cat <<EOF > src/Services/ApprovalService/Domain/Entities/ApprovalTemplate.cs
public class ApprovalTemplate
{
    public Guid TemplateId { get; set; }
    public string TemplateName { get; set; }
    public string? TemplateContent { get; set; }
    public string CreatedByEmployeeNumber { get; set; }
    public DateTime CreatedAt { get; set; }
}
EOF

# UserService
cat <<EOF > src/Services/UserService/Domain/Entities/User.cs
public class User
{
    public string EmployeeNumber { get; set; }
    public string Name { get; set; }
    public string? Department { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public DateTime CreatedAt { get; set; }
}
EOF

cat <<EOF > src/Services/UserService/Domain/Entities/UserRole.cs
public class UserRole
{
    public Guid RoleId { get; set; }
    public string RoleName { get; set; }
}
EOF

cat <<EOF > src/Services/UserService/Domain/Entities/UserRoleMapping.cs
public class UserRoleMapping
{
    public Guid MappingId { get; set; }
    public string EmployeeNumber { get; set; }
    public Guid RoleId { get; set; }
}
EOF

# AttachmentService
cat <<EOF > src/Services/AttachmentService/Domain/Entities/ApprovalAttachment.cs
public class ApprovalAttachment
{
    public Guid AttachmentId { get; set; }
    public Guid ApprovalId { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public DateTime UploadedAt { get; set; }
}
EOF

# NotificationService
cat <<EOF > src/Services/NotificationService/Domain/Entities/EmailNotification.cs
public class EmailNotification
{
    public Guid EmailNotificationId { get; set; }
    public string Recipient { get; set; }
    public string Subject { get; set; }
    public string? Body { get; set; }
    public DateTime SentAt { get; set; }
    public string Status { get; set; }
}
EOF

# LoggingService
cat <<EOF > src/Services/LoggingService/Domain/Entities/SystemLog.cs
public class SystemLog
{
    public Guid LogId { get; set; }
    public string LogLevel { get; set; }
    public string Message { get; set; }
    public string? Source { get; set; }
    public DateTime CreatedAt { get; set; }
}
EOF

echo "✅ Model class scaffolding complete."
