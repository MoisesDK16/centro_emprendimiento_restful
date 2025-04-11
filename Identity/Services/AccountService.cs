using Application.Behaviors;
using Application.DTOs.Correos;
using Application.DTOs.Negocios;
using Application.DTOs.Users;
using Application.Enums;
using Application.Exceptions;
using Application.Interfaces;
using Application.Services.ExternalS;
using Application.Specifications;
using Application.Wrappers;
using Domain.Entities;
using Domain.Enums.Negocio;
using Domain.Settings;
using Identity.Helpers;
using Identity.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Uno.Extensions;

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
        private readonly IWebHostEnvironment _env;

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
            IReadOnlyRepositoryAsync<NegocioVendedores> negocioVendedoresReadingRepository,
            IWebHostEnvironment env
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
            _env = env;
        }

        public async Task<Response<AuthenticationResponse>> logInByEmail(AuthenticationRequestEmail request, string ipAddress)
        {

            var userToLog = _userManager.Users.FirstOrDefault(x => x.Email == request.Email);

            if (userToLog != null)
            {
                if (userToLog.EmailConfirmed == false)
                    throw new ApiException($"Tu usuario con nombre {userToLog.UserName} y correo {userToLog.Email} no tiene confirmacion de correo electrónico. " +
                        $"Por favor, verifica tu bandeja de entrada o carpeta de spam para confirmar su correo electrónico.");
                
                /*var negociosEmprendedor = await _negocioReadingRepository.ListAsync(new NegocioSpecification(userToLog.Id));
                if (negociosEmprendedor.All(n => n.estado != Estado.Activo))
                    throw new ApiException($"Tu usuario con nombre {userToLog.UserName} no tiene negocios activos. ");*/

            }

            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                    throw new ApiException($"No existe cuenta registrada con el email: {request.Email}.");

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
                if (!result.Succeeded)
                    throw new ApiException("Credenciales inválidas");

                var refreshToken = GenerateRefreshToken(ipAddress);
                JwtSecurityToken jwtSecurityToken = await generateJwtToken(user);
                var roles = await _userManager.GetRolesAsync(user);

                var authenticationResponse = new AuthenticationResponse
                {
                    Id = user.Id,
                    JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = roles,
                    isVerified = user.EmailConfirmed,
                    RefreshToken = refreshToken.Token,
                    Negocios = new List<SelectNegocioDTO>() // Inicial vacío
                };

                // 1. Negocios como Emprendedor
                var negociosEmprendedor = await _negocioReadingRepository.ListAsync(new NegocioSpecification(user.Id));

                if (negociosEmprendedor?.Any() == true)
                {
                    authenticationResponse.Negocios = negociosEmprendedor
                        .Where(n => !string.IsNullOrEmpty(n?.nombre))
                        .Select(n => new SelectNegocioDTO
                        {
                            Id = n.Id,
                            Nombre = n.nombre,
                            Estado = n.estado
                        }).ToList();
                }
                else
                {
                    // 2. Negocios como Vendedor
                    var negociosVendedor = await _negocioVendedoresReadingRepository.ListAsync(new NegocioVendedorSpecification(user.Id));

                    if (negociosVendedor?.Any() == true)
                    {
                        authenticationResponse.Negocios = negociosVendedor
                            .Where(nv => nv?.Negocio != null && !string.IsNullOrEmpty(nv.Negocio.nombre))
                            .Select(nv => new SelectNegocioDTO
                            {
                                Id = nv.NegocioId,
                                Nombre = nv.Negocio.nombre,
                                Estado = nv.Negocio.estado
                            }).ToList();
                    }
                    else if (roles.Contains("ADMIN"))
                    {
                        Console.WriteLine("USUARIO ADMIN SIN NEGOCIOS");
                    }
                }

                return new Response<AuthenticationResponse>(authenticationResponse, $"Usuario autenticado: {user.UserName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR EN LOGIN POR EMAIL: " + ex.Message);
                return new Response<AuthenticationResponse>
                {
                    Succeeded = false,
                    Message = "Error durante el inicio de sesión.",
                    Data = null,
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<Response<AuthenticationResponse>> logInByUserName(AuthenticationRequestUserName request, string ipAddress)
        {
            var userToLog = _userManager.Users.FirstOrDefault(x => x.UserName == request.UserName);

            if (userToLog != null)
            {
                if(userToLog.EmailConfirmed == false)
                    throw new ApiException($"Tu usuario con nombre {userToLog.UserName} y correo {userToLog.Email} no tiene confirmacion de correo electrónico. " +
                        $"Por favor, verifica tu bandeja de entrada o carpeta de spam para confirmar su correo electrónico.");
            
                /*var negociosEmprendedor = await _negocioReadingRepository.ListAsync(new NegocioSpecification(userToLog.Id));
                if (negociosEmprendedor.All(n => n.estado != Estado.Activo))
                    throw new ApiException($"Tu usuario con nombre {userToLog.UserName} no tiene negocios activos. ");*/
            }

            try
            {
                var user = await _userManager.FindByNameAsync(request.UserName);
                Console.WriteLine("USER: " + user);
                if (user == null)
                    throw new ApiException($"No existe cuenta registrada con el nombre de usuario: {request.UserName}.");

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
                if (!result.Succeeded)
                    throw new ApiException("Credenciales inválidas");

                var refreshToken = GenerateRefreshToken(ipAddress);
                var jwtSecurityToken = await generateJwtToken(user);
                var roles = await _userManager.GetRolesAsync(user);

                var authenticationResponse = new AuthenticationResponse
                {
                    Id = user.Id,
                    JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = roles,
                    isVerified = user.EmailConfirmed,
                    RefreshToken = refreshToken.Token,
                    Negocios = new List<SelectNegocioDTO>() 
                };

                // 1. Negocios como Emprendedor
                var negociosEmprendedor = await _negocioReadingRepository.ListAsync(new NegocioSpecification(user.Id));

                if (negociosEmprendedor?.Any() == true)
                {
                    Console.WriteLine("NEGOCIOS DEL EMPRENDEDOR: " + negociosEmprendedor.Count);
                    authenticationResponse.Negocios = negociosEmprendedor
                        .Where(n => !string.IsNullOrEmpty(n?.nombre))
                        .Select(n => new SelectNegocioDTO
                        {
                            Id = n.Id,
                            Nombre = n.nombre,
                            Estado = n.estado
                        }).ToList();
                }
                else
                {
                    // 2. Negocios como Vendedor
                    var negociosVendedor = await _negocioVendedoresReadingRepository.ListAsync(new NegocioVendedorSpecification(user.Id));

                    if (negociosVendedor?.Any() == true)
                    {
                        authenticationResponse.Negocios = negociosVendedor
                            .Where(nv => nv?.Negocio != null && !string.IsNullOrEmpty(nv.Negocio.nombre))
                            .Select(nv => new SelectNegocioDTO
                            {
                                Id = nv.NegocioId,
                                Nombre = nv.Negocio.nombre,
                                Estado = nv.Negocio.estado
                            }).ToList();
                    }
                    else if (roles.Contains("ADMIN"))
                    {
                        Console.WriteLine("USUARIO ADMIN SIN NEGOCIOS");
                    }

                }

                return new Response<AuthenticationResponse>(authenticationResponse, $"Usuario autenticado: {user.UserName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR EN LOGIN: " + ex.Message);
                Console.WriteLine("STACK TRACE: " + ex.StackTrace);

                return new Response<AuthenticationResponse>
                {
                    Succeeded = false,
                    Message = "Error durante el inicio de sesión.",
                    Data = null,
                    Errors = new List<string> { ex.Message }
                };
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

            ApplicationUser user = null;
            Negocio negocio = null;

            try
            {
                user = new ApplicationUser
                {
                    Email = request.Email,
                    UserName = request.UserName,
                    Nombre = request.Nombre,
                    Apellido = request.Apellido,
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    Identificacion = request.Identificacion,
                    Telefono = request.Telefono,
                    CiudadOrigen = request.CiudadOrigen,
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new ApiException($"No se pudo crear la cuenta de usuario. Errores: {errors}");
                }

                await _userManager.AddToRoleAsync(user, Roles.Emprendedor.ToString());

                negocio = new Negocio
                {
                    nombre = request.NombreNegocio,
                    descripcion = request.Descripcion,
                    direccion = request.DireccionNegocio,
                    estado = Estado.Pendiente,
                    telefono = request.TelefonoNegocio,
                    CategoriaId = request.CategoriaNegocio,
                    EmprendedorId = user.Id
                };

                await _negocioRepository.AddAsync(negocio);
                await _negocioRepository.SaveChangesAsync();

                await EnviarCorreoConfirmacionAsync(user.Id);
                await EnviarSolicitudAprobacionNegocioAsync(user.Id, negocio);

                return new Response<string>("Se ha enviado un enlace de confirmación a tu correo electrónico.");
            }
            catch (Exception ex)
            {
                if (negocio != null)
                {
                    await _negocioRepository.DeleteAsync(negocio);
                    await _negocioRepository.SaveChangesAsync();
                }

                if (user != null)
                {
                    await _userManager.DeleteAsync(user);
                }

                var inner = ex.InnerException?.Message ?? ex.Message;
                throw new ApiException($"Error al registrar usuario y negocio: {inner}");
            }
        }

        public async Task<Response<string>> RegisterVendedorAsync(RegistrarVendedor request, string origin)
        {
            var negocio = await _negocioReadingRepository.GetByIdAsync(request.NegocioId)
                ?? throw new ApiException($"Negocio con ID {request.NegocioId} no encontrado");

            if (await _userManager.FindByNameAsync(request.UserName) != null)
                throw new ApiException($"Ya existe un usuario con nombre: {request.UserName}");

            if (await _userManager.FindByEmailAsync(request.Email) != null)
                throw new ApiException($"Ya existe un usuario con email: {request.Email}");

            if (!string.IsNullOrWhiteSpace(request.Identificacion) &&
                !ValidacionIdentificacion.VerificaIdentificacion(request.Identificacion))
                throw new ApiException("Identificación no válida");

            if (request.UserName.Contains("@"))
                throw new ApiException("El nombre de usuario no puede contener '@'.");

            ApplicationUser user = null;
            NegocioVendedores negocioVendedor = null;

            try
            {
                user = new ApplicationUser
                {
                    Email = request.Email,
                    UserName = request.UserName,
                    Nombre = request.Nombre,
                    Apellido = request.Apellido,
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    Identificacion = request.Identificacion,
                    Telefono = request.Telefono,
                    CiudadOrigen = request.CiudadOrigen,
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new ApiException($"No se pudo crear la cuenta de usuario. Errores: {errors}");
                }

                await _userManager.AddToRoleAsync(user, Roles.Vendedor.ToString());

                negocioVendedor = new NegocioVendedores
                {
                    NegocioId = negocio.Id,
                    VendedorId = user.Id
                };

                await _negocioVendedoresRepository.AddAsync(negocioVendedor);
                await _negocioReadingRepository.SaveChangesAsync();

                // Generar token
                var rawToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var token = WebUtility.UrlEncode(rawToken);

                // Preparar correo
                string path = Path.Combine(_env.ContentRootPath, "Templates", "Confirmar.html");
                string content = File.ReadAllText(path);

                string url = $"https://localhost:7050/api/v1/Account/confirmar?token={token}&userId={user.Id}";
                string htmlBody = string.Format(content, user.UserName, url);

                var correo = new CorreoDTO
                {
                    Para = user.Email,
                    Asunto = "Confirmación de cuenta (Vendedor)",
                    Contenido = htmlBody
                };

                var enviado = CorreoServicio.Enviar(correo);
                if (!enviado)
                    throw new Exception("No se pudo enviar el correo de confirmación. Asegúrate de usar un correo válido y real (ej. Gmail).");

                return new Response<string>("Se ha enviado un enlace de confirmación a tu correo electrónico.");
            }
            catch (Exception ex)
            {
                // Rollback manual
                if (negocioVendedor != null)
                {
                    await _negocioVendedoresRepository.DeleteAsync(negocioVendedor);
                    await _negocioReadingRepository.SaveChangesAsync();
                }

                if (user != null)
                {
                    await _userManager.DeleteAsync(user);
                }

                var inner = ex.InnerException?.Message ?? ex.Message;
                throw new ApiException($"Error al registrar vendedor: {inner}");
            }
        }
        public async Task EnviarCorreoConfirmacionAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new ApiException("Usuario no encontrado para confirmación de correo.");

            var rawToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var token = WebUtility.UrlEncode(rawToken);

            string path = Path.Combine(_env.ContentRootPath, "Templates", "Confirmar.html");
            string content = File.ReadAllText(path);

            string url = $"https://localhost:7050/api/v1/Account/confirmar?token={token}&userId={user.Id}";

            string htmlBody = content
                .Replace("{{userName}}", user.UserName)
                .Replace("{{url}}", url);

            var correo = new CorreoDTO
            {
                Para = user.Email,
                Asunto = "Confirmación de cuenta",
                Contenido = htmlBody
            };

            if (!CorreoServicio.Enviar(correo))
                throw new ApiException("No se pudo enviar el correo de confirmación.");
        }


        public async Task EnviarSolicitudAprobacionNegocioAsync(string userId, Negocio negocio)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new ApiException("Usuario no encontrado para solicitud de aprobación.");

            string pathNegocio = Path.Combine(_env.ContentRootPath, "Templates", "SolicitudAprobacionNegocio.html");
            string contentNegocio = File.ReadAllText(pathNegocio);

            string aprobarUrl = $"https://localhost:7050/api/v1/Negocio/aprobar?negocioId={negocio.Id}&aprobado=true";
            string rechazarUrl = $"https://localhost:7050/api/v1/Negocio/aprobar?negocioId={negocio.Id}&aprobado=false";

            string cuerpoHtml = contentNegocio
                .Replace("{{NombreNegocio}}", negocio.nombre)
                .Replace("{{Direccion}}", negocio.direccion)
                .Replace("{{Telefono}}", negocio.telefono)
                .Replace("{{Descripcion}}", negocio.descripcion ?? "Sin descripción")
                .Replace("{{NombreEmprendedor}}", user.Nombre)
                .Replace("{{ApellidoEmprendedor}}", user.Apellido)
                .Replace("{{TelefonoEmprendedor}}", user.Telefono)
                .Replace("{{CiudadOrigen}}", user.CiudadOrigen)
                .Replace("{{Identificacion}}", user.Identificacion)
                .Replace("{{AprobarUrl}}", aprobarUrl)
                .Replace("{{RechazarUrl}}", rechazarUrl);

            var solicitud = new CorreoDTO
            {
                Para = "moisesloor122@gmail.com",
                Asunto = "Solicitud de aprobación de negocio",
                Contenido = cuerpoHtml
            };

            if (!CorreoServicio.Enviar(solicitud))
                throw new ApiException("No se pudo enviar la solicitud de aprobación del negocio.");
        }




        public async Task<bool> ReenviarConfirmacion(string correo)
        {
            if (string.IsNullOrEmpty(correo))
                throw new ApiException("El correo no puede estar vacío.");

            var user = await _userManager.FindByEmailAsync(correo);
            if (user == null)
                throw new ApiException("No se encontró un usuario con ese correo.");

            // Generar token de confirmación
            var rawToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var token = WebUtility.UrlEncode(rawToken);

            // Cargar plantilla HTML
            string path = Path.Combine(_env.ContentRootPath, "Templates", "Confirmar.html");
            if (!File.Exists(path))
                throw new ApiException("No se encontró la plantilla de correo.");

            string content = File.ReadAllText(path);

            string url = $"https://localhost:7050/api/v1/Account/confirmar?token={token}&userId={user.Id}";
            string htmlBody = content
                .Replace("{{userName}}", user.UserName)
                .Replace("{{url}}", url);

            Console.WriteLine("URL de confirmación: " + url);

            var correoDTO = new CorreoDTO
            {
                Para = user.Email,
                Asunto = "Confirmación de cuenta",
                Contenido = htmlBody
            };

            var enviado = CorreoServicio.Enviar(correoDTO);
            if (!enviado)
                throw new ApiException("No se pudo enviar el correo de confirmación. Asegúrese de que el correo exista y sea válido.");

            return enviado;
        }

        public Task<Response<string>> EnviarInformacionAEmprendedores(CorreoDTO correo, List<string> correos)
        {
            if (correos == null || !correos.Any())
                throw new ApiException("Debe enviar al menos un destinatario.");

            try
            {
                string path = Path.Combine(_env.ContentRootPath, "Templates", "InformacionParaEmprendedores.html");

                if (!File.Exists(path))
                    throw new ApiException("La plantilla HTML no fue encontrada.");

                string content = File.ReadAllText(path);
                string cuerpoHtml = content.Replace("{{contenido}}", correo.Contenido);

                foreach (var destinatario in correos)
                {
                    var correoEnviar = new CorreoDTO
                    {
                        Para = destinatario,
                        Asunto = correo.Asunto,
                        Contenido = cuerpoHtml
                    };

                    var enviado = CorreoServicio.Enviar(correoEnviar);

                    Console.WriteLine($"Enviando a {destinatario}... ¿Enviado?: {enviado}");

                    if (!enviado)
                    {
                        Console.WriteLine($"❌ No se pudo enviar a: {destinatario}");
                    }
                }

                return Task.FromResult(new Response<string>("Correos enviados correctamente."));
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error al enviar correos: " + ex.Message);
                throw new ApiException("Error al enviar correos: " + ex.Message);
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

        public async Task<Response<string>> ResetPassword(ResetPassword request)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Email == request.Email)
                       ?? throw new ApiException($"No existe cuenta registrada con el email: {request.Email}.");

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new ApiException($"No se pudo restablecer la contraseña. Errores: {errors}");
            }

            return new Response<string>($"Contraseña actualizada correctamente para el usuario: {user.UserName}");
        }

        public async Task<bool> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new ApiException("No existe una cuenta registrada con ese correo electrónico.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebUtility.UrlEncode(token);

            // Ruta del HTML
            string path = Path.Combine(_env.ContentRootPath, "Templates", "forgotPassword.html");
            if (!File.Exists(path))
                throw new ApiException("Plantilla de correo no encontrada.");

            string content = await File.ReadAllTextAsync(path);

            string resetUrlForm = $"https://localhost:7050/api/v1/Account/ResetPasswordForm?email={email}&token={encodedToken}";

            string cuerpoHtml = content
                .Replace("{{userName}}", user.UserName)
                .Replace("{{resetUrl}}", resetUrlForm);

            var correo = new CorreoDTO
            {
                Para = user.Email,
                Asunto = "Restablecer contraseña",
                Contenido = cuerpoHtml
            };

            bool enviado = CorreoServicio.Enviar(correo);

            if (!enviado)
                throw new ApiException("No se pudo enviar el correo de restablecimiento de contraseña.");

            return true;
        }

    }
}
 