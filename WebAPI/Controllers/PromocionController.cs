using Application.Feautures.PromocionC.Commands;
using Application.Feautures.PromocionC.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiVersion("1.0")]
    public class PromocionController : BaseApiController
    {
        public PromocionController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("crear")]
        public async Task<IActionResult> CrearPromocion([FromBody] CrearPromocion command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut("actualizar")]
        public async Task<IActionResult> EditarPromocion([FromBody] ActualizarPromocion command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet("listarPromociones")]
        public async Task<IActionResult> ListarPromociones([FromQuery] ListarPromocionesParameters filter)
        {
            return Ok(await Mediator.Send(
                new ListarPromociones{
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    NegocioId = filter.NegocioId 
                }));
        }

        [HttpGet("ById")]   
        public async Task<IActionResult> PromocionById([FromQuery] long Id)
        {
            return Ok(await Mediator.Send(
                new PromocionById
                {
                    Id = Id
                }));
        }
    }
}
