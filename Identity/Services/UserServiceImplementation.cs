using Application.DTOs.Negocios;
using Application.DTOs.Users;
using Application.Interfaces;
using Application.Specifications;
using Domain.Entities;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using NuGet.Common;
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
        private readonly IReadOnlyRepositoryAsync<Negocio> _negocioReadingRepository;
        private readonly IReadOnlyRepositoryAsync<NegocioVendedores> _negocioReadingVendedoresRepository;

        public UserServiceImplementation(UserManager<ApplicationUser> userManager, IReadOnlyRepositoryAsync<Negocio> negocioReadingRepository)
        {
            _userManager = userManager;
            _negocioReadingRepository = negocioReadingRepository;
        }

        public async Task<bool> UserExistsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user != null;
        }

        public async Task<UserEmprendedor> GetEmprendedorInfoAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId)
                       ?? throw new Exception("El usuario no fue encontrado");

            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("Emprendedor"))
                throw new Exception("El usuario no es un emprendedor");

            // Obtener negocios del emprendedor antes de retornar
            var negocios = await _negocioReadingRepository.ListAsync(new NegocioSpecification(user.Id));

            var negociosInfo = negocios.Select(n => new NegocioInfoDTO
            {
                Id = n.Id,
                Nombre = n.nombre,
                Telefono = n.telefono,
                Direccion = n.direccion,
                Estado = n.estado
            }).ToList();

            return new UserEmprendedor
            {
                Id = user.Id,
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Identificacion = user.Identificacion,
                CiudadOrigen = user.CiudadOrigen,
                Telefono = user.Telefono,
                Email = user.Email,
                UserName = user.UserName,
                NegociosInfo = negociosInfo
            };
        }


        public async Task<bool> Confirmar(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new Exception("Usuario no encontrado");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            Console.WriteLine("RESULT: " + result.Succeeded);
            return result.Succeeded;
        }

        public async Task<bool> ActualizarUsuario(UserInfo userInfo)
        {
            var user = await _userManager.FindByIdAsync(userInfo.Id);
            if (user == null) throw new Exception("Usuario no encontrado");
            user.Nombre = userInfo.Nombre;
            user.Apellido = userInfo.Apellido;
            user.CiudadOrigen = userInfo.CiudadOrigen;
            user.Telefono = userInfo.Telefono;
            user.UserName = userInfo.UserName;
            user.Email = userInfo.Email;
            return _userManager.UpdateAsync(user).Result.Succeeded;
        }

        public async Task<List<UserEmprendedor>> ListarEmprendedores()
        {
            var emprendedores = await _userManager.GetUsersInRoleAsync("Emprendedor");
            List<UserEmprendedor> userEmprendedores = new List<UserEmprendedor>();

            foreach (var user in emprendedores)
            {
                //Negocios del emprendedor
                var negocios = await _negocioReadingRepository.ListAsync(new NegocioSpecification(user.Id));

                var userInfo = new UserEmprendedor
                {
                    Id = user.Id,
                    Nombre = user.Nombre,
                    Apellido = user.Apellido,
                    Identificacion = user.Identificacion,
                    CiudadOrigen = user.CiudadOrigen,
                    Telefono = user.Telefono,
                    Email = user.Email,
                    UserName = user.UserName,
                    NegociosInfo = negocios.Select(n => new NegocioInfoDTO
                    {
                        Id = n.Id,
                        Nombre = n.nombre,
                        Telefono = n.telefono,
                        Direccion = n.direccion,
                        Estado = n.estado
                    }).ToList()

                };

                userEmprendedores.Add(userInfo);
            }

            return userEmprendedores;
        }

        public async Task<List<UserVendedor>> ListarVendedores()
        {
            var vendedores = await _userManager.GetUsersInRoleAsync("Vendedor");
            Console.WriteLine("Vendedores: " + vendedores.Count);
            List<UserVendedor> userVendedores = new List<UserVendedor>();
            foreach (var user in vendedores)
            {
                Console.WriteLine("Vendedor: " + user.Id);
                //Negocios del vendedor
                var negocios = await _negocioReadingVendedoresRepository.ListAsync(new NegocioVendedorSpecification(user.Id));
                Console.WriteLine("Negocios: " + negocios.Count);
                var userInfo = new UserVendedor
                {
                    Id = user.Id,
                    Nombre = user.Nombre,
                    Apellido = user.Apellido,
                    Identificacion = user.Identificacion,
                    CiudadOrigen = user.CiudadOrigen,
                    Telefono = user.Telefono,
                    Email = user.Email,
                    UserName = user.UserName,
                    NegociosInfo = negocios.Select(n => new NegocioInfoDTO
                    {
                        Id = n.Negocio.Id,
                        Nombre = n.Negocio.nombre,
                        Telefono = n.Negocio.telefono,
                        Direccion = n.Negocio.direccion,
                        Estado = n.Negocio.estado
                    }).ToList()
                };
                userVendedores.Add(userInfo);
            }
            return userVendedores;
        }

        public async Task<bool> IsAdmin(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new Exception("Usuario no encontrado");

            var roles = await _userManager.GetRolesAsync(user);
            return roles.Contains("Admin");
        }

        public async Task<bool> IsEmprendedor(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new Exception("Usuario no encontrado");

            var roles = await _userManager.GetRolesAsync(user);
            return roles.Contains("Emprendedor");
        }

        public async Task<bool> IsVendedor(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new Exception("Usuario no encontrado");

            var roles = await _userManager.GetRolesAsync(user);
            return roles.Contains("Vendedor");
        }
    }
}
