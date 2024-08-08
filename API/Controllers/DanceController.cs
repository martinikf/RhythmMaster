using Application.Interfaces.Services;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("dance")]
[ApiController]
public class DanceController(IDanceService danceService) : ControllerBase
{ 
    private readonly IDanceService _danceService = danceService;
    
    [Authorize(Roles=$"{nameof(Role.Management)}, {nameof(Role.Administrator)}")]
    [HttpPost("createDance")]
    public async Task<IActionResult> CreateDance([FromBody] string danceName)
    {
        var result = await _danceService.CreateDanceAsync(danceName);
        
        return result switch
        {
            SuccessResult => Ok(),
            ErrorResult errorResult => Problem(errorResult.Message),
            _ => Problem("An unknown error occurred")
        };
    }
    
    [Authorize(Roles=$"{nameof(Role.Management)},{nameof(Role.Administrator)}")]
    [HttpPost("removeDance")]
    public async Task<IActionResult> RemoveDance([FromBody] DanceId danceId)
    {
        var result = await _danceService.RemoveDanceAsync(danceId);
        
        return result switch
        {
            SuccessResult => Ok(),
            ErrorResult errorResult => Problem(errorResult.Message),
            _ => Problem("An unknown error occurred")
        };
    }
    
    [HttpGet("getDanceById")]
    public async Task<IActionResult> GetDanceById([FromQuery] DanceId danceId)
    {
        var result = await _danceService.GetDanceByIdAsync(danceId);

        return result switch
        {
            SuccessResult<Dance> => Ok(result.Data),
            ErrorResult<Dance> errorResult => Problem(errorResult.Message),
            _ => Problem("An unknown error occurred")
        };
    }
    
    [HttpGet("getAllDances")]
    public async Task<IActionResult> GetAllDances()
    {
        var result = await _danceService.GetAllDancesAsync();

        return result switch
        {
            SuccessResult<IEnumerable<Dance>> => Ok(result.Data),
            ErrorResult<IEnumerable<Dance>> errorResult => Problem(errorResult.Message),
            _ => Problem("An unknown error occurred")
        };
    }
    
}