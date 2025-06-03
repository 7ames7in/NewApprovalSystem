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

        public ApprovalRequestController(
            IApprovalRequestRepository<ApprovalRequest> approvalRequestRepository, 
            ILogger<ApprovalRequestController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _approvalRequestRepository = approvalRequestRepository ?? throw new ArgumentNullException(nameof(approvalRequestRepository));
        }

        /// <summary>
        /// Get all approval requests for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of approval requests.</returns>
        [HttpGet("my-requests/{userId}")]
        public async Task<IActionResult> GetMyRequests(string userId)
        {
            var requests = await _approvalRequestRepository.GetMyRequestsAsync(userId);
            return Ok(requests);
        }

        /// <summary>
        /// Get all pending approval requests for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of pending approval requests.</returns>
        [HttpGet("my-pending-requests/{userId}")]
        public async Task<IActionResult> GetMyPendingRequests(string userId)
        {
            var requests = await _approvalRequestRepository.GetMyPendingRequestsAsync(userId);
            return Ok(requests);
        }

        /// <summary>
        /// Get all approved approval requests for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of approved approval requests.</returns>
        [HttpGet("my-approved-requests/{userId}")]
        public async Task<IActionResult> GetMyApprovedRequests(string userId)
        {
            var requests = await _approvalRequestRepository.GetMyApprovedRequestsAsync(userId);
            return Ok(requests);
        }

        /// <summary>
        /// Get all rejected approval requests for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of rejected approval requests.</returns>
        [HttpGet("my-rejected-requests/{userId}")]
        public async Task<IActionResult> GetMyRejectedRequests(string userId)
        {
            var requests = await _approvalRequestRepository.GetMyRejectedRequestsAsync(userId);
            return Ok(requests);
        }

        /// <summary>
        /// Get a specific approval request by its ID.
        /// </summary>
        /// <param name="approvalId">The ID of the approval request.</param>
        /// <returns>The approval request if found, otherwise NotFound.</returns>
        [HttpGet("{approvalId}")]
        public async Task<IActionResult> GetRequestById(Guid approvalId)
        {
            var request = await _approvalRequestRepository.GetRequestByIdAsync(approvalId);
            if (request == null)
            {
                return NotFound();
            }
            return Ok(request);
        }

        /// <summary>
        /// Create a new approval request.
        /// </summary>
        /// <param name="approvalRequest">The approval request to create.</param>
        /// <returns>The created approval request with a 201 Created status.</returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateApprovalRequest([FromBody] ApprovalRequest approvalRequest)
        {
            _logger.LogInformation("Creating a new approval request.");
            _logger.LogDebug("Approval Request Details: {@ApprovalRequest}", approvalRequest);

            // Validate the incoming approval request
            if (approvalRequest == null)
                return BadRequest("Approval request cannot be null.");

            if (approvalRequest.Steps == null || !approvalRequest.Steps.Any())
                return BadRequest("At least one approval step must be provided.");

            // Set relationships for approval steps
            foreach (var step in approvalRequest.Steps)
            {
                step.ApprovalRequest = approvalRequest;
                step.ApprovalId = approvalRequest.ApprovalId; // Explicit assignment recommended
            }

            // Set relationships for attachments
            foreach (var attachment in approvalRequest.Attachments)
            {
                attachment.ApprovalRequest = approvalRequest;
                attachment.ApprovalId = approvalRequest.ApprovalId; // Explicit assignment recommended
            }

            // Create the approval request
            var createdRequest = await _approvalRequestRepository.CreateApprovalRequestAsync(approvalRequest);

            _logger.LogInformation("Approval request created successfully with ID: {ApprovalId}", createdRequest.ApprovalId);

            // Return the created approval request with a 201 Created status
            return CreatedAtAction(nameof(GetRequestById), new { approvalId = createdRequest.ApprovalId }, createdRequest);
        }
    }
}
