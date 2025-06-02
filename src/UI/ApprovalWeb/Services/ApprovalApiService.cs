using ApprovalWeb.Models;
using System.Net.Http.Json;
using ApprovalWeb.Interfaces;

namespace ApprovalWeb.Services;

public class ApprovalApiService : IApprovalService
{
    private readonly HttpClient _http;

    public ApprovalApiService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("ApprovalApi");
    }

    public async Task<List<ApprovalRequestViewModel>> GetMyApprovalRequestsAsync(string approverId)
        => await _http.GetFromJsonAsync<List<ApprovalRequestViewModel>>($"api/approval/my-approvals/{approverId}") ?? new List<ApprovalRequestViewModel>();

    public async Task<Guid> SubmitApprovalAsync(ApprovalRequestViewModel model)
    {
        var response = await _http.PostAsJsonAsync("api/approval/submit", model);
        var result = await response.Content.ReadFromJsonAsync<Dictionary<string, Guid>>();
        return result?["ApprovalId"] ?? Guid.Empty;
    }

    public async Task<ApprovalRequestViewModel?> GetByIdAsync(string requestId)
        => await _http.GetFromJsonAsync<ApprovalRequestViewModel>($"api/approval/{requestId}");

    public async Task UpdateStatusAsync(string requestId, string status, string? comment)
    {
        var content = JsonContent.Create(new { requestId, status, comment });
        await _http.PostAsync("api/approval/update-status", content);
    }
}
