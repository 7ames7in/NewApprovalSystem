using ApprovalWeb.Models;
using System.Net.Http.Json;
using ApprovalWeb.Interfaces;

namespace ApprovalWeb.Services;

/// <summary>
/// Service class for interacting with the Approval API.
/// Implements the IApprovalService interface.
/// </summary>
public class ApprovalApiService : IApprovalService
{
    private readonly HttpClient _http;

    /// <summary>
    /// Initializes a new instance of the ApprovalApiService class.
    /// </summary>
    /// <param name="factory">The HTTP client factory used to create the HTTP client.</param>
    public ApprovalApiService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("ApprovalApi");
    }

    /// <summary>
    /// Retrieves a list of approval requests for a specific approver.
    /// </summary>
    /// <param name="approverId">The ID of the approver.</param>
    /// <returns>A list of approval requests.</returns>
    public async Task<IEnumerable<ApprovalRequestViewModel>> GetMyApprovalRequestsAsync(string approverId)
    {
        return await _http.GetFromJsonAsync<IEnumerable<ApprovalRequestViewModel>>($"api/approval/my-approvals/{approverId}") 
               ?? new List<ApprovalRequestViewModel>();
    }

    // public async Task<IEnumerable<ApprovalRequestViewModel>> GetMyApprovalRequestsAsync(string approverId)
    //     => await _http.GetFromJsonAsync<IEnumerable<ApprovalRequestViewModel>>($"api/approvalrequest/my-requests/{approverId}") ?? new List<ApprovalRequestViewModel>();


    /// <summary>
    /// Submits a new approval request.
    /// </summary>
    /// <param name="model">The approval request model to submit.</param>
    /// <returns>The ID of the submitted approval request.</returns>
    public async Task<Guid> SubmitApprovalAsync(ApprovalRequestViewModel model)
    {
        var response = await _http.PostAsJsonAsync("api/approval/submit", model);
        var result = await response.Content.ReadFromJsonAsync<Dictionary<string, Guid>>();
        return result?["ApprovalId"] ?? Guid.Empty;
    }

    /// <summary>
    /// Retrieves an approval request by its ID.
    /// </summary>
    /// <param name="requestId">The ID of the approval request.</param>
    /// <returns>The approval request model, or null if not found.</returns>
    public async Task<ApprovalRequestViewModel?> GetByIdAsync(string requestId)
    {
        return await _http.GetFromJsonAsync<ApprovalRequestViewModel>($"api/approval/{requestId}");
    }

    /// <summary>
    /// Updates the status of an approval request.
    /// </summary>
    /// <param name="requestId">The ID of the approval request.</param>
    /// <param name="status">The new status of the approval request.</param>
    /// <param name="comment">An optional comment for the status update.</param>
    public async Task UpdateStatusAsync(string requestId, string status, string? comment)
    {
        var content = JsonContent.Create(new { requestId, status, comment });
        await _http.PostAsync("api/approval/update-status", content);
    }

    /// <summary>
    /// Approves an approval request.
    /// </summary>
    /// <param name="ApprovalId">The ID of the approver.</param>
    /// <param name="comment">An optional comment for the approval.</param>
    /// <returns>A task that represents the asynchronous operation, containing a boolean indicating success.</returns>
    public async Task<bool> ApproveRequestAsync(string approvalId, string? comment, string approverEmployeeNumber)
    {
        var payload = new 
        {
            ApprovalId = approvalId,
            Comment = comment,
            CurrentApproverEmployeeNumber = approverEmployeeNumber
        };

        var response = await _http.PostAsJsonAsync("api/approval/approve", payload);
        if (response.IsSuccessStatusCode)
        {
            // API가 true/false 결과를 Body로 보내는 경우
            var result = await response.Content.ReadFromJsonAsync<bool>();
            return result;
        }
        else
        {
            // 실패 처리
            // 로깅 추가 가능
            return false;
        }
    }

    public Task<bool> RejectRequestAsync(string approverId, string? comment, string approverEmployeeNumber)
    {
        var payload = new 
        {
            ApprovalId = approverId,
            Comment = comment,
            ApproverEmployeeNumber = approverEmployeeNumber
        };

        return _http.PostAsJsonAsync("api/approval/reject", payload)
            .ContinueWith(response => response.Result.IsSuccessStatusCode);
    }
}
