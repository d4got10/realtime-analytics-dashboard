using Authorization.Application.Tokens.Common;
using Authorization.Application.Tokens.Extensions;
using Authorization.Domain;

namespace Authorization.Application.Tokens.Factories;

public class TokenPairFactory : ITokenPairFactory
{
    public TokenPairFactory(IAccessTokenFactory accessTokenFactory, IRefreshTokenFactory refreshTokenFactory)
    {
        _accessTokenFactory = accessTokenFactory;
        _refreshTokenFactory = refreshTokenFactory;
    }

    private readonly IAccessTokenFactory _accessTokenFactory;
    private readonly IRefreshTokenFactory _refreshTokenFactory;

    public TokenPair CreateFor(User user)
    {
        string accessToken = _accessTokenFactory.Create((ClaimType.UserId.ToClaimString(), user.Id.ToString()));
        string refreshToken = _refreshTokenFactory.Create((ClaimType.UserId.ToClaimString(), user.Id.ToString()));
        
        return new TokenPair(accessToken, refreshToken);
    }
}