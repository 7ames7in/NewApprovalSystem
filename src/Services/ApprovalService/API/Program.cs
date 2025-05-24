var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers(); // Ensure controllers are added
builder.Services.AddEndpointsApiExplorer(); // Required for minimal APIs and Swagger
builder.Services.AddSwaggerGen(); // Add Swagger generation

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization(); // Ensure authorization middleware is added

app.MapControllers(); // Map controller routes

app.Run();
