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
}
