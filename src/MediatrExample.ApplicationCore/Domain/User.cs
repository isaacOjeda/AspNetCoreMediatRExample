using Microsoft.AspNetCore.Identity;

namespace MediatrExample.ApplicationCore.Domain;
public class User : IdentityUser
{
    public ICollection<AccessToken> AccessTokens { get; set; } =
        new HashSet<AccessToken>();
}
