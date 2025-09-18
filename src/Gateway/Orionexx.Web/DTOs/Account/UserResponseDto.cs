namespace Orionexx.Web.DTOs.Account;

public class UserResponseDto
{
    public required string Id { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Image { get; set; }
}