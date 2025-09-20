using System.Text.RegularExpressions;

namespace Orionexx.Web.Validators;

public static partial class ValidatorsHelpers
{
    public static bool IsValidEmail(string? email)
    {
        return !string.IsNullOrEmpty(email) && EmailRegex().IsMatch(email);
    }

    [GeneratedRegex(@"^[^\s@]+@[^\s@]+\.[^\s@]+$")]
    private static partial Regex EmailRegex();
}