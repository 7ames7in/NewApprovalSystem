using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NotificationService.Infrastructure.Persistence;
using NotificationService.Domain.Entities;

namespace NotificationService.Infrastructure.Seed
{
    public static class NotificationSeedData
    {
        public static async Task InitializeAsync(NotificationDbContext context)
        {
            // DB 존재 확인 및 생성
            await context.Database.EnsureCreatedAsync();

            // 이미 데이터가 있으면 시딩 생략
            if (await context.EmailNotifications.AnyAsync())
                return;

            // 시드 데이터 구성
            var notifications = new[]
            {
                new EmailNotification
                {
                    NotificationId = Guid.NewGuid(),
                    Recipient = "user1@example.com",
                    Subject = "Welcome to the System",
                    Body = "Thank you for joining!",
                    SentAt = DateTime.UtcNow.AddDays(-2),
                    Status = "Sent"
                },
                new EmailNotification
                {
                    NotificationId = Guid.NewGuid(),
                    Recipient = "user2@example.com",
                    Subject = "Action Required",
                    Body = "Please update your profile.",
                    SentAt = DateTime.UtcNow.AddDays(-1),
                    Status = "Failed"
                },
                new EmailNotification
                {
                    NotificationId = Guid.NewGuid(),
                    Recipient = "user3@example.com",
                    Subject = "Your Report is Ready",
                    Body = "Click the link to download your report.",
                    SentAt = DateTime.UtcNow,
                    Status = "NotReady"
                }
            };

            await context.EmailNotifications.AddRangeAsync(notifications);
            await context.SaveChangesAsync();
        }
    }
}
