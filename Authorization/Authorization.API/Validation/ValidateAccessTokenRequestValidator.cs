using Authorization.Requests;
using FastEndpoints;
using FluentValidation;

namespace Authorization.Validation;

public class ValidateAccessTokenRequestValidator : Validator<ValidateAccessTokenRequest>
{
    public ValidateAccessTokenRequestValidator()
    {
        RuleFor(x => x.AccessToken)
            .NotEmpty()
            .WithMessage("Access token is required");
    }
}