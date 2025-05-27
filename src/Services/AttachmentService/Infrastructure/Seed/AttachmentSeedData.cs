using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AttachmentService.Domain.Entities;
using AttachmentService.Infrastructure.Persistence;

public static class AttachmentSeedData
{
    public static async Task InitializeAsync(AttachmentDbContext context)
    {
        // DB 생성 여부 확인 및 생성
        await context.Database.EnsureCreatedAsync();

        // 기존 데이터가 있으면 시드 생략
        if (await context.ApprovalAttachments.AnyAsync())
            return;

        // 샘플 데이터 추가
        var attachments = new[]
        {
            new ApprovalAttachment
            {
                AttachmentId = Guid.NewGuid(),
                ApprovalId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                FileName = "proposal.pdf",
                FilePath = "/files/proposal.pdf",
                UploadedAt = DateTime.UtcNow.AddDays(-3)
            },
            new ApprovalAttachment
            {
                AttachmentId = Guid.NewGuid(),
                ApprovalId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                FileName = "budget.xlsx",
                FilePath = "/files/budget.xlsx",
                UploadedAt = DateTime.UtcNow.AddDays(-2)
            },
            new ApprovalAttachment
            {
                AttachmentId = Guid.NewGuid(),
                ApprovalId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                FileName = "organization_chart.png",
                FilePath = "/files/org_chart.png",
                UploadedAt = DateTime.UtcNow.AddDays(-1)
            }
        };

        context.ApprovalAttachments.AddRange(attachments);
        await context.SaveChangesAsync();
    }
}
