using Authorization.Application.Credentials;
using Authorization.Application.Tokens;
using ErrorOr;

namespace Authorization.Application.Registration;

public interface IRegistrationService
{
    Task<ErrorOr<TokenPair>> RegisterAsync(UserCredentials credentials, CancellationToken ct);
}