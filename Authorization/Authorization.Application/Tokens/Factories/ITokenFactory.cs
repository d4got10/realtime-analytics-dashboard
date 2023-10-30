namespace Authorization.Application.Tokens.Factories;

public interface ITokenFactory
{
    string Create(params (string Key, string Value)[] payload);
}