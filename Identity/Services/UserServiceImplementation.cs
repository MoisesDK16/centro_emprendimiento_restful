using Application.DTOs.Users;
using Application.Interfaces;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Services
{
    public class UserServiceImplementation : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserServiceImplementation(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> UserExistsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user != null;
        }

        public async Task<UserInfo> GetUserInfoAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return null;

            return new UserInfo
            {
                Id = user.Id,
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Identificacion = user.Identificacion,
                CiudadOrigen = user.CiudadOrigen,
                Telefono = user.Telefono
            };
        }
    }
}
