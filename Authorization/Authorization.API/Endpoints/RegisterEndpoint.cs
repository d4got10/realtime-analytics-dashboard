using Authorization.Application.Credentials;
using Authorization.Application.Registration;
using Authorization.Application.Tokens;
using Authorization.Application.Tokens.Common;
using Authorization.Requests;
using Authorization.Responses;
using ErrorOr;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Authorization.Endpoints;

public class RegisterEndpoint : Endpoint<Credentials, Results<Ok<Tokens>, BadRequest<BadRequestPayload>>>
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

    public override async Task<Results<Ok<Tokens>, BadRequest<BadRequestPayload>>> ExecuteAsync(Credentials credentials, CancellationToken ct)
    {
        var userCredentials = new UserCredentials(credentials.Username, credentials.Password);
        
        ErrorOr<TokenPair> errorOrTokenPair = await _registrationService.RegisterAsync(userCredentials, ct);
        if (errorOrTokenPair.IsError)
        {
            List<KeyValuePair<string, string>> errors = errorOrTokenPair.Errors
                .Select(error => new KeyValuePair<string, string>(error.Code, error.Description)).ToList();
            
            string errorsMessage = string.Join('\n', errors);
            _logger.LogInformation("Didn't register {username}! Reason: {errorsMessage}", credentials.Username, errorsMessage);
            
            return TypedResults.BadRequest(new BadRequestPayload
            {
                Errors = errors
            });
        }
        
        TokenPair tokenPair = errorOrTokenPair.Value;
        
        _logger.LogInformation("Registered {username}!", credentials.Username);

        return TypedResults.Ok(new Tokens
        {
            AccessToken = tokenPair.AccessToken,
            RefreshToken = tokenPair.RefreshToken
        });
    }
}