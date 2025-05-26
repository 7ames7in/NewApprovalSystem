#!/bin/bash
set -e

echo "📁 Create Repository folders..."

mkdir -p src/Services/ApprovalService/Domain/Interfaces
mkdir -p src/Services/ApprovalService/Infrastructure/Repositories
mkdir -p src/Services/UserService/Domain/Interfaces
mkdir -p src/Services/UserService/Infrastructure/Repositories
mkdir -p src/Services/AttachmentService/Domain/Interfaces
mkdir -p src/Services/AttachmentService/Infrastructure/Repositories
mkdir -p src/Services/NotificationService/Domain/Interfaces
mkdir -p src/Services/NotificationService/Infrastructure/Repositories
mkdir -p src/Services/LoggingService/Domain/Interfaces
mkdir -p src/Services/LoggingService/Infrastructure/Repositories

echo "🧩 Generating IRepository and Repository classes..."

generate_repo() {
  entity=$1
  domainPath=$2
  infraPath=$3

cat <<EOF > $domainPath/I${entity}Repository.cs
public interface I${entity}Repository
{
    Task<$entity?> GetByIdAsync(Guid id);
    Task<IEnumerable<$entity>> GetAllAsync();
    Task AddAsync($entity entity);
    Task UpdateAsync($entity entity);
    Task DeleteAsync(Guid id);
}
EOF

cat <<EOF > $infraPath/${entity}Repository.cs
public class ${entity}Repository : I${entity}Repository
{
    private readonly DbContext _context;

    public ${entity}Repository(DbContext context)
    {
        _context = context;
    }

    public async Task<$entity?> GetByIdAsync(Guid id) =>
        await _context.Set<$entity>().FindAsync(id);

    public async Task<IEnumerable<$entity>> GetAllAsync() =>
        await _context.Set<$entity>().ToListAsync();

    public async Task AddAsync($entity entity)
    {
        await _context.Set<$entity>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync($entity entity)
    {
        _context.Set<$entity>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Set<$entity>().FindAsync(id);
        if (entity != null)
        {
            _context.Set<$entity>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
EOF
}

# ApprovalService
generate_repo "ApprovalRequest" "src/Services/ApprovalService/Domain/Interfaces" "src/Services/ApprovalService/Infrastructure/Repositories"
generate_repo "ApprovalStep" "src/Services/ApprovalService/Domain/Interfaces" "src/Services/ApprovalService/Infrastructure/Repositories"
generate_repo "ApprovalTemplate" "src/Services/ApprovalService/Domain/Interfaces" "src/Services/ApprovalService/Infrastructure/Repositories"

# UserService
generate_repo "User" "src/Services/UserService/Domain/Interfaces" "src/Services/UserService/Infrastructure/Repositories"
generate_repo "UserRole" "src/Services/UserService/Domain/Interfaces" "src/Services/UserService/Infrastructure/Repositories"
generate_repo "UserRoleMapping" "src/Services/UserService/Domain/Interfaces" "src/Services/UserService/Infrastructure/Repositories"

# AttachmentService
generate_repo "ApprovalAttachment" "src/Services/AttachmentService/Domain/Interfaces" "src/Services/AttachmentService/Infrastructure/Repositories"

# NotificationService
generate_repo "EmailNotification" "src/Services/NotificationService/Domain/Interfaces" "src/Services/NotificationService/Infrastructure/Repositories"

# LoggingService
generate_repo "SystemLog" "src/Services/LoggingService/Domain/Interfaces" "src/Services/LoggingService/Infrastructure/Repositories"

echo "✅ Repository scaffolding complete."
