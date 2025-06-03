using ApprovalWeb.Models;
using ApprovalWeb.Services;
using ApprovalWeb.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ApprovalWeb.Controllers;

// Controller for handling approval-related actions
[Authorize]
public class ApprovalController : Controller
{
    private readonly IApprovalService _apiService;

    // Constructor to inject the approval service
    public ApprovalController(IApprovalService apiService)
    {
        _apiService = apiService;
    }

    // Displays the list of approval requests for the logged-in user
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                     ?? throw new InvalidOperationException("User ID not found in claims.");
        var approvals = await _apiService.GetMyApprovalRequestsAsync(userId);
        return View(approvals);
    }

    // Displays the form to create a new approval request
    public IActionResult Create()
    {
        var model = new ApprovalRequestViewModel();
        return View(model);
    }

    // Handles the submission of a new approval request
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

    // Displays the details of a specific approval request
    public IActionResult Details(string id)
    {
        var model = _apiService.GetByIdAsync(id).Result; // Fetch the approval request details
        return View(model);
    }
}
