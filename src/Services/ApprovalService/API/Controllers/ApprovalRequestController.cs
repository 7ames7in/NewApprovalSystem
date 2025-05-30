using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApprovalService.Domain.Entities;
using ApprovalService.Domain.Interfaces;

namespace ApprovalService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApprovalRequestController : ControllerBase
    {
        private readonly IApprovalRequestRepository<ApprovalRequest> _approvalRequestRepository;
        private readonly ILogger<ApprovalRequestController> _logger;
        public ApprovalRequestController(IApprovalRequestRepository<ApprovalRequest> approvalRequestRepository, ILogger<ApprovalRequestController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _approvalRequestRepository = approvalRequestRepository;
        }

        [HttpGet("my-requests/{userId}")]
        public async Task<IActionResult> GetMyRequests(string userId)
        {
            var requests = await _approvalRequestRepository.GetMyRequestsAsync(userId);
            return Ok(requests);
        }

        [HttpGet("my-pending-requests/{userId}")]
        public async Task<IActionResult> GetMyPendingRequests(string userId)
        {
            var requests = await _approvalRequestRepository.GetMyPendingRequestsAsync(userId);
            return Ok(requests);
        }

        [HttpGet("my-approved-requests/{userId}")]
        public async Task<IActionResult> GetMyApprovedRequests(string userId)
        {
            var requests = await _approvalRequestRepository.GetMyApprovedRequestsAsync(userId);
            return Ok(requests);
        }

        [HttpGet("my-rejected-requests/{userId}")]
        public async Task<IActionResult> GetMyRejectedRequests(string userId)
        {
            var requests = await _approvalRequestRepository.GetMyRejectedRequestsAsync(userId);
            return Ok(requests);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRequestById(Guid requestid)
        {
            var request = await _approvalRequestRepository.GetRequestByIdAsync(requestid);
            if (request == null)
            {
                return NotFound();
            }
            return Ok(request);
        }
        
        [HttpPost("create")]
        public async Task<IActionResult> CreateApprovalRequest([FromBody] ApprovalRequest approvalRequest)
        {
            _logger.LogInformation("Creating a new approval request.");
            _logger.LogDebug("Approval Request Details: {@ApprovalRequest}", approvalRequest);
            _logger.LogInformation("Approval Request Created at: {Time}", DateTime.UtcNow);
            
            if (approvalRequest == null)
            {
                return BadRequest("Approval request cannot be null.");
            }

            var createdRequest = await _approvalRequestRepository.CreateApprovalRequestAsync(approvalRequest);
            return CreatedAtAction(nameof(GetRequestById), new { id = createdRequest.ApprovalId }, createdRequest);
        }
        
    }
}
