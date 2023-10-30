using Authorization.Application.Tokens.Common;
using Authorization.Application.Tokens.Extensions;
using Authorization.Application.Tokens.Factories;
using Authorization.Application.Tokens.Validators;
using Authorization.Application.UnitsOfWork;
using Authorization.Domain;
using ErrorOr;

namespace Authorization.Application.Tokens.Services;

public class TokenService : ITokenService
{
    public TokenService(ITokenValidator tokenValidator, ITokenPairFactory tokenPairFactory, IUsersUnitOfWork usersUnitOfWork)
    {
        _tokenValidator = tokenValidator;
        _tokenPairFactory = tokenPairFactory;
        _usersUnitOfWork = usersUnitOfWork;
    }

    private readonly ITokenValidator _tokenValidator;
    private readonly ITokenPairFactory _tokenPairFactory;
    private readonly IUsersUnitOfWork _usersUnitOfWork;

    public async Task<ErrorOr<TokenData>> ValidateTokenAsync(string token)
    {
        return await _tokenValidator.ValidateAsync(token);
    }

    public async Task<ErrorOr<TokenPair>> RefreshTokensAsync(string refreshToken, CancellationToken ct)
    {
        ErrorOr<TokenData> errorOrTokenData = await _tokenValidator.ValidateAsync(refreshToken);
        if (errorOrTokenData.IsError)
        {
            return ErrorOr<TokenPair>.From(errorOrTokenData.Errors);
        }

        TokenData tokenData = errorOrTokenData.Value;
        KeyValuePair<string, string>? claimPair = tokenData.Data.FirstOrDefault(pair => pair.Key == ClaimType.UserId.ToClaimString());
        if (!claimPair.HasValue)
            return Error.Validation(description: "Refresh token had no claim with user's id");

        string userIdRaw = claimPair.Value.Value;
        if (!Guid.TryParse(userIdRaw, out Guid userId))
            return Error.Validation(description: "User's id was not a valid guid");
        
        User? user = await _usersUnitOfWork.Users.FindByIdAsync(userId, ct);
        if (user == null)
            return Error.NotFound(description: "User with such id was not found");

        if (user.RefreshToken != refreshToken)
            return Error.Conflict(description: "User's refresh token doesn't match received refresh token");
        
        TokenPair tokenPair = _tokenPairFactory.CreateFor(user);
        user.RefreshToken = tokenPair.RefreshToken;
        await _usersUnitOfWork.SaveAsync(ct);

        return tokenPair;
    }
}