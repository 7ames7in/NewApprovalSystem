using System;

namespace ApprovalWeb.Services.Auth;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ApprovalWeb.Interfaces;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;

public class LocalAuthenticationService : ApprovalWeb.Interfaces.IAuthenticationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserService<User> _userService;
    private readonly ILogger<LocalAuthenticationService> _logger;

    public LocalAuthenticationService(IHttpContextAccessor accessor, IUserService<User> userService, ILogger<LocalAuthenticationService> logger)
    {
        _httpContextAccessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<bool> SignInAsync(string emailid, string password)
    {
        _logger.LogInformation("Attempting to sign in user {emailid}.", emailid);
        var userinfo  = await _userService.UserLoginAndInformationAsync(emailid);
        
        if (userinfo != null && userinfo.Email.Length > 0)
        {
            _logger.LogInformation("User {emailid} signed in successfully.", emailid);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userinfo?.EmployeeNumber??string.Empty),
                new Claim(ClaimTypes.Name, userinfo?.Name ?? string.Empty),
                new Claim(ClaimTypes.Role, userinfo?.Role ?? string.Empty),
                new Claim("Position", userinfo?.Position ?? string.Empty),
                new Claim(ClaimTypes.Email, emailid),
                new Claim("Department", userinfo?.Department ?? string.Empty),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            if (_httpContextAccessor.HttpContext != null)
            {
                await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            }
            else
            {
                throw new InvalidOperationException("HttpContext is null.");
            }
            return true;
        }
        else
        {
            _logger.LogWarning("Sign in failed for user {emailid}. User not found or invalid credentials.", emailid);
        }

        return false;
    }

    public async Task SignOutAsync()
    {
        if (_httpContextAccessor.HttpContext != null)
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
        else
        {
            throw new InvalidOperationException("HttpContext is null.");
        }
    }
}
