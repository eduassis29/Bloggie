using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Data {
    public class AuthDbContext : IdentityDbContext {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) {



        }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

            //seed Roles (User, Admin, Superadmin)
            var adminRoleId = "9ebb9286-4fd9-4146-a27b-f2233c938b8b";
            var superAdminRoleId = "ecc8050a-8eff-44f7-8d23-78dfd8e0d1b5";
            var userRoleId = "9d56192e-649c-4083-9f00-9133a30132b3";

            var roles = new List<IdentityRole> {
                
                new IdentityRole {
                    Name = "Admin",
                    NormalizedName = "Admin",
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId
                },

                new IdentityRole {
                    Name = "SuperAdmin",
                    NormalizedName = "SuperAdmin",
                    Id = superAdminRoleId,
                    ConcurrencyStamp = superAdminRoleId
                },

                 new IdentityRole {
                     Name = "User",
                     NormalizedName = "User",
                     Id = userRoleId,
                     ConcurrencyStamp= userRoleId
                 }
            };

            builder.Entity<IdentityRole>().HasData(roles);

            //seed SuperAdminUser

            var superAdminId = "2ab7cdfb-5a53-4ac0-a854-0d6ec8e1eb56";

            var superAdminUser = new IdentityUser {
                UserName = "superadmin@bloggie.com",
                Email = "superadmin@bloggie.com",
                NormalizedEmail = "superadmin@bloggie.com".ToUpper(),
                NormalizedUserName = "superadmin@bloggie.com".ToUpper(),
                Id = superAdminId
            };

            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>()
                .HashPassword(superAdminUser, "Superadmin@123");

            builder.Entity<IdentityUser>().HasData(superAdminUser);

            //Add All Roles to SuperAdminUser

            var superAdminRoles = new List<IdentityUserRole<string>> {
                new IdentityUserRole<string> {
                    RoleId = adminRoleId,
                    UserId = superAdminId,
                },

                new IdentityUserRole<string> {
                    RoleId = superAdminRoleId,
                    UserId = superAdminId,
                },

                new IdentityUserRole<string> {
                    RoleId = userRoleId,
                    UserId = superAdminId,
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);

        }

    }
}
