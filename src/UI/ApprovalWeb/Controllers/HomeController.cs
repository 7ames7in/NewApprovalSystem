using System.Diagnostics; // Provides classes for interacting with processes, event logs, and performance counters
using Microsoft.AspNetCore.Mvc; // Provides classes for building web applications using MVC
using ApprovalWeb.Models;
using Microsoft.AspNetCore.Diagnostics; // Namespace for application-specific models

namespace ApprovalWeb.Controllers; // Defines the namespace for the controller

// Controller for handling requests to the home page
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger; // Logger instance for logging messages

    // Constructor to initialize the logger
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    // Action method for the home page
    public IActionResult Index()
    {
        return View(); // Returns the default view for the home page
    }

    // Action method for the privacy page
    public IActionResult Privacy()
    {
        return View(); // Returns the default view for the privacy page
    }

    // Action method for handling errors
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)] // Prevents caching of error responses
    public IActionResult Error()
    {
        // Returns the error view with the current request ID
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [ApiExplorerSettings(IgnoreApi = true)] // Ignores this action in API documentation
    [Route("/error")] // Route for handling errors without a specific code
    public IActionResult HandleError() => Problem();

    [ApiExplorerSettings(IgnoreApi = true)] // Ignores this action in API documentation
    [Route("/error-development")] // Route for handling errors without a specific code
    public IActionResult HandleErrorDevelopment(IHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>()!;
            return Problem(exceptionHandlerFeature.Error.StackTrace, 
                           exceptionHandlerFeature.Error.Message, 
                           statusCode: 500, 
                           title: "An error occurred while processing your request.");
        }
        
        // Otherwise, return a generic error view
        return HandleError();
    }

    [ApiExplorerSettings(IgnoreApi = true)] // Ignores this action in API documentation
    [HttpGet("error/{code:int}")] // Route for handling specific error codes
    public IActionResult Error(int code)
    {
        // Log the error code
        _logger.LogError("Error occurred with code: {Code}", code);
        
        // Return the error view with the specified code
        return View(new ErrorViewModel { RequestId = code.ToString() });
    }
}
