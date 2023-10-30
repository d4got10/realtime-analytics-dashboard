using Authorization.Requests;
using FastEndpoints;
using FluentValidation;

namespace Authorization.Validation;

public class ValidateAccessTokenRequestValidator : Validator<ValidateTokenRequest>
{
    public ValidateAccessTokenRequestValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty()
            .WithMessage("Access token is required");
    }
}