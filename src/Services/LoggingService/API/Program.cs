using Microsoft.EntityFrameworkCore;
using BuildingBlocks.Core.Infrastructure.Data.Interfaces;
using LoggingService.Infrastructure.Persistence;
using Serilog;
using LoggingService.Domain.Entities;
using LoggingService.Infrastructure.Repositories;
using LoggingService.Infrastructure.Seed;
using LoggingService.Infrastructure.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerGen(); // Add Swagger generation

// Serilog 설정
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("Logs/logging_service_log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog(); // Serilog 사용
// Serilog 설정 완료

// SQLite 연결 등록
builder.Services.AddDbContext<SystemLogDbContext>(options =>
    options.UseSqlite("Data Source=Data/LoggingService.db"));

builder.Services.AddScoped<IRepository<SystemLog>, SystemLogRepository<SystemLog>>();
builder.Services.AddScoped<ILoggingRepository, LoggingRepository>();

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
    var db = scope.ServiceProvider.GetRequiredService<SystemLogDbContext>();
    db.Database.EnsureCreated(); // 또는 db.Database.Migrate();
    await SystemLogSeedData.InitializeAsync(db);
}

app.Run();
