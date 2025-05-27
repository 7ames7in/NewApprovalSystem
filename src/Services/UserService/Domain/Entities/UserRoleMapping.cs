namespace UserService.Domain.Entities;

public class UserRoleMapping
{
    public Guid MappingId { get; set; }
    public string EmployeeNumber { get; set; }
    public Guid RoleId { get; set; }
}