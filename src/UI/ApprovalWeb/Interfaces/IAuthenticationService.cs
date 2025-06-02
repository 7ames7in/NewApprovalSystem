using System;

namespace ApprovalWeb.Interfaces;

public interface IAuthenticationService
{
    Task<bool> SignInAsync(string employeeNumber, string password);
    Task SignOutAsync();
}

