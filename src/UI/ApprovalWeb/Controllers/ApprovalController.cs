using ApprovalWeb.Models;
using ApprovalWeb.Services;
using ApprovalWeb.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApprovalWeb.Controllers;

public class ApprovalController : Controller
{
    private readonly IApprovalService _apiService;

    public ApprovalController(IApprovalService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User ID not found in claims.");
        var approvals = await _apiService.GetMyApprovalRequestsAsync(userId);
        return View(approvals);
    }

    public IActionResult Create()
    {
        var model = new ApprovalRequestViewModel();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ApprovalRequestViewModel model)
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
        //var model = new ApprovalViewModel { RequestId = id, ApproverName = "Alice", Status = "Pending", Comments = "From server" };
        var model = _apiService.GetByIdAsync(id).Result;
        return View(model);
    }
}
