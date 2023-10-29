namespace Authorization.Application.Tokens;

public interface ITokenFactory
{
    string Create(params (string Key, string Value)[] payload);
}