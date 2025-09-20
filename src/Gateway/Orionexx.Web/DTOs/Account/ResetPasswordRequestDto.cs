namespace Orionexx.Web.DTOs.Account;

public class ResetPasswordRequestDto
{
    public required string Email { get; set; }
    public required string ResetCode { get; set; }
    public required string NewPassword { get; set; }
}