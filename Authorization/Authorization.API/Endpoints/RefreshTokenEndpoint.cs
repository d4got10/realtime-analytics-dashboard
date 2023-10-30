using Authorization.Application.Tokens;
using Authorization.Application.Tokens.Common;
using Authorization.Application.Tokens.Services;
using Authorization.Requests;
using Authorization.Responses;
using ErrorOr;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Authorization.Endpoints;

using ResponseType = Results<Ok<Tokens>, BadRequest<BadRequestPayload>>;

public class RefreshTokenEndpoint : Endpoint<RefreshTokenRequest, ResponseType>
{
    public RefreshTokenEndpoint(ILogger<RefreshTokenEndpoint> logger, ITokenService tokenService)
    {
        _logger = logger;
        _tokenService = tokenService;
    }

    private readonly ILogger<RefreshTokenEndpoint> _logger;
    private readonly ITokenService _tokenService;

    public override void Configure()
    {
        Post("api/refresh");
        AllowAnonymous();
    }

    public override async Task<ResponseType> ExecuteAsync(RefreshTokenRequest request, CancellationToken ct)
    {
        ErrorOr<TokenPair> errorOrTokenPair = await _tokenService.RefreshTokensAsync(request.RefreshToken, ct);
        if (errorOrTokenPair.IsError)
        {
            return TypedResults.BadRequest(new BadRequestPayload
            {
                Errors = errorOrTokenPair.Errors.Select(error => new KeyValuePair<string, string>(error.Code, error.Description))
            });
        }

        TokenPair tokenPair = errorOrTokenPair.Value;
        
        return TypedResults.Ok(new Tokens
        {
            AccessToken = tokenPair.AccessToken,
            RefreshToken = tokenPair.RefreshToken
        });
    }
}