namespace Authorization.Application;

public record JwtTokenSettings(TimeSpan Duration, string Issuer, string Audience);