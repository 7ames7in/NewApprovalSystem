using ApprovalWeb.Models;
using ApprovalWeb.Interfaces;
using System.Collections;
using System.Linq;

namespace ApprovalWeb.Services;

public class ApprovalRequestApiService : IApprovalRequestService
{
    private readonly HttpClient _http;
    private static readonly ILogger<ApprovalRequestApiService> Logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<ApprovalRequestApiService>();

    public ApprovalRequestApiService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("ApprovalApi");
    }
    public async Task<ResultViewModel> CreateApprovalRequestAsync(ApprovalRequestViewModel model)
    {
        model.ApprovalId = Guid.NewGuid(); // Ensure RequestId is set
        var response = await _http.PostAsJsonAsync("api/approvalrequest/create", model);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            // Log the error or handle it as needed
            Logger.LogError($"Error creating approval request: {errorContent}");
            return new ResultViewModel { IsSuccess = false, Message = errorContent };
        }
        return new ResultViewModel { IsSuccess = true }; // Example implementation
    }

    public async Task<IEnumerable<ApprovalRequestViewModel>> GetMyRequestsAsync(string userId)
        => await _http.GetFromJsonAsync<IEnumerable<ApprovalRequestViewModel>>($"api/approvalrequest/my-requests/{userId}") ?? new List<ApprovalRequestViewModel>();

    public async Task<IEnumerable<ApprovalRequestViewModel>> GetMyPendingRequestsAsync(string userId)
        => await _http.GetFromJsonAsync<IEnumerable<ApprovalRequestViewModel>>($"api/approvalrequest/my-pending-requests/{userId}") ?? new List<ApprovalRequestViewModel>();

    public async Task<IEnumerable<ApprovalRequestViewModel>> GetMyApprovedRequestsAsync(string userId)
        => await _http.GetFromJsonAsync<IEnumerable<ApprovalRequestViewModel>>($"api/approvalrequest/my-approved-requests/{userId}") ?? new List<ApprovalRequestViewModel>();

    public async Task<IEnumerable<ApprovalRequestViewModel>> GetMyRejectedRequestsAsync(string userId)
        => await _http.GetFromJsonAsync<IEnumerable<ApprovalRequestViewModel>>($"api/approvalrequest/my-rejected-requests/{userId}") ?? new List<ApprovalRequestViewModel>();

    public async Task<ApprovalRequestViewModel?> GetRequestByIdAsync(string requestid)
        => await _http.GetFromJsonAsync<ApprovalRequestViewModel>($"api/approvalrequest/{requestid}");
    
    public async Task ApproveRequestAsync(string requestId)
    {
        var content = JsonContent.Create(new { requestId });
        await _http.PostAsync("api/approvalrequest/approve", content);
    }
    public async Task RejectRequestAsync(string requestId)
    {
        var content = JsonContent.Create(new { requestId });
        await _http.PostAsync("api/approvalrequest/reject", content);
    }
}
