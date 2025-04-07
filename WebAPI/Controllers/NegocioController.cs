using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Feautures.NegocioC.Commands;
using Application.Feautures.NegocioC.Queries;
using Microsoft.AspNetCore.Authorization;
using Application.Services.NegocioS;
using System.Text;

namespace WebAPI.Controllers
{
    [ApiVersion("1.0")]
    public class NegocioController : BaseApiController
    {
        private readonly NegocioService _negocioService;
        public NegocioController(IMediator mediator, NegocioService negocioService) : base(mediator)
        {
            _negocioService = negocioService;
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


        [HttpGet("aprobar")]
        [AllowAnonymous]
        public async Task<IActionResult> AprobarORechazarNegocio([FromQuery] long negocioId, [FromQuery] bool aprobado)
        {
            var mensaje = await _negocioService.DeterminarNegocio(negocioId, aprobado);
            return Content(mensaje, "text/html", Encoding.UTF8);
        }


    }
}
