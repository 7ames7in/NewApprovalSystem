using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models; // For OpenAPI/Swagger support
using Swashbuckle;
using System.Net.Mail;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerGen(); // Add Swagger generation

// Serilog 설정
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("Logs/email_service_log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog(); // Serilog 사용
// Serilog 설정 완료

// SQLite 연결 등록
// builder.Services.AddDbContext<SystemLogDbContext>(options =>
//     options.UseSqlite("Data Source=Data/LoggingService.db"));

//builder.Services.AddScoped<ILoggingRepository, LoggingRepository>();

builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

// 데이터베이스 생성 및 시드
// using (var scope = app.Services.CreateScope())
// {
//     var db = scope.ServiceProvider.GetRequiredService<SystemLogDbContext>();
//     db.Database.EnsureCreated(); // 또는 db.Database.Migrate();
//     await SystemLogSeedData.InitializeAsync(db);
// }

app.MapGet("/", () => "EmailService Runnnnning...");
app.MapPost("/api/emailservice/send-email", async ([FromBody] EmailRequest request) =>
{
    // 간단한 이메일 전송 예시 (실제 환경에서는 SmtpClient 등 사용)
    try
    {
        // 실제 이메일 전송 로직 구현 필요
        // 예: SmtpClient, SendGrid, MailKit 등 사용
        // 아래는 예시용 코드
        Console.WriteLine($"To: {request.To}, Subject: {request.Subject}, Body: {request.Body}");
        var smtp = new SmtpClient("mailgot.it.volvo.net");
        var mailMessage = new MailMessage("noreply-eep@volvo.com", request.To, request.Subject, request.Body);

        if (request.Attachments != null)
        {
            foreach (var formFile in request.Attachments)
            {
                if (formFile.Length > 0)
                {
                    using var ms = new MemoryStream();
                    await formFile.CopyToAsync(ms);
                    ms.Position = 0;
                    var attachment = new Attachment(ms, formFile.FileName, formFile.ContentType);
                    mailMessage.Attachments.Add(attachment);
                }
            }
        }

        await smtp.SendMailAsync(mailMessage);

        Log.Information("Email sent. To: {To}, Subject: {Subject}, Body: {Body}", request.To, request.Subject, request.Body);
        // 성공 응답 반환
        return Results.Ok(new { message = "Email sent successfully." });
    }
    catch (Exception ex)
    {
        // 실패 응답 반환
        return Results.Problem($"Failed to send email: {ex.Message}");
    }
});

app.Run();

// EmailRequest DTO 정의
public record EmailRequest(string To, string Subject, string Body, List<IFormFile>? Attachments);
