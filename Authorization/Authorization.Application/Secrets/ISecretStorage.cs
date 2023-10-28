namespace Authorization.Application;

public interface ISecretStorage
{
    string SecretKey { get; }
}