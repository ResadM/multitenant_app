using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using multitenant_app.Models;

namespace multitenant_app.Context
{
    public class DatabaseContextAdmin : IdentityDbContext<ApplicationUser>
    {
        public DatabaseContextAdmin(DbContextOptions<DatabaseContextAdmin> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Name = EnumUserRoles.Admin.ToString(), NormalizedName = EnumUserRoles.Admin.ToString().ToUpper(), Id = "103c43ae-1b1a-44ce-9b16-f29a0a544104" },
                new IdentityRole { Name = EnumUserRoles.User.ToString(), NormalizedName = EnumUserRoles.User.ToString().ToUpper(), Id = "9853375e-2d89-4d57-981e-4089a5961a13" }
            );
            base.OnModelCreating(builder);
        }
    }
}
