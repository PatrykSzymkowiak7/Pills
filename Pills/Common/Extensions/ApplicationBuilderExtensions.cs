using Microsoft.AspNetCore.Identity;
using Pills.Identity;

namespace Pills.Common.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        private readonly static string AdminEmail = "admin@pills.app";

        public static async Task SeedAsync(this WebApplication app)
        {
            using(var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                await SeedRoles(roleManager);
                await SeedAdmin(userManager);
            }
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { UserRoles.Admin, UserRoles.User };

            foreach(var role in roles)
            {
                if(!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private static async Task SeedAdmin(UserManager<ApplicationUser> userManager)
        {
            var admin = await userManager.FindByEmailAsync(AdminEmail);
            if (admin != null)
                return;

            admin = new ApplicationUser
            {
                UserName = AdminEmail,
                Email = AdminEmail,
                EmailConfirmed = true
            };

            await userManager.CreateAsync(admin, "Test12345@");
            await userManager.AddToRoleAsync(admin, UserRoles.Admin);
        }
    }
}
