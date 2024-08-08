using Application.DTOs.Persons;
using Application.Interfaces.Services;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("credit")]
[ApiController]
public class CreditController(ICreditService creditService, IUserService userService) : ControllerBase
{
    private readonly ICreditService _creditService = creditService;
    private readonly IUserService _userService = userService;
    
    [Authorize(Roles=$"{nameof(Role.Management)},{nameof(Role.Administrator)}")]
    [HttpPost("updateCredit")]
    public async Task<IActionResult> UpdateCredit([FromBody] UpdateCreditDto data)
    {
        var result = await _creditService.UpdateCredit(data);

        return result switch
        {
            SuccessResult => Ok(),
            ErrorResult errorResult => Problem(errorResult.Message),
            _ => Problem("An unknown error occurred")
        };
    }
    
    [Authorize(Roles=$"{nameof(Role.Management)},{nameof(Role.Administrator)}")]
    [HttpGet("getCreditBalance")]
    public async Task<IActionResult> GetCreditBalance([FromQuery] PersonId userId)
    {
        var result = await _creditService.GetCreditBalance(userId);
        
        return result switch
        {
            SuccessResult<int> => Ok(result.Data),
            ErrorResult<int> errorResult => Problem(errorResult.Message),
            _ => Problem("An unknown error occurred")
        };
    }
    
    [Authorize]
    [HttpGet("getMyCreditBalance")]
    public async Task<IActionResult> GetMyCreditBalance()
    {
        if (User.Identity?.Name == null)
        {
            return Unauthorized();
        }
        
        var userResult = await _userService.GetUserByEmail(User.Identity.Name);
        
        switch (userResult)
        {
            case ErrorResult<AppUser> errorResult:
                return Problem(errorResult.Message);
            case SuccessResult<AppUser> successResult:
            {
                var user = successResult.Data;
                var creditResult = await _creditService.GetCreditBalance(user.Id);
                if (creditResult is ErrorResult<int> er) return Problem(er.Message);
                return Ok(creditResult.Data);
            }
        }
        
        return Problem("An unknown error occurred");
    }
}