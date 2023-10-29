using Authorization.Domain;

namespace Authorization.Application.Tokens;

public interface ITokenPairFactory
{
    TokenPair CreateFor(User user);
}