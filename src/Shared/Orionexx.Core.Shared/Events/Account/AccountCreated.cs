using Orionexx.Core.Shared.Primitives;

namespace Orionexx.Core.Shared.Events.Account;

public class AccountCreated(string email, string lastName, string confirmationUrl) : BaseEvent
{
    public string LastName { get; } = lastName;
    public string Email { get; } = email;
    public string ConfirmationUrl { get; } = confirmationUrl;
}
