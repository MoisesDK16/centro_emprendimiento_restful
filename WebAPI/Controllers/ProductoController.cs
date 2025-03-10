using Application.Feautures.ProductoC.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiVersion("1.0")]
    public class ProductoController : BaseApiController
    {
        public ProductoController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("crear")]
        public async Task<IActionResult> CrearProducto([FromBody] CrearProducto comando)
        {
            return Ok(await Mediator.Send(comando));
        }

        [HttpPut("actualizar")]
        public async Task<IActionResult> ActualizarProducto([FromBody] ActualizarProducto comando)
        {
            return Ok(await Mediator.Send(comando));
        }
    }
}
