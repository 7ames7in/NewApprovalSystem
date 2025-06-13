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

// Add Swagger generation for API documentation
builder.Services.AddSwaggerGen();

// Configure Serilog for logging
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("Logs/approval_service_log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog(); // Use Serilog as the logging provider

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
builder.Services.AddScoped<IApprovalRepository<ApprovalRequest>, ApprovalRepository<ApprovalRequest>>();

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
