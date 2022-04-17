namespace MediatrExample.ApplicationCore.Domain;

public class AccessToken
{
    public int AccessTokenId { get; set; }
    public string AccessTokenValue { get; set; }
    public bool Active { get; set; }
    public DateTime Expiration { get; set; }
    public bool Used { get; set; }
    public User User { get; set; }
    public string UserId { get; set; }
}