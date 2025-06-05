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

// Load user secrets for configuration
builder.Configuration.AddUserSecrets<Program>();

// Configure controllers and JSON options
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

#region Serilog Configuration
// Configure Serilog for logging
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("Logs/approval_web_log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();
#endregion

// Register Serilog as a singleton service
builder.Services.AddSingleton<Serilog.ILogger>(Log.Logger);

// Register HTTP context accessor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Register authentication and user context services
builder.Services.AddScoped<IAuthenticationService, LocalAuthenticationService>();
builder.Services.AddScoped<IUserContext, ClaimUserContext>();

/*
#region Authentication Configuration
// Configure cookie-based authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });
#endregion

#region Authorization Configuration
// Configure authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Administrator"));
    options.AddPolicy("RequireManagerRole", policy => policy.RequireRole("Manager"));
    options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
});
#endregion
*/

// Add controllers with views
builder.Services.AddControllersWithViews();

#region Azure AD Authentication (Commented Out)
// Example configuration for Azure AD authentication (currently disabled)
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

#endregion

#region Service Registration
// Register ApprovalService based on configuration
var useMock = builder.Configuration.GetValue<bool>("UseMockService");
if (!useMock)
{
    builder.Services.AddScoped<IApprovalService, MockApprovalService>();
}
else
{
    // Configure ApprovalService API client
    builder.Services.AddHttpClient("ApprovalApi", client =>
    {
        client.BaseAddress = new Uri("https://localhost:5001/");
    });
    builder.Services.AddScoped<IApprovalService, ApprovalApiService>();
}

// Configure UserService API client
builder.Services.AddHttpClient("UserApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7129/");
});
builder.Services.AddScoped<IUserService<User>, UserApiService<User>>();
builder.Services.AddScoped<IApprovalRequestService, ApprovalRequestApiService>();
#endregion

var app = builder.Build();

#region Middleware Configuration
// Configure middleware for error handling, HTTPS redirection, and static files
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Uncomment the following line to enable authentication middleware
// app.UseAuthentication();

app.UseAuthorization();

// Map default controller route
app.MapDefaultControllerRoute();

// Example custom route mapping (commented out)
// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=ApprovalRequest}/{action=Index}/{id?}");
#endregion

app.Run();
