using System.Collections;

namespace Orionexx.Core.Shared.Abstractions;

public sealed class Error(string code, string message) : IEquatable<Error>, IEnumerable
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.");

    private string Code { get; } = code;
    private string Message { get; } = message;

    // Implicit operator to convert Error to string (returns the Code)
    public static implicit operator string(Error error) => error.Code;

    // Equality operators
    public static bool operator ==(Error? a, Error? b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Error? a, Error? b) => !(a == b);

    // Equals method for comparison
    public bool Equals(Error? other)
    {
        if (other is null)
            return false;

        return Code == other.Code && Message == other.Message;
    }

    public override bool Equals(object? obj) => obj is Error error && Equals(error);

    // GetHashCode implementation
    public override int GetHashCode() => HashCode.Combine(Code, Message);

    // ToString override to return Code
    public override string ToString() => Code;

    // GetEnumerator implementation to iterate over Code and Message
    public IEnumerator GetEnumerator()
    {
        // Return an enumerator for Code and Message
        return new ErrorEnumerator(Code, Message);
    }

    // Enumerator for iterating over Code and Message
    private class ErrorEnumerator(string code, string message) : IEnumerator
    {
        private readonly string[] _values = [code, message]; // Store Code and Message in an array
        private int _index = -1;

        public bool MoveNext()
        {
            _index++;
            return _index < _values.Length;
        }

        public void Reset() => _index = -1;

        public object Current
        {
            get
            {
                if (_index == -1 || _index >= _values.Length)
                {
                    throw new InvalidOperationException("Enumeration is not in a valid state.");
                }
                return _values[_index];
            }
        }
    }
}
