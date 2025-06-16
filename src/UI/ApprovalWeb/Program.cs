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

builder.Services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    options.Events = new OpenIdConnectEvents
    {
        OnTokenValidated = async context =>
        {
            var claimsIdentity = context.Principal?.Identity as ClaimsIdentity;
            if (claimsIdentity == null) return;

            var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService<User>>();

            // email claim 찾기 (provider 마다 살짝 다름)
            var email = claimsIdentity.FindFirst(ClaimTypes.Email)?.Value
                        ?? claimsIdentity.FindFirst("preferred_username")?.Value;

            if (string.IsNullOrEmpty(email)) return;

            var userinfo  = await userService.UserLoginAndInformationAsync(email);
            if (userinfo != null && userinfo.Email.Length > 0)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userinfo?.EmployeeNumber??string.Empty),
                    new Claim(ClaimTypes.Name, userinfo?.Name ?? string.Empty),
                    new Claim("UserName", userinfo?.Name ?? string.Empty),
                    new Claim(ClaimTypes.Role, userinfo?.Role ?? string.Empty),
                    new Claim("Position", userinfo?.Position ?? string.Empty),
                    new Claim("Department", userinfo?.Department ?? string.Empty),
                };
                
                claimsIdentity.RemoveClaim(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier));
                claimsIdentity.AddClaims(claims);
            }
            else
            {
                // Handle case where user is not found or email is not valid
                context.Fail("User not found or invalid email.");
            }
        }
    };
});

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

#region ApprovalService & UserService API 클라이언트 등록

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
    }).AddTransientHttpErrorPolicy(policyBuilder =>
        policyBuilder.WaitAndRetryAsync(3, retryAttempt =>
            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))); // ;
    builder.Services.AddScoped<IApprovalService, ApprovalApiService>();
}

builder.Services.AddHttpClient("UserApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7129/");
}).AddTransientHttpErrorPolicy(policyBuilder =>
        policyBuilder.WaitAndRetryAsync(3, retryAttempt =>
            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

builder.Services.AddScoped<IUserService<User>, UserApiService<User>>();
builder.Services.AddScoped<IApprovalRequestService, ApprovalRequestApiService>();

#endregion

var app = builder.Build();

#region Middleware

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
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