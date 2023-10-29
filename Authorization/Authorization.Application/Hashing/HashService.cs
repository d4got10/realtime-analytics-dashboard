using System.Security.Cryptography;
using System.Text;

namespace Authorization.Application.Hashing;

public class HashService : IHashService
{
    private const int KeySize = 64;
    private const int Iterations = 100_000;
    private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA512;
    
    public string Hash(string value, out string salt)
    {
        byte[] saltBytes = RandomNumberGenerator.GetBytes(KeySize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(value),
            saltBytes,
            Iterations,
            HashAlgorithm,
            KeySize);
        salt = Convert.ToHexString(saltBytes);
        return Convert.ToHexString(hash);
    }

    public string Hash(string value, string salt)
    {
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(value),
            Convert.FromHexString(salt),
            Iterations,
            HashAlgorithm,
            KeySize);
        return Convert.ToHexString(hash);
    }
}