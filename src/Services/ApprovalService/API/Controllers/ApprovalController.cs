using Microsoft.AspNetCore.Mvc;
using ApprovalService.API.Models;
using ApprovalService.Infrastructure.Repositories;
using ApprovalService.Domain.Entities;
using BuildingBlocks.Core.Infrastructure.Data.Interfaces;

namespace ApprovalService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApprovalController : ControllerBase
{
    private readonly IRepository<Approval> _approvalRepository;

    public ApprovalController(IRepository<Approval> approvalRepository)
    {
        _approvalRepository = approvalRepository;
    }

    private static readonly List<ApprovalDto> _approvals = new()
    {
        new ApprovalDto { ApprovalId = Guid.NewGuid(), RequestId = "REQ001", ApproverName = "Alice", Status = "Pending", RequestedAt = DateTime.UtcNow },
        new ApprovalDto { ApprovalId = Guid.NewGuid(), RequestId = "REQ002", ApproverName = "Bob", Status = "Approved", RequestedAt = DateTime.UtcNow }
    };

    [HttpGet]
    public IActionResult GetAll()
    {
        _approvalRepository.GetAllAsync().ContinueWith(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                return Ok(task.Result);
            }
            return StatusCode(500, "Internal server error");
        });
        // For simplicity, returning the static list instead of fetching from repository
        return Ok(_approvals);
    }

    [HttpPost("submit")]
    public IActionResult Submit([FromBody] ApprovalDto dto)
    {
        dto.ApprovalId = Guid.NewGuid();
        dto.Status = "Pending";
        dto.RequestedAt = DateTime.UtcNow;
        _approvals.Add(dto);
        return Ok(new { ApprovalId = dto.ApprovalId });
    }
}


