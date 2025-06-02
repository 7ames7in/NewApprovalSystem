using System;
using System.ComponentModel.DataAnnotations;

namespace UserService.API.Dtos;

public class LoginDto
{
    [Required]
    [EmailAddress]
    public string EmailId { get; set; } = default!;

    [Required]
    public string Password { get; set; } = default!;
}
