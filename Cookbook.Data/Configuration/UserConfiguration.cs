using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cookbook.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<IdentityUser>
    {
        public static IdentityUser SeedUser = new IdentityUser
        {
            Id = "4dd95024-058e-44aa-a548-8d28b38a6e41",
            UserName = "test@gmail.com",
            NormalizedUserName = "TEST@GMAIL.COM",
            Email = "test@gmail.com",
            NormalizedEmail = "TEST@GMAIL.COM",
            EmailConfirmed = true,
            PasswordHash = new PasswordHasher<IdentityUser>()
                    .HashPassword(new IdentityUser { UserName = "test@gmail.com" }, "Pass123")
        };

        public void Configure(EntityTypeBuilder<IdentityUser> builder)
        {
            builder.HasData(SeedUser);
        }
    }
}
