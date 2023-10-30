namespace Authorization.Application.Tokens;

public readonly struct TokenData
{
    public IEnumerable<KeyValuePair<string, string>> Data { get; init; }
}