using FluentValidation;
using Orionexx.Web.DTOs.Identity;

namespace Orionexx.Web.Validators.Identity.Account;

public class SignupRequestDtoValidator : AbstractValidator<SignupRequestDto>
{
    public SignupRequestDtoValidator()
    {
        RuleFor(m => m.Email)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Email required")
            .Must(ValidatorsHelpers.IsValidEmail).WithMessage("Invalid email format");
        RuleFor(m => m.Password)
            .NotNull().WithMessage("Password required");
    }
}