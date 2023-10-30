using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Authorization.Application.Secrets;
using Authorization.Application.Time;
using Authorization.Application.Tokens.Common;
using ErrorOr;
using Microsoft.IdentityModel.Tokens;

namespace Authorization.Application.Tokens.Validators;

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

        bool tokenHasExpired = false;
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
        {
            if (tokenHasExpired)
            {
                return Error.Validation(code: "Token.Validation.Lifetime", description: "Token has expired");
            }

            return Error.Validation(code: "Token.Validation.General", description: "Token is invalid");
        }
        
        return new TokenData
        {
            Data = validationResult.Claims.Select(e => new KeyValuePair<string, string>(e.Key, e.Value.ToString()!))
        };
        
        bool ValidateLifetime(
            DateTime? notBefore, 
            DateTime? expires, 
            SecurityToken securityToken, 
            TokenValidationParameters validationParameters)
        {
            tokenHasExpired = expires < _clock.UtcNow;
            return !tokenHasExpired;
        }
    }
}