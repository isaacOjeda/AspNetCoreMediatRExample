using Microsoft.AspNetCore.Identity;

namespace MediatrExample.ApplicationCore.Domain;
public class User : IdentityUser
{
    public ICollection<RefreshToken> AccessTokens { get; set; } =
        new HashSet<RefreshToken>();
}
