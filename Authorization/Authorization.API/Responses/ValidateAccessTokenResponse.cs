namespace Authorization.Responses;

public class ValidateAccessTokenResponse
{
    public bool AccessTokenIsValid { get; set; }
    public IEnumerable<KeyValuePair<string, string>>? Data { get; set; }
}