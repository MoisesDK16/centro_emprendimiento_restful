using Application.DTOs.Users;
using Application.Feautures.Authenticate.Commands.AuthenticateCommand;
using Application.Feautures.Authenticate.Commands.RegisterCommand;
using Application.Feautures.Authenticate.Commands.RegisterSellerCommand;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiVersion("1.0")]
    public class AccountController : BaseApiController
    {
        public AccountController(IMediator mediator) : base(mediator)
        {
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
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
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
            }));
        }

        [Authorize(Roles = "Emprendedor")]
        [HttpPost("registerSeller")]
        public async Task<IActionResult> RegisterSellerAsync([FromBody] RegisterRequest request)
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
            }));
        }


        private string generateIpAddress()
        {
            if(Request.Headers.ContainsKey("X-Forwarded-For")) return Request.Headers["X-Forwarded-For"];
            else return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
