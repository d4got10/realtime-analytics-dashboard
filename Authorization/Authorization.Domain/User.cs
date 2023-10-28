namespace Authorization.Domain;

public class User : DbModel
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
    public string RefreshToken { get; set; }
}