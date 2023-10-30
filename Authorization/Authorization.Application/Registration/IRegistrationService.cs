using Authorization.Application.Credentials;
using Authorization.Application.Tokens;
using Authorization.Application.Tokens.Common;
using ErrorOr;

namespace Authorization.Application.Registration;

public interface IRegistrationService
{
    Task<ErrorOr<TokenPair>> RegisterAsync(UserCredentials credentials, CancellationToken ct);
}