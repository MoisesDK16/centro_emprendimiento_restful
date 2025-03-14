using Application.Feautures.PromocionC.Commands;
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
    }
}
