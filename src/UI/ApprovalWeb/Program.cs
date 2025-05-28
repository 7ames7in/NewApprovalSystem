using ApprovalWeb.Services;
using ApprovalWeb.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
var useMock = builder.Configuration.GetValue<bool>("UseMockService");

if (!useMock)
{
    builder.Services.AddScoped<IApprovalService, MockApprovalService>();
}
else
{
    builder.Services.AddHttpClient("ApprovalApi", client =>
    {
        client.BaseAddress = new Uri("https://localhost:5001/");
    });
    builder.Services.AddScoped<IApprovalService, ApprovalApiService>();
}

builder.Services.AddScoped<IApprovalRequestService, ApprovalRequestApiService>();

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
    pattern: "{controller=ApprovalRequest}/{action=Index}/{id?}");

app.Run();