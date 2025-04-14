using Application.Feautures.ClienteC.Commands;
using Application.Feautures.ClienteC.Queries;
using Application.Parameters;
using Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiVersion("1.0")]
    public class ClienteController : BaseApiController
    {
        public ClienteController(IMediator mediator) : base(mediator)
        {
        }

        [Authorize(Roles = "Emprendedor,Vendedor")]
        [HttpPost("crear")]
        public async Task<IActionResult> CrearCliente([FromBody] CrearClienteParameters commando)
        {
            return Ok(await Mediator.Send(new CrearCliente
            {
                Identificacion = commando.Identificacion,
                Nombres = commando.Nombres,
                PrimerApellido = commando.PrimerApellido,
                SegundoApellido = commando.SegundoApellido,
                Email = commando.Email,
                Telefono = commando.Telefono,
                Ciudad = commando.Ciudad,
                Direccion = commando.Direccion,
                NegocioId = commando.NegocioId,
                UserId = User.FindFirst("uid")?.Value
            }));
        }

        [Authorize(Roles = "Emprendedor,Vendedor")]
        [HttpPut("actualizar")]
        public async Task<IActionResult> ActualizarCliente([FromBody] ActualizarClienteParameters commando)
        {
            return Ok(await Mediator.Send(new ActualizarCliente
            {
                Id = commando.Id,
                Identificacion = commando.Identificacion,
                Nombres = commando.Nombres,
                PrimerApellido = commando.PrimerApellido,
                SegundoApellido = commando.SegundoApellido,
                Email = commando.Email,
                Telefono = commando.Telefono,
                Ciudad = commando.Ciudad,
                Direccion = commando.Direccion
            }));
        }

        [Authorize(Roles = "Admin,Emprendedor,Vendedor")]
        [HttpGet("listar")]
        public async Task<IActionResult> ListarClientes([FromQuery] ListarClientesParameters filter)
        {
            return Ok(await Mediator.Send(
                new ListarClientes { 
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    NegocioId = filter.NegocioId,
                    Identificacion = filter.Identificacion,
                    Nombres = filter.Nombres,
                    PrimerApellido = filter.PrimerApellido,
                    Ciudad = filter.Ciudad,
                    UserId = User.FindFirst("uid")?.Value
                }));
        }

        [Authorize(Roles = "Admin,Emprendedor,Vendedor")]
        [HttpGet("clienteById")]
        public async Task<IActionResult> ClienteById([FromQuery] long id)
        {
            return Ok(await Mediator.Send(new ClienteById { Id = id }));
        }

        [Authorize(Roles = "Admin,Emprendedor,Vendedor")]
        [HttpGet("clienteByIdentificacion")]
        public async Task<IActionResult> ClienteByIdentificacion([FromQuery] string identificacion)
        {
            return Ok(await Mediator.Send(new ClienteByIdentificacion { 
                Identificacion = identificacion,
                UserId = User.FindFirst("uid")?.Value
            }));
        }
    }
}
