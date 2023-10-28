using System.Text.RegularExpressions;
using Authorization.Requests;
using FastEndpoints;
using FluentValidation;

namespace Authorization.Validation;

public partial class CredentialsValidator : Validator<Credentials>
{
    [GeneratedRegex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{0,}$")]
    private static partial Regex PasswordRegex();

    [GeneratedRegex("^[A-Za-z0-9]+([A-Za-z0-9]*|[._-]?[A-Za-z0-9]+)*$")]
    private static partial Regex UsernameRegex();

    public CredentialsValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Username is required!")
            .MinimumLength(4)
            .WithMessage("Username too short!")
            .MaximumLength(30)
            .WithMessage("Username too big!")
            .Matches(UsernameRegex())
            .WithMessage("Invalid username!");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required!")
            .MinimumLength(8)
            .WithMessage("Password is too short!")
            .MaximumLength(50)
            .WithMessage("Password is too big!")
            .Matches(PasswordRegex())
            .WithMessage("Password must contain at least one number, one lowercase character, " +
                         "one uppercase character and one special character!");
    }
}