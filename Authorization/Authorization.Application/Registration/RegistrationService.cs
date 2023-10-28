using Authorization.Domain;

namespace Authorization.Application;

public class RegistrationService : IRegistrationService
{
    public RegistrationService(
        IAccessTokenFactory accessTokenFactory, 
        IRefreshTokenFactory refreshTokenFactory, 
        IUserRepository userRepository,
        IHashService hashService)
    {
        _accessTokenFactory = accessTokenFactory;
        _refreshTokenFactory = refreshTokenFactory;
        _userRepository = userRepository;
        _hashService = hashService;
    }
    
    private readonly IAccessTokenFactory _accessTokenFactory;
    private readonly IRefreshTokenFactory _refreshTokenFactory;
    private readonly IUserRepository _userRepository;
    private readonly IHashService _hashService;
    
    public async Task<TokenPair> RegisterAsync(UserCredentials credentials)
    {
        string hashedPassword = _hashService.Hash(credentials.Password, out string salt);

        var user = new User
        {
            Username = credentials.Username,
            Password = hashedPassword,
            Salt = salt,
        };
        
        string accessToken = _accessTokenFactory.Create(("user-id", user.Id.ToString()));
        string refreshToken = _refreshTokenFactory.Create(("user-id", user.Id.ToString()));

        user.RefreshToken = refreshToken;
        await _userRepository.Add(user);
        
        return new TokenPair(accessToken, refreshToken);
    }
}