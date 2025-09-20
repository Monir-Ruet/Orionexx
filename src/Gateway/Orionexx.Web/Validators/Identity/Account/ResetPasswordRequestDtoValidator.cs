using FluentValidation;
using Orionexx.Web.DTOs.Account;

namespace Orionexx.Web.Validators.Identity.Account;

public class ResetPasswordRequestDtoValidator : AbstractValidator<ResetPasswordRequestDto>
{
    public ResetPasswordRequestDtoValidator()
    {
        RuleFor(m => m.Email)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Email required")
            .Must(ValidatorsHelpers.IsValidEmail).WithMessage("Invalid email format");
        RuleFor(m => m.NewPassword)
            .NotNull().WithMessage("Password required");
        RuleFor(m => m.ResetCode)
            .NotNull().WithMessage("Reset code required");
    }
}