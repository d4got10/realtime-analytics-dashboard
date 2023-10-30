﻿using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Authorization.Application.Secrets;
using Authorization.Application.Time;
using ErrorOr;
using Microsoft.IdentityModel.Tokens;

namespace Authorization.Application.Tokens;

public class JwtTokenValidator : ITokenValidator
{
    public JwtTokenValidator(IClock clock, ISecretStorage secretStorage, JwtTokenSettings settings)
    {
        _clock = clock;
        _secretStorage = secretStorage;
        _settings = settings;
    }

    private readonly IClock _clock;
    private readonly ISecretStorage _secretStorage;
    private readonly JwtTokenSettings _settings;

    public async Task<ErrorOr<TokenData>> ValidateAsync(string token)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secretStorage.SecretKey));
        var tokenHandler = new JwtSecurityTokenHandler();
        
        TokenValidationResult? validationResult = await tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = securityKey,
            ValidateIssuer = true,
            ValidIssuer = _settings.Issuer,
            ValidateAudience = true,
            ValidAudience = _settings.Audience,
            ValidateLifetime = true,
            LifetimeValidator = ValidateLifetime,
            ClockSkew = TimeSpan.Zero
        });
        
        if (!validationResult.IsValid) 
            return Error.Validation(description: "Token is invalid");
        
        return new TokenData
        {
            Data = validationResult.Claims.Select(e => new KeyValuePair<string, string>(e.Key, e.Value.ToString()!))
        };
    }

    private bool ValidateLifetime(
        DateTime? notbefore, 
        DateTime? expires, 
        SecurityToken securitytoken, 
        TokenValidationParameters validationparameters)
    {
        return expires >= _clock.UtcNow;
    }
}