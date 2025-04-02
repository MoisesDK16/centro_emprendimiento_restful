using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Feautures.NegocioC.Commands;
using Application.Feautures.NegocioC.Queries;

namespace WebAPI.Controllers
{
    [ApiVersion("1.0")]
    public class NegocioController : BaseApiController
    {
        public NegocioController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("crear")]
        public async Task<IActionResult> CrearNegocio([FromBody] CrearNegocio commando)
        {
            return Ok(await Mediator.Send(commando));
        }

        [HttpPut("actualizar")]
        public async Task<IActionResult> ActualizarNegocio([FromBody] ActualizarNegocio commando)
        {
            return Ok(await Mediator.Send(commando));
        }

        [HttpGet("listar")]
        public async Task<IActionResult> ListarNegocios([FromQuery] ListarNegociosParameter filter)
        {
            return Ok(await Mediator.Send(
                new ListarNegocios
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Nombre = filter.Nombre,
                    Telefono = filter.Telefono,
                    Tipo = filter.Tipo,
                    Estado = filter.Estado,
                    EmprendedorId = filter.EmprendedorId
                }));
        }

        [HttpGet("negocioById")]
        public async Task<IActionResult> NegocioById([FromQuery] long id)
        {
            return Ok(await Mediator.Send(new NegocioById { Id = id }));
        }

        [HttpGet("negocioByTelefono")]
        public async Task<IActionResult> NegocioByTelefono([FromQuery] string telefono)
        {
            return Ok(await Mediator.Send(new NegocioByTelefono { telefono = telefono }));
        }

        [HttpGet("selectNegocios")]
        public async Task<IActionResult> SelectNegocios([FromQuery] SelectNegocios selectNegocios)
        {
            return Ok(await Mediator.Send(selectNegocios));
        }
    }
}
