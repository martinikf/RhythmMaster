using Application.DTOs;
using Application.DTOs.Persons;
using FluentValidation;

namespace API.Validators.User;

public class ChangeRoleValidator : AbstractValidator<ChangeRoleDto>
{
    public ChangeRoleValidator()
    {
        RuleFor(p => p.Role)
            .IsInEnum()
            .WithMessage("Invalid role");

        RuleFor(p => p.UserId)
            .NotNull()
            .WithMessage("User ID is required");
    }
}