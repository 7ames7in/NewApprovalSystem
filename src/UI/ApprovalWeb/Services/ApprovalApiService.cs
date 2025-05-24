using ApprovalWeb.Models;
using System.Net.Http.Json;

namespace ApprovalWeb.Services;

public class ApprovalApiService
{
    private readonly HttpClient _http;

    public ApprovalApiService(IHttpClientFactory httpClientFactory)
    {
        _http = httpClientFactory.CreateClient("ApprovalApi");
    }

    public async Task<List<ApprovalViewModel>> GetApprovalsAsync()
    {
        return await _http.GetFromJsonAsync<List<ApprovalViewModel>>("api/approval") ?? new();
    }

    public async Task<Guid> SubmitApprovalAsync(ApprovalViewModel model)
    {
        var response = await _http.PostAsJsonAsync("api/approval/submit", model);
        var result = await response.Content.ReadFromJsonAsync<Dictionary<string, Guid>>();
        return result?["ApprovalId"] ?? Guid.Empty;
    }
}
