using ApprovalWeb.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient("ApprovalApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:5001/"); // ApprovalService.API 주소
});

builder.Services.AddScoped<ApprovalApiService>(); // ApprovalService.API 주소

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Approval}/{action=Index}/{id?}");

app.Run();