#!/bin/bash

echo "🛠️ 파일 템플릿 코드 생성 중..."

# ApprovalController.cs
cat <<EOF > src/Services/ApprovalService/API/Controllers/ApprovalController.cs
using ApprovalService.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApprovalService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApprovalController : ControllerBase
{
    private readonly IMediator _mediator;

    public ApprovalController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("submit")]
    public async Task<IActionResult> SubmitApproval([FromBody] SubmitApprovalCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(new { ApprovalId = id });
    }
}
EOF

# SubmitApprovalCommand.cs
cat <<EOF > src/Services/ApprovalService/Application/Commands/SubmitApprovalCommand.cs
using MediatR;

namespace ApprovalService.Application.Commands;

public class SubmitApprovalCommand : IRequest<Guid>
{
    public string RequestId { get; set; } = string.Empty;
    public Guid ApproverId { get; set; }
    public string ApproverName { get; set; } = string.Empty;
}
EOF

# SubmitApprovalCommandHandler.cs
cat <<EOF > src/Services/ApprovalService/Application/Commands/SubmitApprovalCommandHandler.cs
using ApprovalService.Application.Interfaces;
using ApprovalService.Domain.Entities;
using ApprovalService.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApprovalService.Application.Commands;

public class SubmitApprovalCommandHandler : IRequestHandler<SubmitApprovalCommand, Guid>
{
    private readonly IApprovalRepository _repository;
    private readonly ILogger<SubmitApprovalCommandHandler> _logger;

    public SubmitApprovalCommandHandler(IApprovalRepository repository, ILogger<SubmitApprovalCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Guid> Handle(SubmitApprovalCommand request, CancellationToken cancellationToken)
    {
        var approver = new Approver(request.ApproverId, request.ApproverName);
        var approval = new Approval(request.RequestId, approver);

        await _repository.AddAsync(approval);
        await _repository.SaveChangesAsync();

        _logger.LogInformation("Approval submitted: {ApprovalId}", approval.Id);

        return approval.Id;
    }
}
EOF

# IApprovalRepository.cs
cat <<EOF > src/Services/ApprovalService/Application/Interfaces/IApprovalRepository.cs
using ApprovalService.Domain.Entities;

namespace ApprovalService.Application.Interfaces;

public interface IApprovalRepository
{
    Task AddAsync(Approval approval);
    Task<Approval?> GetByIdAsync(Guid id);
    Task SaveChangesAsync();
}
EOF

# Approval.cs
cat <<EOF > src/Services/ApprovalService/Domain/Entities/Approval.cs
using ApprovalService.Domain.Enums;
using ApprovalService.Domain.ValueObjects;

namespace ApprovalService.Domain.Entities;

public class Approval
{
    public Guid Id { get; private set; }
    public string RequestId { get; private set; }
    public ApprovalStatus Status { get; private set; }
    public Approver Approver { get; private set; }
    public string? Comments { get; private set; }
    public DateTime RequestedAt { get; private set; }
    public DateTime? RespondedAt { get; private set; }

    private Approval() { }

    public Approval(string requestId, Approver approver)
    {
        Id = Guid.NewGuid();
        RequestId = requestId;
        Approver = approver;
        Status = ApprovalStatus.Pending;
        RequestedAt = DateTime.UtcNow;
    }

    public async Task ApproveAsync(string? comments = null)
    {
        await Task.Yield();
        if (Status != ApprovalStatus.Pending)
            throw new InvalidOperationException("Already processed.");

        Status = ApprovalStatus.Approved;
        Comments = comments;
        RespondedAt = DateTime.UtcNow;
    }

    public async Task RejectAsync(string? comments = null)
    {
        await Task.Yield();
        if (Status != ApprovalStatus.Pending)
            throw new InvalidOperationException("Already processed.");

        Status = ApprovalStatus.Rejected;
        Comments = comments;
        RespondedAt = DateTime.UtcNow;
    }
}
EOF

# ApprovalStatus.cs
cat <<EOF > src/Services/ApprovalService/Domain/Enums/ApprovalStatus.cs
namespace ApprovalService.Domain.Enums;

public enum ApprovalStatus
{
    Pending = 0,
    Approved = 1,
    Rejected = 2,
    Cancelled = 3
}
EOF

# Approver.cs
cat <<EOF > src/Services/ApprovalService/Domain/ValueObjects/Approver.cs
namespace ApprovalService.Domain.ValueObjects;

public class Approver
{
    public Guid UserId { get; private set; }
    public string Name { get; private set; }

    public Approver(Guid userId, string name)
    {
        UserId = userId;
        Name = name;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Approver other) return false;
        return UserId == other.UserId && Name == other.Name;
    }

    public override int GetHashCode() => HashCode.Combine(UserId, Name);
}
EOF

# ApprovalRepository.cs
cat <<EOF > src/Services/ApprovalService/Infrastructure/Repositories/ApprovalRepository.cs
using ApprovalService.Application.Interfaces;
using ApprovalService.Domain.Entities;
using ApprovalService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ApprovalService.Infrastructure.Repositories;

public class ApprovalRepository : IApprovalRepository
{
    private readonly ApprovalDbContext _context;

    public ApprovalRepository(ApprovalDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Approval approval)
    {
        await _context.Approvals.AddAsync(approval);
    }

    public async Task<Approval?> GetByIdAsync(Guid id)
    {
        return await _context.Approvals.FindAsync(id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
EOF

# ApprovalDbContext.cs
cat <<EOF > src/Services/ApprovalService/Infrastructure/Persistence/ApprovalDbContext.cs
using ApprovalService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApprovalService.Infrastructure.Persistence;

public class ApprovalDbContext : DbContext
{
    public ApprovalDbContext(DbContextOptions<ApprovalDbContext> options)
        : base(options) { }

    public DbSet<Approval> Approvals => Set<Approval>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Approval>().OwnsOne(a => a.Approver);
        base.OnModelCreating(modelBuilder);
    }
}
EOF

echo "✅ 모든 템플릿 코드가 성공적으로 생성되었습니다!"
