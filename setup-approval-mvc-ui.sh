#!/bin/bash

set -e

echo "🔧 ApprovalWeb MVC UI 프로젝트를 솔루션에 추가 중..."

# 1. MVC 프로젝트 생성
mkdir -p src/UI
cd src/UI

dotnet new mvc -n ApprovalWeb --force
sleep 3
cd ../..

# 2. 솔루션에 프로젝트 추가
dotnet sln ApprovalSystem.sln add src/UI/ApprovalWeb/ApprovalWeb.csproj
sleep 3

# 3. 폴더 및 파일 생성
mkdir -p src/UI/ApprovalWeb/Controllers
mkdir -p src/UI/ApprovalWeb/Views/Approval
mkdir -p src/UI/ApprovalWeb/Models
mkdir -p src/UI/ApprovalWeb/Services
sleep 3

# 4. Program.cs 설정 (HttpClient 등록)
cat <<EOF > src/UI/ApprovalWeb/Program.cs
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient("ApprovalApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:5001/"); // ApprovalService.API 주소
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Approval}/{action=Index}/{id?}");

app.Run();
EOF
sleep 3

# 5. ApprovalApiService.cs 생성
cat <<EOF > src/UI/ApprovalWeb/Services/ApprovalApiService.cs
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
EOF
sleep 3

# 6. ApprovalController.cs (API 연동 반영)
cat <<EOF > src/UI/ApprovalWeb/Controllers/ApprovalController.cs
using ApprovalWeb.Models;
using ApprovalWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApprovalWeb.Controllers;

public class ApprovalController : Controller
{
    private readonly ApprovalApiService _apiService;

    public ApprovalController(ApprovalApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> Index()
    {
        var approvals = await _apiService.GetApprovalsAsync();
        return View(approvals);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ApprovalViewModel model)
    {
        if (ModelState.IsValid)
        {
            await _apiService.SubmitApprovalAsync(model);
            return RedirectToAction("Index");
        }
        return View(model);
    }

    public IActionResult Details(string id)
    {
        var model = new ApprovalViewModel { RequestId = id, ApproverName = "Alice", Status = "Pending", Comments = "From server" };
        return View(model);
    }
}
EOF
sleep 3

echo "✅ ApprovalWeb MVC UI에 Program.cs 및 API 연동 서비스 설정이 완료되었습니다."
