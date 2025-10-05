using Orionexx.Core.Shared.Primitives;

namespace Orionexx.Identity.Core.Events.Account;

public class AccountCreateEvent(string email) : BaseEvent
{
    public string Email { get; } = email;
}
