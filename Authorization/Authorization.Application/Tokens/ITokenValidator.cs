using ErrorOr;

namespace Authorization.Application.Tokens;

public interface ITokenValidator
{
    Task<ErrorOr<TokenData>> ValidateAsync(string token);
}