using Microsoft.EntityFrameworkCore;
using BuildingBlocks.Core.Infrastructure.Data.Interfaces;
using UserService.Infrastructure.Seed;
using Serilog;
using UserService.Domain.Entities;
using UserService.Infrastructure.Repositories;
using UserService.Infrastructure.Persistence;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerGen(); // Add Swagger generation

// Serilog 설정
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("Logs/attachment_service_log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog(); // Serilog 사용
// Serilog 설정 완료
// Serilog 미들웨어 추가
    
// SQLite 연결 등록
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlite("Data Source=Data/UserService.db"));

builder.Services.AddScoped<IRepository<User>, UserRepository<User>>();
builder.Services.AddScoped<IRepository<UserRoleMapping>, UserRoleMappingRepository<UserRoleMapping>>();
builder.Services.AddScoped<IRepository<UserRole>, UserRoleRepository<UserRole>>();

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
    var db = scope.ServiceProvider.GetRequiredService<UserDbContext>();
    db.Database.EnsureCreated(); // 또는 db.Database.Migrate();
    await UserSeedData.InitializeAsync(db);
}

app.Run();
