namespace Authorization.Application.Hashing;

public interface IHashService
{
    string Hash(string value, out string salt);
    string Hash(string value, string salt);
}