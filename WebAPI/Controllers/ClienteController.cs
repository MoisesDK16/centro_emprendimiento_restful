using Application.Feautures.ClienteC.Commands;
using Application.Feautures.ClienteC.Queries;
using Application.Parameters;
using Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiVersion("1.0")]
    public class ClienteController : BaseApiController
    {
        public ClienteController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("crear")]
        public async Task<IActionResult> CrearCliente([FromBody] CrearCliente commando)
        {
            return Ok(await Mediator.Send(commando));
        }

        [HttpPut("actualizar")]
        public async Task<IActionResult> ActualizarCliente([FromBody] ActualizarCliente commando)
        {
            return Ok(await Mediator.Send(commando));
        }

        [HttpGet("listar")]
        public async Task<IActionResult> ListarClientes([FromQuery] ListarClientesParameters filter)
        {
            return Ok(await Mediator.Send(
                new ListarClientes { 
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Identificacion = filter.Identificacion,
                    Nombres = filter.Nombres,
                    PrimerApellido = filter.PrimerApellido,
                    Ciudad = filter.Ciudad 
                }));
        }

        [HttpGet("clienteById")]
        public async Task<IActionResult> ClienteById([FromQuery] long id)
        {
            return Ok(await Mediator.Send(new ClienteById { Id = id }));
        }

        [HttpGet("clienteByIdentificacion")]
        public async Task<IActionResult> ClienteByIdentificacion([FromQuery] string identificacion)
        {
            return Ok(await Mediator.Send(new ClienteByIdentificacion { Identificacion = identificacion }));
        }
    }
}
