using Authorization.Domain;

namespace Authorization.Application.Tokens;

public class TokenPairFactory : ITokenPairFactory
{
    public TokenPairFactory(IAccessTokenFactory accessTokenFactory, IRefreshTokenFactory refreshTokenFactory)
    {
        _accessTokenFactory = accessTokenFactory;
        _refreshTokenFactory = refreshTokenFactory;
    }

    public const string TokenUserIdClaimKey = "user-id";
    
    private readonly IAccessTokenFactory _accessTokenFactory;
    private readonly IRefreshTokenFactory _refreshTokenFactory;

    public TokenPair CreateFor(User user)
    {
        string accessToken = _accessTokenFactory.Create((TokenUserIdClaimKey, user.Id.ToString()));
        string refreshToken = _refreshTokenFactory.Create((TokenUserIdClaimKey, user.Id.ToString()));
        
        return new TokenPair(accessToken, refreshToken);
    }
}