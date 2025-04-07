using Application.DTOs.Correos;
using Application.DTOs.Users;
using Application.Feautures.Authenticate.Commands.AuthenticateCommand;
using Application.Feautures.Authenticate.Commands.RegisterCommand;
using Application.Feautures.Authenticate.Commands.RegisterSellerCommand;
using Application.Interfaces;
using Application.Services.ExternalS;
using Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

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
        public async Task<IActionResult> GetAllEmprendedores()
        {
            return Ok(new Response<List<UserInfo>>(await _userService.GetEmprendedoresAsync()));
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

        [HttpGet("reenviarConfirmacion")]
        [AllowAnonymous]
        public async Task<IActionResult> ReenviarConfirmacion([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("Parámetros inválidos.");

            var result = await _accountService.ReenviarConfirmacion(email);
            if (result) return Ok("Se ha reenviado el correo de confirmación.");
            return BadRequest("No se pudo reenviar el correo de confirmación.");
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("EnviarInformacionAEmprendedores")]
        [AllowAnonymous]
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
