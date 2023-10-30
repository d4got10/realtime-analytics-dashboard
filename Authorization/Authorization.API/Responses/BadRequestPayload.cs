namespace Authorization.Responses;

public class BadRequestPayload
{
    public IEnumerable<KeyValuePair<string, string>> Errors { get; set; }
}