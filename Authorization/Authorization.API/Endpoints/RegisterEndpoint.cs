using Authorization.Application;
using Authorization.Requests;
using Authorization.Responses;
using FastEndpoints;

namespace Authorization.Endpoints;

public class RegisterEndpoint : Endpoint<Credentials, Tokens>
{
    public RegisterEndpoint(ILogger<RegisterEndpoint> logger, IRegistrationService registrationService)
    {
        _registrationService = registrationService;
        _logger = logger;
    }
    
    private readonly IRegistrationService _registrationService;
    private readonly ILogger<RegisterEndpoint> _logger;

    public override void Configure()
    {
        Post("/api/register");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Credentials credentials, CancellationToken ct)
    {
        TokenPair tokenPair = await _registrationService.RegisterAsync(new UserCredentials(credentials.Username, credentials.Password));
        //TODO: handle registration failure
     
        _logger.LogInformation("Registered {username} with password:\"{password}\"!", credentials.Username, credentials.Password);
        
        await SendAsync(new Tokens
        {
            AccessToken = tokenPair.AccessToken,
            RefreshToken = tokenPair.RefreshToken
        }, cancellation: ct);
    }
}