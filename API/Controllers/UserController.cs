using API.Validators;
using API.Validators.User;
using Application.DTOs.Persons;
using Application.Interfaces.Services;
using Domain.Common;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("users")]
[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;
    
    [Authorize(Roles=$"{nameof(Role.Management)},{nameof(Role.Administrator)}")]
    [HttpPost("changeRole")]
    public async Task<IActionResult> ChangeRole([FromBody] ChangeRoleDto data)
    {
        var validationResult = await Validation.Validate<ChangeRoleValidator, ChangeRoleDto>(data);
        
        var result = validationResult.IsValid 
            ? await _userService.ChangeRole(data)
            : new ValidationError(Validation.GetErrors(validationResult));
        
        return result switch
        {
            SuccessResult => Ok(),
            ValidationError err => ValidationProblem(err.Message),
            NotFoundError err => NotFound(err.Message),
            ErrorResult err => BadRequest(err.Message),
            _ => Problem("An unknown error occurred")
        };
    }

    [Authorize(Roles=$"{nameof(Role.Management)},{nameof(Role.Administrator)}")]
    [HttpPost("createInterpret")]
    public async Task<IActionResult> CreateInterpret([FromBody] RegisterInterpretDto data)
    {
        var validationResult = await Validation.Validate<CreateInterpretValidator, RegisterInterpretDto>(data);
        var result = validationResult.IsValid ?
            await _userService.CreateInterpret(data):
            new ValidationError(Validation.GetErrors(validationResult));
        
        return result switch
        {
            SuccessResult => Ok(),
            ValidationError err => ValidationProblem(err.Message),
            ErrorResult errorResult => BadRequest(errorResult.Message),
            _ => Problem("An unknown error occurred")
        };
    }
}