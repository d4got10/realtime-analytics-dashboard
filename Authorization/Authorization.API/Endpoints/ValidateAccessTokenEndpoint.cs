using Authorization.Application.Tokens;
using Authorization.Requests;
using Authorization.Responses;
using ErrorOr;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Authorization.Endpoints;

using ResponseType = Results<Ok<ValidateAccessTokenResponse>, BadRequest<BadRequestPayload>>;

public class ValidateAccessTokenEndpoint : Endpoint<ValidateAccessTokenRequest, ResponseType>
{
    public ValidateAccessTokenEndpoint(ILogger<ValidateAccessTokenEndpoint> logger, ITokenValidator tokenValidator)
    {
        _logger = logger;
        _tokenValidator = tokenValidator;
    }
    
    private readonly ILogger<ValidateAccessTokenEndpoint> _logger;
    private readonly ITokenValidator _tokenValidator;

    public override void Configure()
    {
        Post("/api/validate");
        AllowAnonymous();
    }

    public override async Task<ResponseType> ExecuteAsync(ValidateAccessTokenRequest request, CancellationToken ct)
    {
        ErrorOr<TokenData> errorOrTokenData = await _tokenValidator.ValidateAsync(request.AccessToken);
        if (errorOrTokenData.IsError)
        {
            return TypedResults.Ok(new ValidateAccessTokenResponse
            {
                AccessTokenIsValid = false
            });
        }

        return TypedResults.Ok(new ValidateAccessTokenResponse
        {
            AccessTokenIsValid = true,
            Data = errorOrTokenData.Value.Data
        });
    }
}