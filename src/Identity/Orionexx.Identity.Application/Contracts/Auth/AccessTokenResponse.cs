namespace Orionexx.Identity.Application.Contracts.Auth;

public class AccessTokenResponse
{
    public required string AccessToken { get; set; }
    public int ExpiresIn { get; set; }
    public required string TokenType { get; set; }
    public required string RefreshToken { get; set; }
}