using Application.Feautures.CategoriaC.Queries;
using Application.Feautures.ProductoC.Commands;
using Application.Feautures.ProductoC.Queries;
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

        [HttpGet("listarProductos")]
         public async Task<IActionResult> ListarProductos([FromQuery] ListarProductosParameters filter)
        {
            var productos = await Mediator.Send(new ListarProductos
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                Categoria = filter.Categoria });

            return Ok(productos);
        }

        [HttpGet("productoById")]
        public async Task<IActionResult> ProductoById([FromQuery] ProductoById request)
        {
            var producto = await Mediator.Send(request);
            return Ok(producto);
        }
    }
}
