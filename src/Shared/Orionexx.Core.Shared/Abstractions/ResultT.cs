namespace Orionexx.Core.Shared.Abstractions;

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(bool isSuccess, string? message = null, TValue? value = default, IEnumerable<Error>? error = null)
        : base(isSuccess, message, error) =>
        _value = value;

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");
}
