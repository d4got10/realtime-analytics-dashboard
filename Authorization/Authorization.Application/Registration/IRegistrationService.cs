namespace Authorization.Application;

public interface IRegistrationService
{
    Task<TokenPair> RegisterAsync(UserCredentials credentials);
}