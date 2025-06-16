using Microsoft.AspNetCore.Mvc;
using ApprovalService.API.Models;
using ApprovalService.Infrastructure.Repositories;
using ApprovalService.Domain.Entities;
using BuildingBlocks.Core.Interfaces;
using System.Threading.Tasks;
using ApprovalService.Domain.Interfaces;
using ApprovalService.API.DTOs;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ApprovalService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApprovalController : ControllerBase
{
    private readonly IApprovalRepository<ApprovalRequestWithCurrentStepDto> _approvalRepository;

    public ApprovalController(IApprovalRepository<ApprovalRequestWithCurrentStepDto> approvalRepository)
    {
        _approvalRepository = approvalRepository;
    }

    #region Sample Data
    // Sample data for demonstration purposes
    private static readonly List<ApprovalDto> _approvals = new()
    {
        new ApprovalDto { ApprovalId = Guid.NewGuid(), RequestId = "REQ001", ApproverName = "Alice", Status = "Pending", RequestedAt = DateTime.UtcNow },
        new ApprovalDto { ApprovalId = Guid.NewGuid(), RequestId = "REQ002", ApproverName = "Bob", Status = "Approved", RequestedAt = DateTime.UtcNow }
    };

    private static readonly List<ApprovalRequestDto> _approvalRequests = new()
    {
        new ApprovalRequestDto { ApprovalId = Guid.Parse("c6421493-17a0-4985-ab9f-a240d99a031a"), RequestTitle = "출장 신청", ApplicantEmployeeNumber = "EMP001", ApplicantName = "홍길동", Status = "Pending", RequestedAt = DateTime.UtcNow },
        new ApprovalRequestDto { ApprovalId = Guid.NewGuid(), RequestTitle = "휴가 신청", ApplicantEmployeeNumber = "EMP002", ApplicantName = "김철수", Status = "Approved", RequestedAt = DateTime.UtcNow }
    };

    private static readonly ApprovalRequestDto _approvalRequest =
        new ApprovalRequestDto { ApprovalId = Guid.Parse("c6421493-17a0-4985-ab9f-a240d99a031a"), RequestTitle = "출장 신청", ApplicantEmployeeNumber = "EMP001", ApplicantName = "홍길동", Status = "Pending", RequestedAt = DateTime.UtcNow };
    #endregion

    #region API Endpoints

    /// <summary>
    /// Retrieves all approvals.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetMyApprovalAll()
    {
        var list = await _approvalRepository.GetAllAsync();
        return Ok(list);
    }

    /// <summary>
    /// Retrieves an approval by its ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var approval = await _approvalRepository.GetByIdAsync(id);
        return Ok(approval);
    }

    /// <summary>
    /// Retrieves pending approvals for a specific approver.
    /// </summary>
    [HttpGet("my-pending/{approverId}")]
    public async Task<IActionResult> GetMyPendingApprovals(string approverId)
    {
        var approvals = await _approvalRepository.GetMyPendingApprovalRequestsAsync(approverId);
        return Ok(approvals);
    }


    /// <summary>
    /// Retrieves approved approvals for a specific approver.
    /// </summary>
    [HttpGet("my-approved/{approverId}")]
    public async Task<IActionResult> GetMyApprovedApprovals(string approverId)
    {
        var approvals = await _approvalRepository.GetMyApprovedApprovalRequestsAsync(approverId);
        return Ok(approvals);
    }

    /// <summary>
    /// Retrieves rejected approvals for a specific approver.
    /// </summary>
    [HttpGet("my-rejected/{approverId}")]
    public async Task<IActionResult> GetMyRejectedApprovals(string approverId)
    {
        var approvals = await _approvalRepository.GetMyRejectedApprovalRequestsAsync(approverId);
        return Ok(approvals);
    }

    /// <summary>
    /// Retrieves all approval requests for a specific approver.
    /// </summary>
    [HttpGet("my-requests/{approverId}")]
    public async Task<IActionResult> GetMyApprovalRequests(string approverId)
    {
        var approvals = await _approvalRepository.GetMyApprovalRequestsAsync(approverId);
        return Ok(approvals);
    }

    /// <summary>
    /// Retrieves all approvals for a specific approver.
    /// </summary>
    [HttpGet("my-approvals/{approverId}")]
    public async Task<IActionResult> GetMyApprovals(string approverId)
    {
        var approvals = await _approvalRepository.GetMyApprovalRequestsAsync(approverId);
        return Ok(approvals);
    }

    /// <summary>
    /// Submits a new approval request.
    /// </summary>
    [HttpPost("submit")]
    public IActionResult Submit([FromBody] ApprovalDto dto)
    {
        dto.ApprovalId = Guid.NewGuid();
        dto.Status = "Pending";
        dto.RequestedAt = DateTime.UtcNow;
        _approvals.Add(dto);
        return Ok(new { ApprovalId = dto.ApprovalId });
    }

    /// <summary>
    /// Submits a approval request.
    /// </summary>
    [HttpPost("approve")]
    public async Task<IActionResult> Approve(ApproveRequestDto approveRequest)
    {
        if(ModelState.IsValid == false)
        {
            return BadRequest(ModelState);
        }
        if (string.IsNullOrEmpty(approveRequest.ApprovalId))
        {
            return BadRequest("ApprovalId is required.");
        }
        if (string.IsNullOrEmpty(approveRequest.CurrentApproverEmployeeNumber))
        {
            return BadRequest("CurrentApproverEmployeeNumber is required.");
        }

        var approval = await _approvalRepository.ApproveRequestAsync(approveRequest.ApprovalId, approveRequest.Comment, approveRequest.CurrentApproverEmployeeNumber);
        
        return Ok(approval);
    }


    #endregion
}
