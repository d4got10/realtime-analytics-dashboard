namespace Authorization.Application;

public interface ITokenFactory
{
    string Create(params (string Key, string Value)[] payload);
}