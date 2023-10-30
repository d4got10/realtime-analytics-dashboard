using Authorization.Application.Tokens.Common;
using ErrorOr;

namespace Authorization.Application.Tokens.Validators;

public interface ITokenValidator
{
    Task<ErrorOr<TokenData>> ValidateAsync(string token);
}