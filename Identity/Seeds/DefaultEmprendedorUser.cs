using Application.Enums;
using Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Seeds
{
    public static class DefaultEmprendedorUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Seed Default Admin User
            var defaultUser = new ApplicationUser
            {
                UserName = "userEmprendedor",
                Email = "userEmprendedor@gmail.com",
                Nombre = "Raul",
                Apellido = "Duran",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Identificacion = "123456789",
                CiudadOrigen = "Bogota",
                Telefono = "123456789"
            };

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Emprendedor1234.");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Emprendedor.ToString());
                }
            }
        }
    }
}
