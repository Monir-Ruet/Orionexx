using FluentValidation;
using Orionexx.Web.DTOs.Identity;

namespace Orionexx.Web.Validators.Identity.Auth;

public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestDtoValidator()
    {
        RuleFor(m => m.Email)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Email required")
            .Must(ValidatorsHelpers.IsValidEmail).WithMessage("Invalid email format");
        RuleFor(m => m.Password)
            .NotNull().WithMessage("Password is required");
    }
}