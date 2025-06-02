using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ApprovalService.Domain.Entities;
using ApprovalService.Infrastructure.Persistence;

namespace ApprovalService.Infrastructure.Seed
{
    public static class ApprovalSeedData
    {
        public static async Task InitializeAsync(ApprovalDbContext context)
        {
            await context.Database.EnsureCreatedAsync();

            if (await context.ApprovalRequests.AnyAsync())
                return;

            var approvalid1 = Guid.NewGuid();
            var approvalid2 = Guid.NewGuid();

            var request1 = new ApprovalRequest
            {
                ApprovalId = approvalid1,
                RequestTitle = "출장 신청",
                RequestContent = "3일간 부산 출장",
                ApplicantEmployeeNumber = "EC20505",
                ApplicantName = "Lee Jiyoung",
                ApplicantPosition = "Staff",
                ApplicantDepartment = "Sales",
                ApprovalType = "출장",
                RequestedAt = DateTime.UtcNow
            };

            var request2 = new ApprovalRequest
            {
                ApprovalId = approvalid2,
                RequestTitle = "회의 참석 요청",
                RequestContent = "다음 주 화요일 회의 참석",
                ApplicantEmployeeNumber = "EC20505",
                ApplicantName = "Park Hyunwoo",
                ApplicantPosition = "Senior Staff",
                ApplicantDepartment = "Marketing",
                ApprovalType = "회의",
                RequestedAt = DateTime.UtcNow.AddDays(-1)
            };

            var step1 = new ApprovalStep
            {
                StepId = Guid.NewGuid(),
                ApprovalId = approvalid1,
                ApproverEmployeeNumber = "EC20507",
                Sequence = 1,
                IsFinalApprover = true
            };

            var step2 = new ApprovalStep
            {
                StepId = Guid.NewGuid(),
                ApprovalId = approvalid2,
                ApproverEmployeeNumber = "EC20507",
                Sequence = 1,
                IsFinalApprover = false
            };

            var template = new ApprovalTemplate
            {
                TemplateId = Guid.NewGuid(),
                TemplateName = "기본 출장 템플릿",
                TemplateContent = "출장지, 기간, 목적 등 작성",
                CreatedByEmployeeNumber = "admin",
                CreatedAt = DateTime.UtcNow.AddDays(-7)
            };

            await context.ApprovalRequests.AddAsync(request1);
            await context.ApprovalRequests.AddAsync(request2);
            await context.ApprovalSteps.AddAsync(step1);
            await context.ApprovalSteps.AddAsync(step2);
            await context.ApprovalTemplates.AddAsync(template);
            await context.SaveChangesAsync();
        }
    }
}
