namespace Orionexx.Identity.Application.Infrastructure.Configurations;

public interface IAppConfiguration
{
    IConnectionStrings ConnectionStrings { get; }
    IJwtSettings JwtSettings { get; }
}

public interface IConnectionStrings
{
    string Orionexx { get; set; }
    string DatabaseName { get; set; }
}

public interface IJwtSettings
{
    string SecretKey { get; init; }
    string Issuer { get; init; }
    string Audience { get; init; }
    int AccessTokenExpirationMinutes { get; init; }
    int RefreshTokenExpirationDays { get; init; }
}