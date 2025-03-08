using Application.Enums;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
namespace Identity.Seeds
{
    public static class DefaultAdminUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Seed Default Admin User
            var defaultUser = new ApplicationUser
            {
                UserName = "userAdmin",
                Email = "userAdmin@gmail.com",
                Nombre = "Moises",
                Apellido = "Loor",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Identificacion = "1234567890",
                CiudadOrigen = "Guayas",
                Telefono = "0987654321",
            };

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Admin1234.");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                }
            }
        }
    }
}
