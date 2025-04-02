using Application.Behaviors;
using Application.DTOs.Negocios;
using Application.DTOs.Users;
using Application.Enums;
using Application.Exceptions;
using Application.Interfaces;
using Application.Specifications;
using Application.Wrappers;
using Domain.Entities;
using Domain.Settings;
using Identity.Helpers;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReadOnlyRepositoryAsync<Negocio> _negocioReadingRepository;
        private readonly IRepositoryAsync<Negocio> _negocioRepository;
        private readonly IRepositoryAsync<NegocioVendedores> _negocioVendedoresRepository;
        private readonly IReadOnlyRepositoryAsync<NegocioVendedores> _negocioVendedoresReadingRepository;   

        public AccountService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<Jwt> jwtSettings,
            SignInManager<ApplicationUser> signInManager,
            IDateTimeService dateTimeService,
            IUnitOfWork unitOfWork,
            IReadOnlyRepositoryAsync<Negocio> negocioReadingRepository,
            IRepositoryAsync<Negocio> negocioRepository,
            IRepositoryAsync<NegocioVendedores> negocioVendedoresRepository,
            IReadOnlyRepositoryAsync<NegocioVendedores> negocioVendedoresReadingRepository
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
            _signInManager = signInManager;
            _dateTimeService = dateTimeService;
            _unitOfWork = unitOfWork;
            _negocioReadingRepository = negocioReadingRepository;
            _negocioRepository = negocioRepository;
            _negocioVendedoresRepository = negocioVendedoresRepository;
            _negocioVendedoresReadingRepository = negocioVendedoresReadingRepository;
        }

        public async Task<Response<AuthenticationResponse>> logInByEmail(AuthenticationRequestEmail request, string ipAddress)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) throw new ApiException($"No existe cuenta registrada con el email de: {request.Email}.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded) throw new ApiException("Credenciales Invalidas");

            var refreshToken = GenerateRefreshToken(ipAddress);
            JwtSecurityToken jwtSecurityToken = await generateJwtToken(user);

            var negociosUser = await _negocioReadingRepository.ListAsync(new NegocioSpecification(user.Id));

            AuthenticationResponse authenticationResponse = new AuthenticationResponse
            {
                Id = user.Id,
                JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                UserName = user.UserName,
                Email = user.Email,
                Roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false),
                isVerified = user.EmailConfirmed,
                RefreshToken = refreshToken.Token,
                Negocios = negociosUser.Any()  ? negociosUser.Select(x => new SelectNegocioDTO
                {
                    Id = x.Id,
                    Nombre = x.nombre
                }).ToList(): null
            };

            return new Response<AuthenticationResponse>(authenticationResponse, $"Usuario Autenticado {user.UserName}");
        }

        public async Task<Response<AuthenticationResponse>> logInByUserName(AuthenticationRequestUserName request, string ipAddress)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            Console.WriteLine("USER: " + user);
            if (user == null) throw new ApiException($"No existe cuenta registrada con el nombre de usuario: {request.UserName}.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded) throw new ApiException("Credenciales Invalidas");

            var refreshToken = GenerateRefreshToken(ipAddress);
            JwtSecurityToken jwtSecurityToken = await generateJwtToken(user);
            var negociosEmprendedor = await _negocioReadingRepository.ListAsync(new NegocioSpecification(user.Id));

            if (negociosEmprendedor.Count != 0){
                Console.WriteLine("NEGOCIOS DEL EMPREDEDOR: " + negociosEmprendedor.Count());

                AuthenticationResponse authenticationResponse = new AuthenticationResponse
                {
                    Id = user.Id,
                    JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false),
                    isVerified = user.EmailConfirmed,
                    RefreshToken = refreshToken.Token,
                    Negocios = negociosEmprendedor.Select(x => new SelectNegocioDTO
                    {
                        Id = x.Id,
                        Nombre = x.nombre
                    }).ToList()

                };

                return new Response<AuthenticationResponse>(authenticationResponse, $"Usuario Autenticado {user.UserName}");
            }else{
                var negociosVendedor = await _negocioVendedoresReadingRepository.ListAsync(new NegocioVendedorSpecification(user.Id));

                AuthenticationResponse authenticationResponse = new AuthenticationResponse
                {
                    Id = user.Id,
                    JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false),
                    isVerified = user.EmailConfirmed,
                    RefreshToken = refreshToken.Token,
                    Negocios = negociosVendedor.Select(x => new SelectNegocioDTO
                    {
                        Id = x.NegocioId,
                        Nombre = x.Negocio.nombre
                    }).ToList()
                };

                return new Response<AuthenticationResponse>(authenticationResponse, $"Usuario Autenticado {user.UserName}");
            }        
        }

        public async Task<Response<string>> RegisterAsync(RegistrarEmprendedor request, string origin)
        {
            if (request.UserName.Contains("@"))
                throw new ApiException("El nombre de usuario no puede contener '@'.");

            var validations = new List<(bool Condition, string Message)>
    {
        ((await _userManager.FindByNameAsync(request.UserName)) != null, $"Usuario con este nombre: {request.UserName} ya existe."),
        ((await _userManager.FindByEmailAsync(request.Email)) != null, $"Usuario con este email: {request.Email} ya existe."),
        (!string.IsNullOrWhiteSpace(request.Identificacion) && !ValidacionIdentificacion.VerificaIdentificacion(request.Identificacion), "Identificación no válida."),
        ((await _userManager.Users.AnyAsync(x => x.Identificacion == request.Identificacion)), $"Usuario con esta identificación: {request.Identificacion} ya existe.")
    };

            foreach (var (condition, message) in validations)
            {
                if (condition) throw new ApiException(message);
            }

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var user = new ApplicationUser
                {
                    Email = request.Email,
                    UserName = request.UserName,
                    Nombre = request.Nombre,
                    Apellido = request.Apellido,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    Identificacion = request.Identificacion,
                    Telefono = request.Telefono,
                    CiudadOrigen = request.CiudadOrigen
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new ApiException($"No se pudo crear la cuenta de usuario. Errores: {errors}");
                }

                await _userManager.AddToRoleAsync(user, Roles.Emprendedor.ToString());

                var negocio = new Negocio
                {
                    nombre = request.NombreNegocio,
                    descripcion = request.Descripcion,
                    direccion = request.DireccionNegocio,
                    estado = Domain.Enums.Negocio.Estado.Pendiente,
                    telefono = request.TelefonoNegocio,
                    CategoriaId = request.CategoriaNegocio,
                    EmprendedorId = user.Id
                };

                await _negocioRepository.AddAsync(negocio);
                await _negocioRepository.SaveChangesAsync();

                await _unitOfWork.CommitAsync();

                return new Response<string>($"Usuario de tipo Emprendedor ha sido registrado exitosamente: {request.UserName}");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                var inner = ex.InnerException?.Message ?? ex.Message;
                throw new ApiException($"Error al registrar usuario y negocio: {inner}");
            }

        }

        public async Task<Response<string>> RegisterVendedorAsync(RegistrarVendedor request, string origin)
        {
            var negocio = await _negocioReadingRepository.GetByIdAsync(request.NegocioId) ?? throw new ApiException($"Negocio con {request.NegocioId} no encontrado");

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

            await _unitOfWork.BeginTransactionAsync();

            try{
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

                    Console.WriteLine("user ejeke: " + user.Id);

                    var negocioVendedor = new NegocioVendedores
                    {
                        NegocioId = negocio.Id,
                        VendedorId = user.Id,
                    };

                    await _negocioVendedoresRepository.AddAsync(negocioVendedor);
                    await _negocioReadingRepository.SaveChangesAsync();
                    await _unitOfWork.CommitAsync();
                    return new Response<string>($"Vendedor registrado exitosamente: {request.UserName}");
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new ApiException($"La cuenta de Usuario no se pudo crear. Errors: {errors}");
                }

            }
            catch(Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                var inner = ex.InnerException?.Message ?? ex.Message;
                throw new ApiException($"Error al registrar usuario y negocio: {inner}");
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
 