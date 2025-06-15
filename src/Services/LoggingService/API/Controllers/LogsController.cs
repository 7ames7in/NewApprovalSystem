using Microsoft.AspNetCore.Http;
using LoggingService.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using LoggingService.Infrastructure.Dtos;
using System.Text.Json;

namespace LoggingService.API.Controllers;

[ApiController]
[Route("api/logs")]
public class LogsController : ControllerBase
{
    private readonly ILoggingRepository _repo;
    
    public LogsController(ILoggingRepository repo)
    {
        _repo = repo;
    }

    [HttpPost]
    public async Task<IActionResult> PostLog([FromBody] List<LogEventDto> logEvents)
    {
        if (logEvents == null || logEvents.Count == 0)
            return BadRequest();

        // 여러 로그를 저장
        foreach (var logEvent in logEvents)
        {
            await _repo.SaveLogAsync(logEvent);
        }

        return Ok();
    }

    // [HttpPost]
    // public async Task<IActionResult> PostLog([FromBody] object logEvent)
    // {
    //     LogEventDto? logEventDto = null;
    //     try
    //     {
    //         logEventDto = logEvent is JsonElement jsonElement 
    //             ? System.Text.Json.JsonSerializer.Deserialize<LogEventDto>(jsonElement.GetRawText()) 
    //             : logEvent as LogEventDto;
    //     }
    //     catch (JsonException ex)
    //     {
    //         return BadRequest($"Invalid log event data: {ex.Message}");
    //     }

    //     if (logEventDto == null)
    //     {
    //         return BadRequest("Invalid log event data.");
    //     }

    //     await _repo.SaveLogAsync(logEventDto);
    //     return Ok();
    // }
}

