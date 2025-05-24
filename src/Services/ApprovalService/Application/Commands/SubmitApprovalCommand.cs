using MediatR;

namespace ApprovalService.Application.Commands;

public class SubmitApprovalCommand : IRequest<Guid>
{
    public string RequestId { get; set; } = string.Empty;
    public Guid ApproverId { get; set; }
    public string ApproverName { get; set; } = string.Empty;
}
