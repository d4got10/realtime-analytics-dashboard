using Authorization.Application.Tokens.Common;
using Authorization.Domain;

namespace Authorization.Application.Tokens.Factories;

public interface ITokenPairFactory
{
    TokenPair CreateFor(User user);
}