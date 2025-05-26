using ApprovalService.Infrastructure.Persistence;
using ApprovalService.Domain.Entities;

namespace ApprovalService.Infrastructure.Seed;

public static class ApprovalSeedData
{
    public static async Task InitializeAsync(ApprovalDbContext context)
    {
        if (!context.ApprovalRequests.Any())
        {
            var approvalId = Guid.NewGuid();

            context.ApprovalRequests.Add(new ApprovalRequest
            {
                ApprovalId = approvalId,
                RequestTitle = "출장 신청",
                RequestContent = "동경 출장을 다녀오겠습니다.",
                ApplicantEmployeeNumber = "EMP001",
                ApplicantName = "홍길동",
                ApplicantPosition = "과장",
                ApplicantDepartment = "해외사업부",
                RequestedAt = DateTime.UtcNow,
                Status = "Pending"
            });

            context.ApprovalSteps.Add(new ApprovalStep
            {
                StepId = Guid.NewGuid(),
                ApprovalId = approvalId,
                ApproverEmployeeNumber = "EMP002",
                Sequence = 1,
                IsFinalApprover = true
            });

            context.ApprovalTemplates.Add(new ApprovalTemplate
            {
                TemplateId = Guid.NewGuid(),
                TemplateName = "출장 신청 양식",
                TemplateContent = "출장 목적과 상세 일정",
                CreatedByEmployeeNumber = "EMP001",
                CreatedAt = DateTime.UtcNow
            });

            await context.SaveChangesAsync();
        }
    }
}
