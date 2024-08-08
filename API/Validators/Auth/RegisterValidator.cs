using Application.DTOs;
using Application.DTOs.Persons;
using FluentValidation;

namespace API.Validators.Auth;

public class RegisterValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterValidator()
    {
        RuleFor(p=>p.Email)
            .NotNull()
            .EmailAddress();

        RuleFor(p => p.Password)
            .NotNull()
            .MinimumLength(8)
            .MaximumLength(512);

        RuleFor(p => p.FirstName)
            .NotNull()
            .Length(2, 64);

        RuleFor(p => p.LastName)
            .NotNull()
            .Length(2, 64);
        
        RuleFor(i => i.CustomName)
            .MaximumLength(128);

        RuleFor(i => i.SocialLink)
            .MaximumLength(1024);
    }
}