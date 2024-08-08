using API.Validators;
using API.Validators.Auth;
using Application.DTOs.Auth;
using Application.DTOs.Persons;
using Application.Interfaces.Services;
using Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("auth")]
[ApiController]
public class AuthController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCredentialsDto data)
    {
        var validationResult = await Validation.Validate<LoginValidator, LoginCredentialsDto>(data);
        var result = validationResult.IsValid ? 
            await _userService.Login(data): 
            new ValidationError<string>(Validation.GetErrors(validationResult));

        return result switch
        {
            SuccessResult<string> => Ok(result.Data),
            ValidationError<string> err => ValidationProblem(err.Message),
            NotFoundError<string> err => NotFound(err.Message),
            ErrorResult<string> err => Problem(err.Message),
            _ => Problem("An unknown error occurred")
        };
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto data)
    {
        var validationResult = await Validation.Validate<RegisterValidator, RegisterUserDto>(data);
       
        var result = validationResult.IsValid ? 
            await _userService.Register(data): 
            new ValidationError(Validation.GetErrors(validationResult));

        return result switch
        {
            SuccessResult => Ok(),
            ValidationError<string> err => ValidationProblem(err.Message),
            ErrorResult errorResult => Problem(errorResult.Message),
            _ => Problem("An unknown error occurred")
        };
    }
}