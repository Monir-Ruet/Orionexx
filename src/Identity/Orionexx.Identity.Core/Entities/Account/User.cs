using Microsoft.AspNetCore.Identity;

namespace Orionexx.Identity.Core.Entities.Account;

public class AppUser : IdentityUser
{
    public string? FullName { get; set; }
}