namespace Authorization.Responses;

public class ValidateTokenResponse
{
    public bool IsValid { get; set; }
    public IEnumerable<KeyValuePair<string, string>>? Data { get; set; }
    public IEnumerable<KeyValuePair<string, string>>? Errors { get; set; }
}