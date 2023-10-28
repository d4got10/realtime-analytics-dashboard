using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Authorization.Application;

public class JwtTokenFactory : IAccessTokenFactory, IRefreshTokenFactory
{
    public JwtTokenFactory(IClock clock, ISecretStorage secretStorage, JwtTokenSettings settings)
    {
        _clock = clock;
        _secretStorage = secretStorage;
        _settings = settings;
    }

    private readonly IClock _clock;
    private readonly ISecretStorage _secretStorage;
    private readonly JwtTokenSettings _settings;

    public string Create(params (string Key, string Value)[] payload)
    {
        List<Claim> claims = payload.Select(pair => new Claim(pair.Key, pair.Value)).ToList();
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secretStorage.SecretKey));
        var token = new JwtSecurityToken(
            claims: claims,
            expires: _clock.Now + _settings.Duration,
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}