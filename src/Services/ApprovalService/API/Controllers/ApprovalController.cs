using Microsoft.AspNetCore.Mvc;
using ApprovalService.API.Models;

namespace ApprovalService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApprovalController : ControllerBase
{
    private static readonly List<ApprovalDto> _approvals = new()
    {
        new ApprovalDto { ApprovalId = Guid.NewGuid(), RequestId = "REQ001", ApproverName = "Alice", Status = "Pending", RequestedAt = DateTime.UtcNow },
        new ApprovalDto { ApprovalId = Guid.NewGuid(), RequestId = "REQ002", ApproverName = "Bob", Status = "Approved", RequestedAt = DateTime.UtcNow }
    };

    [HttpGet]
    public IActionResult GetAll()
    {
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


