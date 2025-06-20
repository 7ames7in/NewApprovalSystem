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
using System.Security.Claims;
using Polly;
using Polly.Extensions.Http;
using Microsoft.AspNetCore.Mvc;
using ApprovalWeb.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Load user secrets for configuration
builder.Configuration.AddUserSecrets<Program>();

#region Serilog Configuration
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("Logs/approval_web_log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();
#endregion

builder.Services.AddSingleton<Serilog.ILogger>(Log.Logger);

// JSON Cycle Ignore 설정
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// HttpContextAccessor 등록
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Custom Services 등록
builder.Services.AddScoped<IAuthenticationService, LocalAuthenticationService>();
builder.Services.AddScoped<IApprovalRequestService, ApprovalRequestApiService>();
builder.Services.AddScoped<IAttachmentService, AttachmentApiService>();
builder.Services.AddScoped<IUserContext, ClaimUserContext>();

#region Hybrid Authentication 구성

// Hybrid 방식으로 쿠키 + OpenIdConnect (Azure AD) 통합 구성
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
.EnableTokenAcquisitionToCallDownstreamApi()
    .AddInMemoryTokenCaches();

// ConfigurationAuthentication will be called after app is built, so userService can be resolved from app.Services

builder.Services.Configure<CookieAuthenticationOptions>(
    CookieAuthenticationDefaults.AuthenticationScheme,
    options => {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

#endregion

builder.Services.AddApiVersioning(options => {
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
});

#region ApprovalService & UserService API 클라이언트 등록

var approvalServiceUri = builder.Configuration["ApiConfigs:ApprovalService:Uri"]??string.Empty;
var attachmentServiceUri = builder.Configuration["ApiConfigs:AttachmentService:Uri"]??string.Empty;
var loggingServiceUri = builder.Configuration["ApiConfigs:LoggingService:Uri"]??string.Empty;
var notificationServiceUri = builder.Configuration["ApiConfigs:NotificationService:Uri"]??string.Empty;
var userServiceUri = builder.Configuration["ApiConfigs:UserService:Uri"]??string.Empty;
var ApprovalWeb = builder.Configuration["ApiConfigs:ApprovalWeb:Uri"]??string.Empty;

var useMock = builder.Configuration.GetValue<bool>("UseMockService");
if (!useMock)
{
    builder.Services.AddScoped<IApprovalService, MockApprovalService>();
}
else
{
    builder.Services.AddHttpClient("ApprovalApi", client =>
    {
        client.BaseAddress = new Uri(approvalServiceUri);
    }).AddTransientHttpErrorPolicy(policyBuilder =>
        policyBuilder.WaitAndRetryAsync(3, retryAttempt =>
            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));
    builder.Services.AddScoped<IApprovalService, ApprovalApiService>();
}

builder.Services.AddHttpClient("UserApi", client =>
{
    client.BaseAddress = new Uri(userServiceUri);
}).AddTransientHttpErrorPolicy(policyBuilder =>
        policyBuilder.WaitAndRetryAsync(3, retryAttempt =>
            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

builder.Services.AddScoped<IUserService<User>, UserApiService<User>>();
builder.Services.AddScoped<IApprovalRequestService, ApprovalRequestApiService>();
builder.Services.AddScoped<IApprovalRequestService, ApprovalRequestApiService>();

#endregion

builder.Services.ConfigureAuthentication();
var app = builder.Build();

#region Middleware

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
    app.UseExceptionHandler("/Home/Error-Development");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 이제 반드시 Authentication 미들웨어를 활성화해야 함
app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();

#endregion

app.Run();