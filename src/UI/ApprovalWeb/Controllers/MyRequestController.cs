using ApprovalWeb.Models;
using ApprovalWeb.Services;
using ApprovalWeb.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text.Json;
using BuildingBlocks.Core.Extensions;
using Azure;
using Serilog;

namespace ApprovalWeb.Controllers;

[Authorize]
[ApiVersion("2")]
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
    [ApiVersion("1", Deprecated = true)]
    public async Task<IActionResult> Index()
    {
        var user = HttpContext.User;

        // Store user information in ViewBag for use in the view
        ViewBag.UserId = _userContext.UserId;
        ViewBag.UserName = _userContext.UserName;
        ViewBag.Email = _userContext.Email;
        ViewBag.Role = _userContext.Role;
        ViewBag.Department = _userContext.Department;

        // Get the page number from the query string, default to 1 if not provided
        int pageNumber = 1;
        if (Request.Query.ContainsKey("page") && int.TryParse(Request.Query["page"], out var page))
        {
            pageNumber = page > 0 ? page : 1;
        }
        
        // Fetch the user's approval requests
        var myrequestlist = await _apiService.GetMyRequestsAsync(_userContext.UserId ?? string.Empty);
        
        // Filtering by User Name or Request Title
        string? searchUserName = Request.Query["userName"];
        string? searchRequestTitle = Request.Query["requestTitle"];

        if (!string.IsNullOrWhiteSpace(searchUserName))
        {
            myrequestlist = myrequestlist
            .Where(r => !string.IsNullOrEmpty(r.ApplicantName) && r.ApplicantName.Contains(searchUserName, StringComparison.OrdinalIgnoreCase))
            .ToList();
        }

        if (!string.IsNullOrWhiteSpace(searchRequestTitle))
        {
            myrequestlist = myrequestlist
            .Where(r => !string.IsNullOrEmpty(r.RequestTitle) && r.RequestTitle.Contains(searchRequestTitle, StringComparison.OrdinalIgnoreCase))
            .ToList();
        }
        
        // Fetch the paginated data for the specified page
        var paginatedRequests = myrequestlist.ToPaginated(pageSize: 10, comparer: ApprovalRequestViewModel.DateComparer);

        ViewBag.TotalPage = paginatedRequests.PagesCount;
        ViewBag.CurrentPage = pageNumber;

        var currentPageList = paginatedRequests[pageNumber - 1];
        #region Foreach for Debugging

        // foreach (var currentPageList in paginatedRequests)
        // {

        //     Log.Information($"[Page {currentPageList.Ordinal}]");
        //     foreach (var request in currentPageList)
        //     {
        //             Log.Information($"{request.RequestTitle} - {request.ApplicantName} - {request.RequestedAt:yyyy-MM-dd}");
        //     }
        // }


        #endregion
        return View(currentPageList);
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
