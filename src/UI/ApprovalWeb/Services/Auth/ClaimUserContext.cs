using System;
using System.Security.Claims;
using ApprovalWeb.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ApprovalWeb.Services.Auth;

public class ClaimUserContext : IUserContext
{
    private readonly IHttpContextAccessor _accessor;

    public ClaimUserContext(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public string? UserId => _accessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    public string? UserName => _accessor.HttpContext?.User.FindFirst("UserName")?.Value;
    public string? Role => _accessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
    public string? Department => _accessor.HttpContext?.User.FindFirst("Department")?.Value;
    public string? Email => _accessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value; // Assuming UserName is the email, adjust as necessary
}
