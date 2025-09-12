namespace Orionexx.Web.DTOs.Account;

public class UserResponseDto
{
    public required string Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? PImage { get; set; }
}