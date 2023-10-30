namespace Authorization.Application.Tokens.Common;

public readonly struct TokenData
{
    public IEnumerable<KeyValuePair<string, string>> Data { get; init; }
}