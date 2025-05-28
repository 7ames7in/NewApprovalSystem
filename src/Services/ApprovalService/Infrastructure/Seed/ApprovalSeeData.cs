// using ApprovalService.Infrastructure.Persistence;
// using ApprovalService.Domain.Entities;

// namespace ApprovalService.Infrastructure.Seed;

// public static class ApprovalSeedData
// {
//     public static async Task InitializeAsync(ApprovalDbContext context)
//     {
//         if (!context.ApprovalRequests.Any())
//         {
//             var approvalId = Guid.NewGuid();

//             context.ApprovalRequests.Add(new ApprovalRequest
//             {
//                 ApprovalId = approvalId,
//                 RequestTitle = "출장 신청",
//                 RequestContent = "동경 출장을 다녀오겠습니다.",
//                 ApplicantEmployeeNumber = "EMP001",
//                 ApplicantName = "홍길동",
//                 ApplicantPosition = "과장",
//                 ApplicantDepartment = "해외사업부",
//                 RequestedAt = DateTime.UtcNow,
//                 Status = "Pending"
//             });

//             context.ApprovalSteps.Add(new ApprovalStep
//             {
//                 StepId = Guid.NewGuid(),
//                 ApprovalId = approvalId,
//                 ApproverEmployeeNumber = "EMP002",
//                 Sequence = 1,
//                 IsFinalApprover = true
//             });

//             context.ApprovalTemplates.Add(new ApprovalTemplate
//             {
//                 TemplateId = Guid.NewGuid(),
//                 TemplateName = "출장 신청 양식",
//                 TemplateContent = "출장 목적과 상세 일정",
//                 CreatedByEmployeeNumber = "EMP001",
//                 CreatedAt = DateTime.UtcNow
//             });

//             await context.SaveChangesAsync();
//         }
//     }
// }


using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ApprovalService.Domain.Entities;
using ApprovalService.Domain.Enums;
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

            var requestId1 = "EC20505";
            var approvalid = Guid.NewGuid();
            var approvalid2 = Guid.NewGuid();
            var approver1 = new Approver("1001", "Kim Minsoo", "Manager", "IT");
            var approval1 = new Approval(requestId1.ToString(), approver1);

            var request = new ApprovalRequest
            {
                ApprovalId = approvalid,
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
            
            var step = new ApprovalStep
            {
                StepId = Guid.NewGuid(),
                ApprovalId = approvalid,
                ApproverEmployeeNumber = "EC20507",
                Sequence = 1,
                IsFinalApprover = true
            };

            var step2 = new ApprovalStep
            {
                StepId = Guid.NewGuid(),
                ApprovalId = approvalid,
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

            await context.Approvals.AddAsync(approval1);
            await context.ApprovalRequests.AddAsync(request);
            await context.ApprovalSteps.AddAsync(step);
            await context.ApprovalRequests.AddAsync(request2);
            await context.ApprovalSteps.AddAsync(step2);
            await context.ApprovalTemplates.AddAsync(template);
            await context.SaveChangesAsync();
        }
    }
}
