using Authorization.Application.Tokens.Common;

namespace Authorization.Application.Tokens.Extensions;

public static class ClaimTypeExtensions
{
    public static string ToClaimString(this ClaimType type)
    {
        return type switch
        {
            ClaimType.UserId => "user-id",
            _ => type.ToString()
        };
    }
}