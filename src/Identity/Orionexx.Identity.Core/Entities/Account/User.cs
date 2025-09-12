using Microsoft.AspNetCore.Identity;

namespace Orionexx.Identity.Core.Entities.Account;

public class AppUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? BirthDay { get; set; }
    public int? BirthMonth { get; set; }
    public int? BirthYear { get; set; }
}