using System.Text.Json.Serialization;

namespace Orionexx.Messaging.Dtos;

public class AccountCreated
{
    [JsonPropertyName("email")]
    public required string Email { get; set; }
}
