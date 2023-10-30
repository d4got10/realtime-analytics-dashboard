using Authorization.Application.Tokens.Common;
using Authorization.Application.Tokens.Services;
using Authorization.Requests;
using Authorization.Responses;
using ErrorOr;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Authorization.Endpoints;

using ResponseType = Results<Ok<ValidateTokenResponse>, BadRequest<BadRequestPayload>>;

public class ValidateTokenEndpoint : Endpoint<ValidateTokenRequest, ResponseType>
{
    public ValidateTokenEndpoint(ILogger<ValidateTokenEndpoint> logger, ITokenService tokenService)
    {
        _logger = logger;
        _tokenService = tokenService;
    }
    
    private readonly ILogger<ValidateTokenEndpoint> _logger;
    private readonly ITokenService _tokenService;

    public override void Configure()
    {
        Post("/api/validate");
        AllowAnonymous();
    }

    public override async Task<ResponseType> ExecuteAsync(ValidateTokenRequest request, CancellationToken ct)
    {
        ErrorOr<TokenData> errorOrTokenData = await _tokenService.ValidateTokenAsync(request.Token);
        if (errorOrTokenData.IsError)
        {
            return TypedResults.Ok(new ValidateTokenResponse
            {
                IsValid = false,
                Errors = errorOrTokenData.Errors.Select(error => new KeyValuePair<string, string>(error.Code, error.Description))
            });
        }

        return TypedResults.Ok(new ValidateTokenResponse
        {
            IsValid = true,
            Data = errorOrTokenData.Value.Data
        });
    }
}