using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserService.Infrastructure.Persistence;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Seed
{
    public static class UserSeedData
    {
        public static async Task InitializeAsync(UserDbContext context)
        {
            await context.Database.EnsureCreatedAsync();

            if (await context.Users.AnyAsync())
                return;

            // 1. 역할 정의
            var adminRole = new UserRole { RoleId = Guid.NewGuid(), RoleName = "Admin" };
            var managerRole = new UserRole { RoleId = Guid.NewGuid(), RoleName = "Manager" };
            var employeeRole = new UserRole { RoleId = Guid.NewGuid(), RoleName = "Employee" };

            // 2. 사용자 정의
            var users = new[]
            {
                new User
                {
                    EmployeeNumber = "1001",
                    Name = "Kim Minsoo",
                    Department = "IT",
                    Email = "minsoo.kim@example.com",
                    Role = "Admin",
                    CreatedAt = DateTime.UtcNow.AddDays(-5)
                },
                new User
                {
                    EmployeeNumber = "1002",
                    Name = "Lee Jiyoung",
                    Department = "HR",
                    Email = "jiyoung.lee@example.com",
                    Role = "Manager",
                    CreatedAt = DateTime.UtcNow.AddDays(-4)
                },
                new User
                {
                    EmployeeNumber = "1003",
                    Name = "Park Jihye",
                    Department = "Finance",
                    Email = "jihye.park@example.com",
                    Role = "Employee",
                    CreatedAt = DateTime.UtcNow.AddDays(-3)
                }
            };

            // 3. 매핑 정의
            var mappings = new[]
            {
                new UserRoleMapping { MappingId = Guid.NewGuid(), EmployeeNumber = "1001", RoleId = adminRole.RoleId },
                new UserRoleMapping { MappingId = Guid.NewGuid(), EmployeeNumber = "1002", RoleId = managerRole.RoleId },
                new UserRoleMapping { MappingId = Guid.NewGuid(), EmployeeNumber = "1003", RoleId = employeeRole.RoleId }
            };

            await context.UserRoles.AddRangeAsync(adminRole, managerRole, employeeRole);
            await context.Users.AddRangeAsync(users);
            await context.UserRoleMappings.AddRangeAsync(mappings);

            await context.SaveChangesAsync();
        }
    }
}
