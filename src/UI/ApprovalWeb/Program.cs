using ApprovalWeb.Services;
using ApprovalWeb.Interfaces;
using UserService.Domain.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("Logs/approval_web_log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();
// Serilog 설정 완료
// Add services to the container.

Log.Information($"AzureAd: {builder.Configuration.GetSection("AzureAd")}");

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));


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

builder.Services.AddHttpClient("UserApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7129/");
});

builder.Services.AddScoped<IUserService<User>, UserApiService<User>>();
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


// builder.Services.AddAuthentication(options =>
// {
//     options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//     options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
// })
// .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
//     .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
//     {
//         options.Authority = builder.Configuration["AzureAd:Instance"] + builder.Configuration["AzureAd:TenantId"];
//         options.ClientId = builder.Configuration["AzureAd:ClientId"];
//         options.ResponseType = "code";
//         options.SaveTokens = true;
//         options.GetClaimsFromUserInfoEndpoint = true;
//         options.Scope.Add("openid");
//         options.Scope.Add("profile");
//         options.Scope.Add("email");
//     });