using System.Text.RegularExpressions;
using FluentValidation;
using Orionexx.Web.Contracts.Identity;

namespace Orionexx.Web.Validators.Identity;

public partial class SignupContractValidator : AbstractValidator<SignupContract>
{
    public SignupContractValidator()
    {
        RuleFor(m => m.Email)
            .NotNull()
            .Must(IsValidEmail)
            .WithMessage("Email is invalid.");
        RuleFor(m => m.Password).NotEmpty().WithMessage("Password is required.");
    }

    private bool IsValidEmail(string? email)
    {
        if (string.IsNullOrEmpty(email)) return false;
        return MyRegex().IsMatch(email);
    }

    [GeneratedRegex(@"^[^\s@]+@[^\s@]+\.[^\s@]+$")]
    private static partial Regex MyRegex();
}