using Authorization.Application.Credentials;
using Authorization.Application.Tokens;
using ErrorOr;

namespace Authorization.Application.Login;

public interface ILoginService
{
    Task<ErrorOr<TokenPair>> LoginAsync(UserCredentials credentials, CancellationToken ct);
}