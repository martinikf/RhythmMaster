using Application.DTOs.Persons;
using FluentValidation;

namespace API.Validators.Purchase;

public class PurchaseSongValidator : AbstractValidator<PurchaseSongDto>
{
    public PurchaseSongValidator()
    {
        RuleFor(p => p.SongId)
            .NotNull();
        
        RuleFor(p => p.UserId)
            .NotNull();
    }
}