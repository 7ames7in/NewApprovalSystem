using MediatR;
using System;

namespace ApprovalService.Application.Commands;

public class SubmitApprovalCommand : IRequest<Guid>
{
    public string RequestId { get; set; } = string.Empty;
    public string ApproverId { get; set; } = string.Empty;
    public string ApproverName { get; set; } = string.Empty;
}
