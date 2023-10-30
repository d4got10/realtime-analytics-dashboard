using Authorization.Application.Credentials;
using Authorization.Application.Hashing;
using Authorization.Application.Tokens;
using Authorization.Application.Tokens.Common;
using Authorization.Application.Tokens.Factories;
using Authorization.Application.UnitsOfWork;
using Authorization.Domain;
using ErrorOr;

namespace Authorization.Application.Login;

public class LoginService : ILoginService
{
    public LoginService(
        IUsersUnitOfWork usersUnitOfWork, 
        IHashService hashService, 
        ITokenPairFactory tokenPairFactory)
    {
        _usersUnitOfWork = usersUnitOfWork;
        _hashService = hashService;
        _tokenPairFactory = tokenPairFactory;
    }

    private readonly IUsersUnitOfWork _usersUnitOfWork;
    private readonly IHashService _hashService;
    private readonly ITokenPairFactory _tokenPairFactory;

    public async Task<ErrorOr<TokenPair>> LoginAsync(UserCredentials credentials, CancellationToken ct)
    {
        User? user = await _usersUnitOfWork.Users.FindByUsernameAsync(credentials.Username, ct);
        if (user == null)
            return Error.NotFound(description: "User with such username was not found");

        if (user.Password != _hashService.Hash(credentials.Password, user.Salt))
            return Error.Failure(description: "Invalid password");

        TokenPair tokenPair = _tokenPairFactory.CreateFor(user);

        user.RefreshToken = tokenPair.RefreshToken;

        await _usersUnitOfWork.SaveAsync(ct);

        return tokenPair;
    }
}