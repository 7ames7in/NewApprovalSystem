using ApprovalWeb.Models;
using ApprovalWeb.Services;
using ApprovalWeb.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text.Json;

namespace ApprovalWeb.Controllers;

[Authorize]
public class MyRequestController : Controller
{
    private readonly IApprovalRequestService _apiService;

    // Constructor to inject the approval request service
    public MyRequestController(IApprovalRequestService apiService)
    {
        _apiService = apiService;
    }

    // Displays the list of approval requests for the logged-in user
    public async Task<IActionResult> Index()
    {
        var user = HttpContext.User;

        // Retrieve user information from claims
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userName = user.Identity?.Name;
        var email = user.FindFirst(ClaimTypes.Email)?.Value;
        var role = user.FindFirst(ClaimTypes.Role)?.Value;
        var department = user.FindFirst("Department")?.Value;

        // Redirect to login if user ID is not found
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }

        // Store user information in ViewBag for use in the view
        ViewBag.UserId = userId;
        ViewBag.UserName = userName;
        ViewBag.Email = email;
        ViewBag.Role = role;
        ViewBag.Department = "test"; // Replace with actual department if available

        // Fetch the user's approval requests
        var requests = await _apiService.GetMyRequestsAsync(userId);
        return View(requests);
    }

    // Handles the creation of a new approval request
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] ApprovalRequestViewModel model)
    {
        // Validate the model
        if (!ModelState.IsValid) return View(model);

        // Handle file uploads
        if (model.Files != null)
        {
            foreach (var file in model.Files)
            {
                var filePath = Path.Combine("wwwroot/uploads", file.FileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                // Create attachment metadata
                var attachment = new ApprovalAttachmentViewModel
                {
                    FileName = file.FileName,
                    FilePath = filePath,
                    FileSize = file.Length,
                    FileType = file.ContentType,
                    UploadedByEmployeeNumber = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Unknown",
                    UploadedAt = DateTime.UtcNow
                };
                model.Attachments.Add(attachment);
            }
        }

        // Deserialize steps from JSON
        if (!string.IsNullOrEmpty(model.StepsJson))
        {
            var steps = JsonSerializer.Deserialize<List<ApprovalStepViewModel>>(model.StepsJson);
            if (steps != null)
            {
                model.Steps.AddRange(steps);
            }
        }

        // Create the approval request
        var result = await _apiService.CreateApprovalRequestAsync(model);
        if (result.IsSuccess)
        {
            TempData["SuccessMessage"] = "Approval request created successfully.";
            return RedirectToAction("Index");
        }

        // Handle errors
        ModelState.AddModelError(string.Empty, "An error occurred while creating the approval request.");
        return View(model);
    }

    // Displays the form for creating a new approval request
    public IActionResult Create()
    {
        var model = new ApprovalRequestViewModel();
        return View(model);
    }

    // Displays an alternate form for creating a new approval request
    public IActionResult Create2()
    {
        var model = new ApprovalRequestViewModel();
        return View(model);
    }

    // Handles creation of approval requests with file uploads and JSON data
    [HttpPost]
    [Route("CreateWithFiles")]
    public async Task<IActionResult> CreateWithFiles(
        List<IFormFile> files,
        [FromForm] string StepsJson,
        [FromForm] string AttachmentsJson)
    {
        // Save uploaded files
        foreach (var file in files)
        {
            var filePath = Path.Combine("wwwroot/uploads", file.FileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
        }

        // Deserialize JSON data
        var steps = JsonSerializer.Deserialize<List<ApprovalStepViewModel>>(StepsJson);
        var attachments = JsonSerializer.Deserialize<List<ApprovalAttachmentViewModel>>(AttachmentsJson);

        // Process the data (e.g., save to database)

        return Ok(new { Message = "Success" });
    }

    // Displays the details of a specific approval request
    public async Task<IActionResult> Details(string id)
    {
        var model = await _apiService.GetRequestByIdAsync(id);
        return View(model);
    }

    // Displays the form for modifying an existing approval request
    public async Task<IActionResult> Modify(string id)
    {
        var model = new ApprovalRequestViewModel();

        // Example: Prepopulate steps for the approval request
        List<ApprovalStepViewModel> list = new()
        {
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

        // Simulate async delay
        await Task.Delay(1000);

        return View(model);
    }
}
