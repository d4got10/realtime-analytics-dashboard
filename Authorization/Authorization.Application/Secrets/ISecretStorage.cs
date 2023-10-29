namespace Authorization.Application.Secrets;

public interface ISecretStorage
{
    string SecretKey { get; }
}