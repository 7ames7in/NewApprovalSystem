using ApprovalWeb.Models;
using ApprovalWeb.Services;
using ApprovalWeb.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        var approvals = await _apiService.GetApprovalsAsync();
        return View(approvals);
    }

    public IActionResult Create()
    {
        var model = new ApprovalViewModel();
        return View(model);
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
        //var model = new ApprovalViewModel { RequestId = id, ApproverName = "Alice", Status = "Pending", Comments = "From server" };
        var model = _apiService.GetByIdAsync(id).Result;
        return View(model);
    }
}
