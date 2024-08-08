using Application.DTOs;
using Application.DTOs.Persons;
using FluentValidation;

namespace API.Validators.User;

public class CreateInterpretValidator : AbstractValidator<RegisterInterpretDto>
{
    public CreateInterpretValidator()
    {
        RuleFor(p => p.FirstName)
            .NotNull()
            .Length(2, 64);
        
        RuleFor(p => p.LastName)
            .NotNull()
            .Length(2, 64);

        RuleFor(p => p.CustomName)
            .MaximumLength(128);

        RuleFor(p => p.SocialLink)
            .MaximumLength(1024);
    }
}