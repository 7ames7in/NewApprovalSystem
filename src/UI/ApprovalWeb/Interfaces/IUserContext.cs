using System;

namespace ApprovalWeb.Interfaces;

public interface IUserContext
{
    string? UserId { get; }
    string? UserName { get; }
    string? Role { get; }
    string? Department { get; }
    string? Email { get; }
}
