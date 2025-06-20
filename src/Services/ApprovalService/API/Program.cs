using Microsoft.EntityFrameworkCore;
using ApprovalService.Infrastructure.Persistence;
using ApprovalService.Infrastructure.Seed;
using BuildingBlocks.Core.Interfaces;
using ApprovalService.Domain.Entities;
using ApprovalService.Infrastructure.Repositories;
using ApprovalService.Domain.Interfaces;
using Serilog;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using BuildingBlocks.EventBus;
using ApprovalService.Application.Interfaces;
using ApprovalService.Infrastructure.Proxies;


var builder = WebApplication.CreateBuilder(args);

// Add Swagger generation for API documentation
builder.Services.AddSwaggerGen();

// Serilog 내부 오류를 파일로 기록 (콘솔 대신)
Serilog.Debugging.SelfLog.Enable(msg => 
    File.AppendAllText("Logs/serilog-internal.log", $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [Serilog Internal] {msg}\n"));

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File(
        "Logs/approval_service_log.txt", 
        rollingInterval: RollingInterval.Day)
    .WriteTo.DurableHttpUsingFileSizeRolledBuffers(
        requestUri: "https://localhost:6001/api/logs",
        bufferBaseFileName: "Logs/http-buffer", // 장애 시 임시 저장 경로
        bufferFileSizeLimitBytes: 10 * 1024 * 1024, // 10MB 버퍼
        period: TimeSpan.FromSeconds(5), // 5초마다 전송 시도
        textFormatter: new Serilog.Formatting.Json.JsonFormatter(),
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information
    )
    .CreateLogger();

builder.Host.UseSerilog();

Log.Information("Starting Approval Service API");

builder.Services.AddApiVersioning(options => {
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
});


// Configure controllers and JSON serialization options
builder.Services.AddControllers()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // Handle circular references
});

// Register SQLite database context
builder.Services.AddDbContext<ApprovalDbContext>(options =>
    options.UseSqlite("Data Source=Data/ApprovalService.db"));

// Register repositories for dependency injection
//builder.Services.AddScoped<IRepository<ApprovalRequestWithCurrentStepDto>, ApprovalRequestRepository<ApprovalRequestWithCurrentStepDto>>();

builder.Services.AddScoped<IRepository<ApprovalTemplate>, ApprovalTemplateRepository<ApprovalTemplate>>();
builder.Services.AddScoped<IApprovalRequestRepository<ApprovalRequestWithCurrentStepDto>, ApprovalRequestRepository<ApprovalRequestWithCurrentStepDto>>();
builder.Services.AddScoped<IApprovalRepository<ApprovalRequestWithCurrentStepDto>, ApprovalRepository<ApprovalRequestWithCurrentStepDto>>();
builder.Services.AddScoped<IAttachmentRepository<ApprovalAttachment>, AttachmentRepository<ApprovalAttachment>>(); // Generic repository for attachments
//builder.Services.AddSingleton<IEventBus, InMemoryEventBus>();
//builder.Services.AddSingleton<IEventBus, RabbitMQEventBus>();

// builder.Services.AddSingleton<IEventBus>(provider =>
//     new RabbitMQEventBus("localhost", "myuser", "mypassword"));

builder.Services.AddSingleton<IEventBus>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<RabbitMQEventBus>>();
    var configuration = provider.GetRequiredService<IConfiguration>();
    var hostName = configuration["RabbitMQ:HostName"] ?? "localhost";
    var userName = configuration["RabbitMQ:UserName"] ?? "guest";
    var password = configuration["RabbitMQ:Password"] ?? "guest";
    return new RabbitMQEventBus(provider, logger, hostName, userName, password);
});

builder.Services.AddScoped<IUserServiceProxy, UserServiceProxy>();

var approvalServiceUri = builder.Configuration["ApiConfigs:ApprovalService:Uri"]??string.Empty;
var attachmentServiceUri = builder.Configuration["ApiConfigs:AttachmentService:Uri"]??string.Empty;
var loggingServiceUri = builder.Configuration["ApiConfigs:LoggingService:Uri"]??string.Empty;
var notificationServiceUri = builder.Configuration["ApiConfigs:NotificationService:Uri"]??string.Empty;
var userServiceUri = builder.Configuration["ApiConfigs:UserService:Uri"]??string.Empty;
var ApprovalWeb = builder.Configuration["ApiConfigs:ApprovalWeb:Uri"] ?? string.Empty;

builder.Services.AddHttpClient<IUserServiceProxy, UserServiceProxy>(client =>
{
    client.BaseAddress = new Uri(userServiceUri); // UserService API 주소
});


// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error-development"); // Use a custom error handler in development
    app.UseSwagger(); // Enable Swagger in development mode
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Approval API V1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "Approval API V2");
        c.RoutePrefix = string.Empty; // Swagger UI를 루트 경로에 배치
    });
}
else
{
    app.UseExceptionHandler("/error"); // Use a custom error handler in production
    app.UseHsts(); // Use HTTP Strict Transport Security
}

app.MapControllers(); // Map controller routes

// Ensure database is created and seed initial data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApprovalDbContext>();
    db.Database.EnsureCreated(); // Ensure the database is created
    await ApprovalSeedData.InitializeAsync(db); // Seed initial data

    var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
}

// Run the application
app.Run();
