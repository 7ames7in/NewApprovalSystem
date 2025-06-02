using ApprovalWeb.Services;
using ApprovalWeb.Interfaces;
using UserService.Domain.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using ApprovalWeb.Services.Auth;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Serilog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });


#region //Serilog 설정
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.Console()
        .WriteTo.File("Logs/approval_web_log.txt", rollingInterval: RollingInterval.Day)
        .CreateLogger();
    builder.Host.UseSerilog();
#endregion

builder.Services.AddSingleton<Serilog.ILogger>(Log.Logger);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
// 현재는 로컬 인증 방식
builder.Services.AddScoped<IAuthenticationService, LocalAuthenticationService>();
builder.Services.AddScoped<IUserContext, ClaimUserContext>();

// Authentication 설정
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });
// Cookie 인증 설정 완료
// Authorization 설정
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Administrator"));
    options.AddPolicy("RequireManagerRole", policy => policy.RequireRole("Manager"));
    options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
});

// // Authentication 설정
// builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//     .AddCookie(options =>
//     {
//         options.LoginPath = "/Account/Login";
//         options.AccessDeniedPath = "/Account/AccessDenied";
//     });
// // Cookie 인증 설정 완료
// // Authorization 설정
// builder.Services.AddAuthorization(options =>
// {
//     options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Administrator"));
//     options.AddPolicy("RequireManagerRole", policy => policy.RequireRole("Manager"));
//     options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
// });
// Authorization 정책 설정 완료
builder.Services.AddControllersWithViews();
// MVC 설정
// builder.Services.AddControllersWithViews(options =>
// {
//     var policy = new AuthorizationPolicyBuilder()
//         .RequireAuthenticatedUser()
//         .Build();
//     options.Filters.Add(new AuthorizeFilter(policy));
// });
// builder.Services.AddRazorPages();

#region Azure AD Authentication
// Azure AD Authentication 설정
/*
Log.Information($"AzureAd: {builder.Configuration.GetSection("AzureAd").GetValue<string>("Instance")}");
Log.Information($"AzureAd: {builder.Configuration.GetSection("AzureAd").GetValue<string>("ClientId")}");
Log.Information($"AzureAd: {builder.Configuration.GetSection("AzureAd").GetValue<string>("TenantId")}");

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));
    
    builder.Services.AddControllersWithViews(options =>
    {
        var policy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
        options.Filters.Add(new AuthorizeFilter(policy));
    });
    builder.Services.AddRazorPages();
*/
#endregion



var useMock = builder.Configuration.GetValue<bool>("UseMockService");
if (!useMock)
{
    builder.Services.AddScoped<IApprovalService, MockApprovalService>();
}
else
{
    // ApprovalService API Client
    builder.Services.AddHttpClient("ApprovalApi", client =>
    {
        client.BaseAddress = new Uri("https://localhost:5001/");
    });
    builder.Services.AddScoped<IApprovalService, ApprovalApiService>();
}

// UserService API Client
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

//app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=ApprovalRequest}/{action=Index}/{id?}");

app.Run();

