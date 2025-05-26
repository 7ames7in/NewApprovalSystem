using Microsoft.EntityFrameworkCore;
using ApprovalService.Infrastructure.Persistence;
using ApprovalService.Infrastructure.Seed;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerGen(); // Add Swagger generation

// SQLite 연결 등록
builder.Services.AddDbContext<ApprovalDbContext>(options =>
    options.UseSqlite("Data Source=Data/ApprovalService.db"));

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
