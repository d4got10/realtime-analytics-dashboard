namespace Authorization.Application.Tokens;

public record JwtTokenSettings(TimeSpan Duration, string Issuer, string Audience);