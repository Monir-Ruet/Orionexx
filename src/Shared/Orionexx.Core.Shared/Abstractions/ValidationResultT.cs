namespace Orionexx.Core.Shared.Abstractions;

public sealed class ValidationResult<TValue> : Result<TValue>
{
    private ValidationResult(string message, Error[] errors)
        : base(false, message, default, errors) { }

    public static ValidationResult<TValue> WithErrors(Error[] errors) => new("Validation failed", errors);
}
