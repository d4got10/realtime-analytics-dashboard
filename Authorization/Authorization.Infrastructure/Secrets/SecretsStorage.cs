using Authorization.Application;
using Authorization.Application.Secrets;
using Microsoft.Extensions.Configuration;

namespace Authorization.Infrastructure.Secrets;

public class SecretsStorage : ISecretStorage
{
    public SecretsStorage(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string SecretKey => _configuration["Secrets:Key"]!;

    private readonly IConfiguration _configuration;
}