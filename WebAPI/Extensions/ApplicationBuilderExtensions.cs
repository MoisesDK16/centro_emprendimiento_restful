using Microsoft.AspNetCore.Identity;
using Identity.Seeds;
using Identity.Models;

namespace WebAPI.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task SeedDatabaseAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                await DefaultRoles.SeedAsync(userManager, roleManager);
                await DefaultAdminUser.SeedAsync(userManager, roleManager);
                await DefaultEmprendedorUser.SeedAsync(userManager, roleManager);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("DatabaseSeeder");
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }
    }
}
