using Authorization.Application.Credentials;
using Authorization.Application.Login;
using Authorization.Application.Tokens;
using Authorization.Requests;
using Authorization.Responses;
using ErrorOr;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Authorization.Endpoints;

public class LoginEndpoint : Endpoint<Credentials, Results<Ok<Tokens>, BadRequest<BadRequestPayload>>>
{
    public LoginEndpoint(ILogger<LoginEndpoint> logger, ILoginService loginService)
    {
        _loginService = loginService;
        _logger = logger;
    }
    
    private readonly ILoginService _loginService;
    private readonly ILogger<LoginEndpoint> _logger;

    public override void Configure()
    {
        Post("/api/login");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<Tokens>, BadRequest<BadRequestPayload>>> ExecuteAsync(Credentials credentials, CancellationToken ct)
    {
        var userCredentials = new UserCredentials(credentials.Username, credentials.Password);
        
        ErrorOr<TokenPair> errorOrTokenPair = await _loginService.LoginAsync(userCredentials, ct);
        if (errorOrTokenPair.IsError)
        {
            IEnumerable<string> errorDescriptions = errorOrTokenPair.Errors.Select(error => error.Description).ToList();
            
            string errorsMessage = string.Join('\n', errorDescriptions);
            _logger.LogInformation("Didn't log in {username}! Reason: {errorsMessage}", credentials.Username, errorsMessage);
            
            return TypedResults.BadRequest(new BadRequestPayload
            {
                Errors = errorDescriptions
            });
        }
        
        TokenPair tokenPair = errorOrTokenPair.Value;
        
        _logger.LogInformation("User {username} logged in!", credentials.Username);

        return TypedResults.Ok(new Tokens
        {
            AccessToken = tokenPair.AccessToken,
            RefreshToken = tokenPair.RefreshToken
        });
    }
}