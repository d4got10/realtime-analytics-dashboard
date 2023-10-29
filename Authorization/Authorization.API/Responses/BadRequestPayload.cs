namespace Authorization.Responses;

public class BadRequestPayload
{
    public IEnumerable<string> Errors { get; set; }
}