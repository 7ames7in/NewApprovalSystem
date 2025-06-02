namespace ApprovalWeb.Models;

public class LoginViewModel
{
    public string EmailId { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string? ReturnUrl { get; set; }
}
