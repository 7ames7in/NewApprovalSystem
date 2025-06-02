using ApprovalService.Application.Interfaces;
using ApprovalService.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

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
        //var approval = new ApprovalRequest(request.RequestId, approver);

        //await _repository.AddAsync(approval);
        await _repository.SaveChangesAsync();

        //_logger.LogInformation("Approval submitted: {ApprovalId}", approval.Id);

        //return approval.Id;
        return Guid.NewGuid(); // Placeholder for actual approval ID
    }
}
