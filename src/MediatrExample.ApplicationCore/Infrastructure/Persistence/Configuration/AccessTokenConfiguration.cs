using MediatrExample.ApplicationCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediatrExample.ApplicationCore.Infrastructure.Persistence.Configuration;
public class AccessTokenConfiguration : IEntityTypeConfiguration<AccessToken>
{
    public void Configure(EntityTypeBuilder<AccessToken> builder)
    {
        builder.HasKey(q => q.AccessTokenId);
        builder.Property(q => q.AccessTokenValue)
            .IsRequired();
        builder.Property(q => q.UserId)
            .IsRequired();


        builder.HasOne(q => q.User)
            .WithMany(q => q.AccessTokens)
            .HasForeignKey(q => q.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
