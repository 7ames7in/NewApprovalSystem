using ApprovalWeb.Models;
using ApprovalWeb.Interfaces;
using System.Collections;
using System.Linq;

namespace ApprovalWeb.Services;

/// <summary>
/// Service for interacting with the Approval API.
/// </summary>
public class ApprovalRequestApiService : IApprovalRequestService
{
    private readonly HttpClient _http;
    private static readonly ILogger<ApprovalRequestApiService> Logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<ApprovalRequestApiService>();

    /// <summary>
    /// Initializes a new instance of the <see cref="ApprovalRequestApiService"/> class.
    /// </summary>
    /// <param name="factory">The HTTP client factory.</param>
    public ApprovalRequestApiService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("ApprovalApi");
    }

    /// <summary>
    /// Creates a new approval request.
    /// </summary>
    /// <param name="model">The approval request model.</param>
    /// <returns>A result indicating success or failure.</returns>
    public async Task<ResultViewModel> CreateApprovalRequestAsync(ApprovalRequestViewModel model)
    {
        model.ApprovalId = Guid.NewGuid(); // Ensure ApprovalId is set
        var response = await _http.PostAsJsonAsync("api/approvalrequest/create", model);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            Logger.LogError($"Error creating approval request: {errorContent}");
            return new ResultViewModel { IsSuccess = false, Message = errorContent };
        }

        return new ResultViewModel { IsSuccess = true }; // Example implementation
    }

    /// <summary>
    /// Retrieves all approval requests for a specific user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>A list of approval requests.</returns>
    public async Task<IEnumerable<ApprovalRequestViewModel>> GetMyRequestsAsync(string userId)
        => await _http.GetFromJsonAsync<IEnumerable<ApprovalRequestViewModel>>($"api/approvalrequest/my-requests/{userId}") ?? new List<ApprovalRequestViewModel>();

    /// <summary>
    /// Retrieves pending approval requests for a specific user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>A list of pending approval requests.</returns>
    public async Task<IEnumerable<ApprovalRequestViewModel>> GetMyPendingRequestsAsync(string userId)
        => await _http.GetFromJsonAsync<IEnumerable<ApprovalRequestViewModel>>($"api/approvalrequest/my-pending-requests/{userId}") ?? new List<ApprovalRequestViewModel>();

    /// <summary>
    /// Retrieves approved approval requests for a specific user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>A list of approved approval requests.</returns>
    public async Task<IEnumerable<ApprovalRequestViewModel>> GetMyApprovedRequestsAsync(string userId)
        => await _http.GetFromJsonAsync<IEnumerable<ApprovalRequestViewModel>>($"api/approvalrequest/my-approved-requests/{userId}") ?? new List<ApprovalRequestViewModel>();

    /// <summary>
    /// Retrieves rejected approval requests for a specific user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>A list of rejected approval requests.</returns>
    public async Task<IEnumerable<ApprovalRequestViewModel>> GetMyRejectedRequestsAsync(string userId)
        => await _http.GetFromJsonAsync<IEnumerable<ApprovalRequestViewModel>>($"api/approvalrequest/my-rejected-requests/{userId}") ?? new List<ApprovalRequestViewModel>();

    /// <summary>
    /// Retrieves an approval request by its ID.
    /// </summary>
    /// <param name="requestId">The request ID.</param>
    /// <returns>The approval request model, or null if not found.</returns>
    public async Task<ApprovalRequestViewModel?> GetRequestByIdAsync(string requestId)
        => await _http.GetFromJsonAsync<ApprovalRequestViewModel>($"api/approvalrequest/{requestId}");

    /// <summary>
    /// Approves an approval request.
    /// </summary>
    /// <param name="requestId">The request ID.</param>
    public async Task ApproveRequestAsync(string requestId)
    {
        var content = JsonContent.Create(new { requestId });
        await _http.PostAsync("api/approvalrequest/approve", content);
    }

    /// <summary>
    /// Rejects an approval request.
    /// </summary>
    /// <param name="requestId">The request ID.</param>
    public async Task RejectRequestAsync(string requestId)
    {
        var content = JsonContent.Create(new { requestId });
        await _http.PostAsync("api/approvalrequest/reject", content);
    }
}
