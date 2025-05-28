using Microsoft.AspNetCore.Mvc;
using ApprovalService.API.Models;
using ApprovalService.Infrastructure.Repositories;
using ApprovalService.Domain.Entities;
using BuildingBlocks.Core.Infrastructure.Data.Interfaces;
using System.Threading.Tasks;

namespace ApprovalService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApprovalController : ControllerBase
{
    private readonly IRepository<ApprovalRequest> _approvalRepository;

    public ApprovalController(IRepository<ApprovalRequest> approvalRepository)
    {
        _approvalRepository = approvalRepository;
    }

#region Sample Data
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


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // await _approvalRepository.GetAllAsync().ContinueWith(task =>
        // {
        //     if (task.IsCompletedSuccessfully)
        //     {
        //         return Ok(task.Result);
        //     }
        //     return StatusCode(500, "Internal server error");
        // });

        // return Ok();
        //For simplicity, returning the static list instead of fetching from repository


        var list = await _approvalRepository.GetAllAsync();
        return Ok(list);
        // return Ok(_approvalRequests);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var approval = await _approvalRepository.GetByIdAsync(id);
        // if (approval == null)
        // {
        //     return NotFound();
        // }
        //return Ok(_approvalRequest);
        return Ok(approval);
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


