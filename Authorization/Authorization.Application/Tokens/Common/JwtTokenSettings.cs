namespace Authorization.Application.Tokens.Common;

public record JwtTokenSettings(TimeSpan Duration, string Issuer, string Audience);