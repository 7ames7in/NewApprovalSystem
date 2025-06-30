#!/bin/bash

set -e

mkdir -p src/UI/ApprovalWeb/Services

# IApprovalService.cs
cat <<EOF > src/UI/ApprovalWeb/Services/IApprovalService.cs
using ApprovalWeb.Models;

namespace ApprovalWeb.Services;

public interface IApprovalService
{
    Task<List<ApprovalViewModel>> GetApprovalsAsync();
    Task<Guid> SubmitApprovalAsync(ApprovalViewModel model);
    Task<ApprovalViewModel?> GetByIdAsync(string requestId);
    Task UpdateStatusAsync(string requestId, string status, string? comment);
}
EOF

# ApprovalApiService.cs
cat <<EOF > src/UI/ApprovalWeb/Services/ApprovalApiService.cs
using ApprovalWeb.Models;
using System.Net.Http.Json;

namespace ApprovalWeb.Services;

public class ApprovalApiService : IApprovalService
{
    private readonly HttpClient _http;

    public ApprovalApiService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("ApprovalApi");
    }

    public async Task<List<ApprovalViewModel>> GetApprovalsAsync()
        => await _http.GetFromJsonAsync<List<ApprovalViewModel>>("api/approval") ?? new();

    public async Task<Guid> SubmitApprovalAsync(ApprovalViewModel model)
    {
        var response = await _http.PostAsJsonAsync("api/approval/submit", model);
        var result = await response.Content.ReadFromJsonAsync<Dictionary<string, Guid>>();
        return result?["ApprovalId"] ?? Guid.Empty;
    }

    public async Task<ApprovalViewModel?> GetByIdAsync(string requestId)
        => await _http.GetFromJsonAsync<ApprovalViewModel>($"api/approval/");

    public async Task UpdateStatusAsync(string requestId, string status, string? comment)
    {
        var content = JsonContent.Create(new { requestId, status, comment });
        await _http.PostAsync("api/approval/update-status", content);
    }
}
EOF

# MockApprovalService.cs
cat <<EOF > src/UI/ApprovalWeb/Services/MockApprovalService.cs
using ApprovalWeb.Models;

namespace ApprovalWeb.Services;

public class MockApprovalService : IApprovalService
{
    public Task<List<ApprovalViewModel>> GetApprovalsAsync()
        => Task.FromResult(MockApprovalData.Approvals);

    public Task<Guid> SubmitApprovalAsync(ApprovalViewModel model)
    {
        MockApprovalData.Add(model);
        return Task.FromResult(Guid.NewGuid());
    }

    public Task<ApprovalViewModel?> GetByIdAsync(string requestId)
        => Task.FromResult(MockApprovalData.GetByRequestId(requestId));

    public Task UpdateStatusAsync(string requestId, string status, string? comment)
    {
        MockApprovalData.UpdateStatus(requestId, status, comment);
        return Task.CompletedTask;
    }
}
EOF

# appsettings.json 에 UseMockService 키 추가 알림
echo "📌 appsettings.json에 다음 항목을 추가하세요:"
echo "\"UseMockService\": true"

# Program.cs 등록 코드 예시 출력
echo -e "\n📌 Program.cs에 아래 코드 블록을 삽입하세요:\n"
echo 'var useMock = builder.Configuration.GetValue<bool>("UseMockService");'
echo 'if (useMock)'
echo '{'
echo '    builder.Services.AddScoped<IApprovalService, MockApprovalService>();'
echo '}'
echo 'else'
echo '{'
echo '    builder.Services.AddHttpClient("ApprovalApi", client =>'
echo '    {'
echo '        client.BaseAddress = new Uri("https://localhost:5002/");'
echo '    });'
echo '    builder.Services.AddScoped<IApprovalService, ApprovalApiService>();'
echo '}'

echo "✅ DI 구성 소스 파일이 성공적으로 생성되었습니다."
