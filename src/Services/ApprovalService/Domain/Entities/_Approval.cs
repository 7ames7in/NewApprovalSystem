// using ApprovalService.Domain.Enums;

// namespace ApprovalService.Domain.Entities;

// public class Approval
// {
//     public Guid Id { get; private set; }
//     public string RequestId { get; private set; }
//     public ApprovalStatus Status { get; private set; } = ApprovalStatus.Pending;
//     public Approver Approver { get; private set; }
//     public string? Comments { get; private set; }
//     public DateTime RequestedAt { get; private set; } = DateTime.UtcNow;
//     public DateTime? RespondedAt { get; private set; }

//     private Approval() { }

//     public Approval(string requestId, Approver approver)
//     {
//         Id = Guid.NewGuid();
//         RequestId = requestId;
//         Approver = approver;
//         Status = ApprovalStatus.Pending;
//         RequestedAt = DateTime.UtcNow;
//     }

//     public async Task ApproveAsync(string? comments = null)
//     {
//         await Task.Yield();
//         if (Status != ApprovalStatus.Pending)
//             throw new InvalidOperationException("Already processed.");

//         Status = ApprovalStatus.Approved;
//         Comments = comments;
//         RespondedAt = DateTime.UtcNow;
//     }

//     public async Task RejectAsync(string? comments = null)
//     {
//         await Task.Yield();
//         if (Status != ApprovalStatus.Pending)
//             throw new InvalidOperationException("Already processed.");

//         Status = ApprovalStatus.Rejected;
//         Comments = comments;
//         RespondedAt = DateTime.UtcNow;
//     }
// }
