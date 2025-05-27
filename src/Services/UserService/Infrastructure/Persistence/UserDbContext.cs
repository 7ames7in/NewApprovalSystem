using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence;
public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<UserRoleMapping> UserRoleMappings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(u => u.EmployeeNumber);
        modelBuilder.Entity<UserRole>().HasKey(r => r.RoleId);
        modelBuilder.Entity<UserRoleMapping>().HasKey(m => m.MappingId);

        modelBuilder.Entity<UserRoleMapping>()
            .HasOne<UserRole>()
            .WithMany()
            .HasForeignKey(m => m.RoleId);

        // 필요한 경우 아래처럼 설정 가능
        modelBuilder.Entity<User>()
            .Property(u => u.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}
