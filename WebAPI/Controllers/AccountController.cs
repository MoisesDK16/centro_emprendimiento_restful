using Application.DTOs.Users;
using Application.Feautures.Authenticate.Commands.AuthenticateCommand;
using Application.Feautures.Authenticate.Commands.RegisterCommand;
using Application.Feautures.Authenticate.Commands.RegisterSellerCommand;
using Application.Interfaces;
using Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiVersion("1.0")]
    public class AccountController : BaseApiController
    {
        private readonly IUserService _userService;
        public AccountController(IMediator mediator, IUserService userService) : base(mediator)
        {
            _userService = userService;
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
            return Ok(await Mediator.Send(new AuthenticateByUserNameCommand
            {
                UserName = request.UserName,
                Password = request.Password,
                IpAddress = generateIpAddress()
            }));
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

        private string generateIpAddress()
        {
            if(Request.Headers.ContainsKey("X-Forwarded-For")) return Request.Headers["X-Forwarded-For"];
            else return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
