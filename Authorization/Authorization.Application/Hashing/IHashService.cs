namespace Authorization.Application;

public interface IHashService
{
    string Hash(string value, out string salt);
}