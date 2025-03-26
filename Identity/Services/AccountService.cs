using Application.Behaviors;
using Application.DTOs.Users;
using Application.Enums;
using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using Azure.Core;
using Domain.Settings;
using Identity.Helpers;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly Jwt _jwtSettings;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IDateTimeService _dateTimeService;

        public AccountService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<Jwt> jwtSettings,
            SignInManager<ApplicationUser> signInManager,
            IDateTimeService dateTimeService
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
            _signInManager = signInManager;
            _dateTimeService = dateTimeService;
        }

        public async Task<Response<AuthenticationResponse>> logInByEmail(AuthenticationRequestEmail request, string ipAddress)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) throw new ApiException($"No existe cuenta registrada con el email de: {request.Email}.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded) throw new ApiException("Credenciales Invalidas");

            var refreshToken = GenerateRefreshToken(ipAddress);
            JwtSecurityToken jwtSecurityToken = await generateJwtToken(user);

            AuthenticationResponse authenticationResponse = new AuthenticationResponse
            {
                Id = user.Id,
                JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                UserName = user.UserName,
                Email = user.Email,
                Roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false),
                isVerified = user.EmailConfirmed,
                RefreshToken = refreshToken.Token,
            };

            return new Response<AuthenticationResponse>(authenticationResponse, $"Usuario Autenticado {user.UserName}");
        }

        public async Task<Response<AuthenticationResponse>> logInByUserName(AuthenticationRequestUserName request, string ipAddress)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null) throw new ApiException($"No existe cuenta registrada con el nombre de usuario: {request.UserName}.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded) throw new ApiException("Credenciales Invalidas");

            var refreshToken = GenerateRefreshToken(ipAddress);
            JwtSecurityToken jwtSecurityToken = await generateJwtToken(user);

            AuthenticationResponse authenticationResponse = new AuthenticationResponse
            {
                Id = user.Id,
                JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                UserName = user.UserName,
                Email = user.Email,
                Roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false),
                isVerified = user.EmailConfirmed,
                RefreshToken = refreshToken.Token,
            };

            return new Response<AuthenticationResponse>(authenticationResponse, $"Usuario Autenticado {user.UserName}");

        }

        public async Task<Response<string>> RegisterAsync(RegisterRequest request, string origin)
        {

            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
                throw new ApiException($"Usuario con este nombre: {request.UserName} ya existe.");

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail != null)
                throw new ApiException($"User con este email: {request.Email} ya existe.");

            if (request.Identificacion != null)
            {
               if(!ValidacionIdentificacion.VerificaIdentificacion(request.Identificacion))
                    throw new ApiException($"Identificacion no valida");
            }

            var userWithSameIdentification = await _userManager.Users.FirstOrDefaultAsync(x => x.Identificacion == request.Identificacion);

            if (userWithSameIdentification != null)
                throw new ApiException($"User con esta identificacion: {request.Identificacion} ya existe.");

            if (request.UserName.Contains("@"))
                throw new ApiException($"El nombre de usuario no puede contene @");

            var user = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.UserName,
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Identificacion = request.Identificacion ?? null,
                Telefono = request.Telefono ?? null,
                CiudadOrigen = request.CiudadOrigen,
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.Emprendedor.ToString());
                return new Response<string>($"Usuario registrado exitosamente: {request.UserName}");
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new ApiException($"La cuenta de Usuario no se pudo crear. Errors: {errors}");
            }
        }

        public async Task<Response<string>> RegisterVendedorAsync(RegisterRequest request, string origin)
        {

            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
                throw new ApiException($"User con este Nombre Usuario: {request.UserName} ya existe.");

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail != null)
                throw new ApiException($"User con este email: {request.Email} ya existe.");


            if (request.Identificacion != null)
            {
                if (!ValidacionIdentificacion.VerificaIdentificacion(request.Identificacion))
                    throw new ApiException($"Identificacion no valida");
            }

            if (request.UserName.Contains("@"))
                throw new ApiException($"El nombre de usuario no puede contene @");

            var user = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.UserName,
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Identificacion = request.Identificacion ?? null,
                Telefono = request.Telefono ?? null,
                CiudadOrigen = request.CiudadOrigen,
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.Vendedor.ToString());
                return new Response<string>($"Usuario registrado exitosamente: {request.UserName}");
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new ApiException($"La cuenta de Usuario no se pudo crear. Errors: {errors}");
            }
        }

        private async Task<JwtSecurityToken> generateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            string ipAddress = IpHelper.GetIpAdress();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
                new Claim("ip", ipAddress)
            }
            .Union(userClaims)
            .Union(roleClaims); 

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials
            );

            return jwtSecurityToken;
        }

        private RefreshToken GenerateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now,
                CreatedByIp = ipAddress
            };
        }

        private string RandomTokenString()
        {
            using var rng = RandomNumberGenerator.Create();
            var randomBytes = new byte[40]; // 40 bytes aleatorios
            rng.GetBytes(randomBytes);

            return BitConverter.ToString(randomBytes).Replace("-", "");
        }
    }
}
 