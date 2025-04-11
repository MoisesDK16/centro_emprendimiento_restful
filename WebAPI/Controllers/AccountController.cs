using Application.DTOs.Correos;
using Application.DTOs.Users;
using Application.Feautures.Authenticate.Commands.AuthenticateCommand;
using Application.Feautures.Authenticate.Commands.RegisterCommand;
using Application.Feautures.Authenticate.Commands.RegisterSellerCommand;
using Application.Feautures.UsuarioC.Commands;
using Application.Feautures.UsuarioC.Queries;
using Application.Interfaces;
using Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WebAPI.Controllers
{
    [ApiVersion("1.0")]
    public class AccountController : BaseApiController
    {
        private readonly IUserService _userService; 
        private readonly IWebHostEnvironment _env;
        private readonly IAccountService _accountService;
        public AccountController(
            IMediator mediator,
            IUserService userService,
            IWebHostEnvironment env,
            IAccountService accountService
            ) : base(mediator)
        {
            _userService = userService;
            _env = env;
            _accountService = accountService;
        }

        [HttpPost("authenticateByEmail")]
        public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticationRequestEmail request)
        {
            return Ok( await Mediator.Send(new AuthenticateByEmailCommand
            { 
                Email = request.Email,
                Password = request.Password,
                IpAddress = generateIpAddress() 
            }));
        }

        [HttpPost("authenticateByUserName")]
        public async Task<IActionResult> AuthenticateByUserNameAsync([FromBody] AuthenticationRequestUserName request)
        {
            var respuesta = await Mediator.Send(new AuthenticateByUserNameCommand
            {
                UserName = request.UserName,
                Password = request.Password,
                IpAddress = generateIpAddress()
            });

            return Ok(respuesta);
        }

        [HttpPost("forgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromQuery] string correo)
        {
            return Ok(await _accountService.ForgotPassword(correo));
        }


        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPassword request, [FromForm] string confirmPassword)
        {
            if (request.Password != confirmPassword)
                return BadRequest("Las contraseñas no coinciden.");

            return Ok(await _accountService.ResetPassword(request));
        }

        [HttpGet("ResetPasswordForm")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordForm([FromQuery] string email, [FromQuery] string token)
        {
            string path = Path.Combine(_env.ContentRootPath, "Templates", "ResetPasswordForm.html");

            if (!System.IO.File.Exists(path))
                return NotFound("Plantilla no encontrada.");

            string htmlContent = await System.IO.File.ReadAllTextAsync(path);

            htmlContent = htmlContent
                .Replace("{{email}}", email)
                .Replace("{{token}}", token);

            return Content(htmlContent, "text/html");
        }


        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegistrarEmprendedor request)
        {
            return Ok(await Mediator.Send(new RegisterCommand
            {
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Email = request.Email,
                Password = request.Password,
                ConfirmPassword = request.ConfirmPassword,
                UserName = request.UserName,
                Identificacion = request.Identificacion,
                Telefono = request.Telefono,
                CiudadOrigen = request.CiudadOrigen,
                Origin = Request.Headers["origin"],
                NombreNegocio = request.NombreNegocio,
                Descripcion = request.Descripcion,
                Direccion = request.DireccionNegocio,
                TelefonoNegocio = request.TelefonoNegocio,
                CategoriaNegocio = request.CategoriaNegocio
            }));
        }

        [Authorize(Roles = "Emprendedor")]
        [HttpPost("registerSeller")]
        public async Task<IActionResult> RegisterSellerAsync([FromBody] RegistrarVendedor request)
        {
            return Ok(await Mediator.Send(new RegisterSellerCommand
            {
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Email = request.Email,
                Password = request.Password,
                ConfirmPassword = request.ConfirmPassword,
                UserName = request.UserName,
                Identificacion = request.Identificacion,
                Telefono = request.Telefono,
                CiudadOrigen = request.CiudadOrigen,
                Origin = Request.Headers["origin"],
                NegocioId = request.NegocioId
            }));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("AllEmprendedores")]
        public async Task<IActionResult> GetAllEmprendedores([FromQuery] ListarEmprendedoresParameters filter)
        {
            var emprendedores = await Mediator.Send(new ListarEmprendedores
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                Email = filter.Email,
                UserName = filter.UserName,
                Identificacion = filter.Identificacion
            });
            return Ok(emprendedores);
        }

        [Authorize(Roles = "Admin,Emprendedor")]
        [HttpGet("listarVendedores")]
        public async Task<IActionResult> ListarVendedores([FromQuery] ListarVendedoresParameters filter)
        {
            var vendedores = await Mediator.Send(new ListarVendedores
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                NegocioId = filter.NegocioId,
                Email = filter.Email,
                UserName = filter.UserName,
                Identificacion = filter.Identificacion
            });
            return Ok(vendedores);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("emprendedorById")]
        public async Task<IActionResult> GetEmprendedorById([FromQuery] string emprendedorId)
        {
            return Ok(await Mediator.Send(new EmprendedorById { Id = emprendedorId }));
        }


        [HttpPost("actualizarUsuario")]
        public async Task<IActionResult> ActualizarUsuario([FromBody] ActualizarUsuario command)
        {
            return Ok(await Mediator.Send(command));

        }

        [HttpGet("confirmar")]
        [AllowAnonymous]
        public async Task<IActionResult> Confirmar(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return BadRequest("Parámetros inválidos.");

            var result = await _userService.Confirmar(userId, token);

            if (result)
            {
                string path = Path.Combine(_env.ContentRootPath, "Templates", "AvisoConfirmacion.html");
                return PhysicalFile(path, "text/html");
            }

            return BadRequest("No se pudo confirmar el correo. Verifique el enlace.");
        }

        [HttpPost("reenviarConfirmacion")]
        [AllowAnonymous]
        public async Task<IActionResult> ReenviarConfirmacion([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("Parámetros inválidos.");

            var result = await _accountService.ReenviarConfirmacion(email);
            if (result) return Ok("Se ha reenviado el correo de confirmación.");
            return BadRequest("No se pudo reenviar el correo de confirmación.");
        }


        [HttpPost("EnviarInformacionAEmprendedores")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EnviarInformacionAEmprendedores([FromBody] EnvioInformacionDTO request)
        {
            if (request.EnvioInformacion == null || string.IsNullOrWhiteSpace(request.EnvioInformacion.Asunto) || string.IsNullOrWhiteSpace(request.EnvioInformacion.Contenido))
                return BadRequest("Parámetros inválidos.");

            if (request.Correos == null || request.Correos.Count == 0)
                return BadRequest("Debe proporcionar al menos un destinatario.");

            return Ok(await _accountService.EnviarInformacionAEmprendedores(request.EnvioInformacion, request.Correos));

        }

        private string generateIpAddress()
        {
            if(Request.Headers.ContainsKey("X-Forwarded-For")) return Request.Headers["X-Forwarded-For"];
            else return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
