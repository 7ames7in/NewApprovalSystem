using ApprovalWeb.Models;
using ApprovalWeb.Services;
using ApprovalWeb.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApprovalWeb.Controllers;

public class ApprovalRequestController : Controller
{
    private readonly IApprovalRequestService _apiService;

    public ApprovalRequestController(IApprovalRequestService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> Index()
    {
        //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userId = "EC20505";
        var requests = await _apiService.GetMyRequestsAsync(userId);
        return View(requests);
    }

    public IActionResult Create()
    {
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
                Comment = ""
            },
            new ApprovalStepViewModel
            {
                StepId = Guid.NewGuid(),
                ApprovalId = Guid.NewGuid(),
                ApproverEmployeeNumber = "E54321",
                ApproverName = "Charlie",
                StepType = "Final Approval",
                Sequence = 3,
                IsFinalApprover = true,
                ActionStatus = "Pending",
                ActionDate = null,
                Comment = ""
            }
        };
        // Example: Adding a default step to the list
        foreach (var step in list)
        {
            model.Steps.Add(step);
        }

        return View(model);
    }
    
}
