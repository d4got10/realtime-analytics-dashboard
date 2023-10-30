using Authorization.Application.Tokens.Common;
using ErrorOr;

namespace Authorization.Application.Tokens.Services;

public interface ITokenService
{
    Task<ErrorOr<TokenData>> ValidateTokenAsync(string token);
    Task<ErrorOr<TokenPair>> RefreshTokensAsync(string refreshToken, CancellationToken ct);
}