namespace UserService.Domain.Entities;

public class User
{
    public string EmployeeNumber { get; set; }
    public string Name { get; set; }
    public string? Department { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public DateTime CreatedAt { get; set; }
}
