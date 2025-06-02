using ApprovalWeb.Models;
using ApprovalWeb.Services;
using ApprovalWeb.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ApprovalWeb.Controllers;

[Authorize]
public class ApprovalRequestController : Controller
{
    private readonly IApprovalRequestService _apiService;

    public ApprovalRequestController(IApprovalRequestService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> Index()
    {
        var user = HttpContext.User;

        // 1. 사용자 ID (보통은 Claim의 NameIdentifier)
        var userId = user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "EC20505";

        // 2. 사용자 이름
        var userName = user.Identity?.Name;

        // 3. 사용자 이메일
        var email = user.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

        ViewBag.UserId = userId;
        ViewBag.UserName = userName;
        ViewBag.Email = email;


        //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //var userId = "EC20505";
        var requests = await _apiService.GetMyRequestsAsync(userId);
        return View(requests);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ApprovalRequestViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (!string.IsNullOrEmpty(model.StepsJson))
        {
            var steps = System.Text.Json.JsonSerializer.Deserialize<List<ApprovalStepViewModel>>(model.StepsJson);
            if (steps != null)
            {
                foreach (var step in steps)
                {
                    model.Steps.Add(step);
                }
            }
        }

        var result = await _apiService.CreateApprovalRequestAsync(model);

        if (result.IsSuccess)
        {
            TempData["SuccessMessage"] = "Approval request created successfully.";
            return RedirectToAction("Index");
        }

        ModelState.AddModelError(string.Empty, "An error occurred while creating the approval request.");
        return View(model);
    }

    public IActionResult Create()
    {
        var model = new ApprovalRequestViewModel();

        return View(model);
    }

    public async Task<IActionResult> Details(string id)
    {
        var model = await _apiService.GetRequestByIdAsync(id);
        return View(model);
    }
    
    public async Task<IActionResult> Modify(string id)
    {
        //var model = await _apiService.GetApprovalRequestByIdAsync(id);
        var model = new ApprovalRequestViewModel();

        List<ApprovalStepViewModel> list = new() {
            new ApprovalStepViewModel
            {
                StepId = Guid.NewGuid(),
                ApprovalId = Guid.NewGuid(),
                ApproverEmployeeNumber = "E12345",
                ApproverName = "Alice",
                StepType = "Agreement",
                Sequence = 1,
                IsFinalApprover = false,
                ActionStatus = "Pending",
                ActionDate = null,
                Department = "Finance",
                JobTitle = "Financial Analyst",
                Position = "Analyst",
                Comment = ""
            },
            new ApprovalStepViewModel
            {
                StepId = Guid.NewGuid(),
                ApprovalId = Guid.NewGuid(),
                ApproverEmployeeNumber = "E67890",
                ApproverName = "Bob",
                StepType = "Review",
                Sequence = 2,
                IsFinalApprover = true,
                ActionStatus = "Pending",
                ActionDate = null,
                Department = "HR",
                JobTitle = "HR Manager",
                Position = "Manager",
                Comment = ""
            },
            new ApprovalStepViewModel
            {
                StepId = Guid.NewGuid(),
                ApprovalId = Guid.NewGuid(),
                ApproverEmployeeNumber = "E54321",
                ApproverName = "Charlie",
                StepType = "Approval",
                Sequence = 3,
                IsFinalApprover = true,
                ActionStatus = "Pending",
                ActionDate = null,
                Department = "IT",
                JobTitle = "IT Director",
                Position = "Director",
                Comment = ""
            }
        };
        // Example: Adding a default step to the list
        foreach (var step in list)
        {
            //model.Steps.Add(step);
        }

        await Task.Delay(1000); // Simulate async delay

        return View(model);
    }

}
