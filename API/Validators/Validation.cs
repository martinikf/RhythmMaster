using FluentValidation;
using FluentValidation.Results;

namespace API.Validators;

public static class Validation
{
    public static async Task<ValidationResult> Validate<TValidator, TData>(TData data) where TValidator : IValidator<TData>, new()
    {
        var validator = new TValidator();
        var validationResult = await validator.ValidateAsync(data);
        return validationResult;
    }
    
    public static string GetErrors(ValidationResult validationResult)
    {
        return string.Join(" ", validationResult.Errors.Select(x => x.ErrorMessage));
    }
}