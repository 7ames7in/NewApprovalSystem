using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApprovalService.Domain.Entities;
using ApprovalService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

using Serilog;
using BuildingBlocks.EventBus;
using BuildingBlocks.EventContracts;
using ApprovalService.Application.Interfaces;

namespace ApprovalService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ApprovalRequestController : ControllerBase
    {
        private readonly IEventBus _eventBus;
        private readonly IUserServiceProxy _userServiceProxy;
        private readonly IApprovalRequestRepository<ApprovalRequestWithCurrentStepDto> _approvalRequestRepository;
        private readonly ILogger<ApprovalRequestController> _logger;
        
        public ApprovalRequestController(
            IApprovalRequestRepository<ApprovalRequestWithCurrentStepDto> approvalRequestRepository,
            ILogger<ApprovalRequestController> logger, IUserServiceProxy userServiceProxy, IEventBus eventBus)
        {
            _userServiceProxy = userServiceProxy ?? throw new ArgumentNullException(nameof(userServiceProxy));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _approvalRequestRepository = approvalRequestRepository ?? throw new ArgumentNullException(nameof(approvalRequestRepository));
        }

        /// <summary>
        /// Get all approval requests for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of approval requests.</returns>
        [HttpGet("my-requests/{userId}")]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> GetMyRequests(string userId)
        {
            // IQueryable을 먼저 가져온다
            var query = _approvalRequestRepository.GetApprovalRequestsWithCurrentStep(userId);

            // 여기서 실제 쿼리를 날림 (비동기 실행)
            var requests = await query.ToListAsync();

            return Ok(requests);
        }

        [HttpGet("my-requests2/{userId}")]
        public async Task<IActionResult> GetMyRequests2(string userId)
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
        public async Task<IActionResult> CreateApprovalRequest([FromBody] ApprovalRequestWithCurrentStepDto approvalRequest)
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
                if (step.Sequence == 1)
                {
                    // Set the first step as the current step
                    approvalRequest.CurrentApproverEmployeeNumber = step.ApproverEmployeeNumber;
                    approvalRequest.CurrentStep = step.Sequence;
                    approvalRequest.CurrentActionStatus = step.ActionStatus;
                    approvalRequest.CurrentApproverName = step.ApproverName;
                }
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

            UserInfoDto? user = await _userServiceProxy.GetUserByIdAsync(approvalRequest.CurrentApproverEmployeeNumber);
            if (user == null)
            {
                _logger.LogWarning("User not found for EmployeeNumber: {EmployeeNumber}", approvalRequest.CurrentApproverEmployeeNumber);
                // Handle the null case as appropriate, e.g., return BadRequest or continue
            }
            else
            {
                _logger.LogInformation("User found: {@User}", user);

                // Publish an event to RabbitMQ for email notification

                // var emailEvent = new EmailEvent
                // {
                //     To = "wonjin.byun@volvo.com",
                //     Subject = "[Approval Request] " + createdRequest.RequestTitle,
                //     Body = "<p>Dear " + user.Name + ",</p>" +
                //            "<p>You have a new approval request:</p>" +
                //            "<p><strong>Title:</strong> " + createdRequest.RequestTitle + "</p>" +
                //            "<p><strong>Requested At:</strong> " + createdRequest.RequestedAt.ToString("yyyy-MM-dd HH:mm") + "</p>" +
                //            "<p><strong>Current Step:</strong> " + createdRequest.CurrentStep + "</p>" +
                //            "<p><strong>Current Approver:</strong> " + createdRequest.CurrentApproverName + "</p>" +
                //            "<p>Please review the request at your earliest convenience.</p>"
                // };
                // var eventBus = new RabbitMQEventBus("localhost", "myuser", "mypassword");
                // await eventBus.PublishAsync(emailEvent);
                
                var emailHelper = new EmailHelper(HttpContext.RequestServices);
                await emailHelper.SendEmailAsync(
                    to: user.Email,
                    approvalRequest: createdRequest
                );
            }

            // Return the created approval request with a 201 Created status
            return CreatedAtAction(nameof(GetRequestById), new { approvalId = createdRequest.ApprovalId }, createdRequest);
        }
    }
}
