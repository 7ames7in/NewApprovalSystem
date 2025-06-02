using Microsoft.EntityFrameworkCore;
using ApprovalService.Infrastructure.Persistence;
using ApprovalService.Infrastructure.Seed;
using BuildingBlocks.Core.Infrastructure.Data.Interfaces;
using ApprovalService.Domain.Entities;
using ApprovalService.Infrastructure.Repositories;
using ApprovalService.Domain.Interfaces;
using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerGen(); // Add Swagger generation

// Serilog 설정
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("Logs/approval_service_log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog(); // Serilog 사용
// Serilog 설정 완료
// Serilog 미들웨어 추가

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });


// SQLite 연결 등록
builder.Services.AddDbContext<ApprovalDbContext>(options =>
    options.UseSqlite("Data Source=Data/ApprovalService.db"));

builder.Services.AddScoped<IRepository<ApprovalRequest>, ApprovalRequestRepository<ApprovalRequest>>();
builder.Services.AddScoped<IRepository<Approval>, ApprovalRepository<Approval>>();
builder.Services.AddScoped<IRepository<ApprovalTemplate>, ApprovalTemplateRepository<ApprovalTemplate>>();
builder.Services.AddScoped<IRepository<ApprovalRequest>, ApprovalRequestRepository<ApprovalRequest>>();

builder.Services.AddScoped<IApprovalRequestRepository<ApprovalRequest>, ApprovalRequestRepository<ApprovalRequest>>();
// builder.Services.AddScoped<IApprovalTemplateRepository<ApprovalTemplate>, ApprovalTemplateRepository<ApprovalTemplate>>();
// builder.Services.AddScoped<IApprovalRequestRepository<ApprovalRequest>, ApprovalRequestRepository<ApprovalRequest>>();


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
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApprovalDbContext>();
    db.Database.EnsureCreated(); // 또는 db.Database.Migrate();
    await ApprovalSeedData.InitializeAsync(db);
}

app.Run();
