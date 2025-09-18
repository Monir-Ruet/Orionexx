namespace Orionexx.Identity.Application.Contracts.Account;

public class User
{
    public required string Id { get; set; }
    public string? FullName { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Image { get; set; }
}