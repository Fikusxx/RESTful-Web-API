using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        var hasher = new PasswordHasher<ApplicationUser>();

        builder.HasData(
             new ApplicationUser()
             {
                 Id = "8e445865-a24d-4543-a6c6-9443d048cdb9",
                 Email = "admin@host.com",
                 NormalizedEmail = "ADMIN@HOST.COM",
                 FirstName = "System",
                 LastName = "Admin",
                 UserName = "admin@host.com",
                 NormalizedUserName = "ADMIN@HOST.COM",
                 PasswordHash = hasher.HashPassword(null, "P@ssword1"),
                 EmailConfirmed = true
             },
             new ApplicationUser()
             {
                 Id = "9e224968-33e4-4652-b7b7-8574d048cdb9",
                 Email = "user@host.com",
                 NormalizedEmail = "USER@HOST.COM",
                 FirstName = "System",
                 LastName = "User",
                 UserName = "user@host.com",
                 NormalizedUserName = "USER@HOST.COM",
                 PasswordHash = hasher.HashPassword(null, "P@ssword1"),
                 EmailConfirmed = true
             });
    }
}
