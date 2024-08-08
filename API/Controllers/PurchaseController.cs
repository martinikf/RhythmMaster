using API.Validators;
using API.Validators.Purchase;
using Application.DTOs.Persons;
using Application.Interfaces.Services;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("purchase")]
[ApiController]
public class PurchaseController(IPurchaseService purchaseService, IUserService userService) : ControllerBase
{
    private readonly IPurchaseService _purchaseService = purchaseService;
    private readonly IUserService _userService = userService;
    
    [Authorize(Roles = nameof(Role.ExternalDJ))]
    [HttpPost("purchaseSong")]
    public async Task<IActionResult> PurchaseSong([FromBody] SongId songId)
    {
        // Try to get user object
        if (User.Identity?.Name == null) return Unauthorized();
        var user = await _userService.GetUserByEmail(User.Identity.Name);
        if (user is ErrorResult<AppUser> er) return NotFound(er.Message);

        var dto = new PurchaseSongDto(user.Data.Id, songId);

        var validationResult = await Validation.Validate<PurchaseSongValidator, PurchaseSongDto>(dto);
        
        var result = validationResult.IsValid ?
            await _purchaseService.PurchaseSong(dto) :
            new ValidationError(Validation.GetErrors(validationResult));

        return result switch
        {
            SuccessResult => Ok(),
            ValidationError errorResult => ValidationProblem(errorResult.Message),
            ErrorResult errorResult => Problem(errorResult.Message),
            _ => Problem("An unknown error occurred")
        };
    }
}