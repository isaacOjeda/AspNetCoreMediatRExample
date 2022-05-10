using Microsoft.AspNetCore.Identity;

namespace MediatrExample.ApplicationCore.Domain;
public class User : IdentityUser
{
    public ICollection<RefreshToken> AccessTokens { get; set; } =
        new HashSet<RefreshToken>();

    public ICollection<Checkout> Checkouts { get; set; } =
        new HashSet<Checkout>();
}
