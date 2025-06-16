using Microsoft.EntityFrameworkCore;
using ApprovalService.Infrastructure.Persistence;
using ApprovalService.Infrastructure.Seed;
using BuildingBlocks.Core.Interfaces;
using ApprovalService.Domain.Entities;
using ApprovalService.Infrastructure.Repositories;
using ApprovalService.Domain.Interfaces;
using Serilog;
using System.Text.Json.Serialization;
using Serilog.Sinks.Http;
using Polly;

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

// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Enable Swagger in development mode
    app.UseSwaggerUI(); // Enable Swagger UI
}

app.MapControllers(); // Map controller routes

// Ensure database is created and seed initial data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApprovalDbContext>();
    db.Database.EnsureCreated(); // Ensure the database is created
    await ApprovalSeedData.InitializeAsync(db); // Seed initial data
}

// Run the application
app.Run();
