using System.Data;
using Application.DTOs;
using Application.DTOs.Auth;
using FluentValidation;

namespace API.Validators.Auth;

public class LoginValidator : AbstractValidator<LoginCredentialsDto>
{
    public LoginValidator()
    {
        RuleFor(p=>p.Email)
            .NotNull()
            .EmailAddress();

        RuleFor(p => p.Password)
            .NotNull();
    }
}