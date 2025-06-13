using ApprovalWeb.Models;
using ApprovalWeb.Services;
using ApprovalWeb.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text.Json;

namespace ApprovalWeb.Controllers;

[Authorize]
public class MyRequestController : BaseController
{
    private readonly IApprovalRequestService _apiService;
    private readonly IUserContext _userContext;

    public MyRequestController(IApprovalRequestService apiService, IUserContext userContext)
    {
        _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
        _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
    }

    // Displays the list of approval requests for the logged-in user
    public async Task<IActionResult> Index()
    {
        var user = HttpContext.User;

        // Store user information in ViewBag for use in the view
        ViewBag.UserId = _userContext.UserId;
        ViewBag.UserName = _userContext.UserName;
        ViewBag.Email = _userContext.Email;
        ViewBag.Role = _userContext.Role;
        ViewBag.Department = _userContext.Department;

        // Fetch the user's approval requests
        var requests = await _apiService.GetMyRequestsAsync(_userContext.UserId?? string.Empty);
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
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var extension = Path.GetExtension(file.FileName);
                var filePath = Path.Combine("wwwroot/uploads", file.FileName);

                // Ensure unique file name if file already exists
                int counter = 1;
                while (System.IO.File.Exists(filePath))
                {
                    filePath = Path.Combine("wwwroot/uploads", $"{fileName}_{counter}{extension}");
                    counter++;
                }

                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                // Create attachment metadata
                var attachment = new ApprovalAttachmentViewModel
                {
                    FileName = Path.GetFileName(filePath),
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
