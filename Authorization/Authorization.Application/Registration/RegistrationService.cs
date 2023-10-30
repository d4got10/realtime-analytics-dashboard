using Authorization.Application.Credentials;
using Authorization.Application.Hashing;
using Authorization.Application.Repositories;
using Authorization.Application.Tokens;
using Authorization.Application.Tokens.Common;
using Authorization.Application.Tokens.Factories;
using Authorization.Application.UnitsOfWork;
using Authorization.Domain;
using ErrorOr;

namespace Authorization.Application.Registration;

public class RegistrationService : IRegistrationService
{
    public RegistrationService(
        ITokenPairFactory tokenPairFactory,
        IUsersUnitOfWork usersUnitOfWork,
        IHashService hashService)
    {
        _tokenPairFactory = tokenPairFactory;
        _usersUnitOfWork = usersUnitOfWork;
        _hashService = hashService;
    }

    private readonly ITokenPairFactory _tokenPairFactory;
    private readonly IUsersUnitOfWork _usersUnitOfWork;
    private readonly IHashService _hashService;
    
    public async Task<ErrorOr<TokenPair>> RegisterAsync(UserCredentials credentials, CancellationToken ct)
    {
        User? existingUser = await _usersUnitOfWork.Users.FindByUsernameAsyncNoTracking(credentials.Username, ct);
        if (existingUser != null)
            return Error.Conflict(description: "User with such username is already registered");

        string hashedPassword = _hashService.Hash(credentials.Password, out string salt);

        var user = new User
        {
            Username = credentials.Username,
            Password = hashedPassword,
            Salt = salt,
        };
        TokenPair tokenPair = _tokenPairFactory.CreateFor(user);

        user.RefreshToken = tokenPair.RefreshToken;
        
        await _usersUnitOfWork.Users.AddAsync(user, ct);
        await _usersUnitOfWork.SaveAsync(ct);
        
        return tokenPair;
    }
}