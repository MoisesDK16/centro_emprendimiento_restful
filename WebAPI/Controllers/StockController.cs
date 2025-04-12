using Application.Feautures.StockC.Commands;
using Application.Feautures.StockC.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiVersion("1.0")]
    public class StockController : BaseApiController
    {
        public StockController(IMediator mediator): base(mediator)
        {
        }

        [Authorize(Roles = "Emprendedor, Vendedor")]
        [HttpPost("crear")]
        public async Task<IActionResult> Create([FromBody] CrearStock command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [Authorize(Roles = "Emprendedor, Vendedor")]
        [HttpPut("actualizar")]
        public async Task<IActionResult> Update([FromBody] ActualizarStock command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [Authorize(Roles = "Emprendedor, Vendedor")]
        [HttpGet("listar")]
        public async Task<IActionResult> List([FromQuery] ListarStockParameters filter)
        {
            var result = await Mediator.Send(new ListarStock
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                NegocioId = filter.NegocioId,
                ProductoId = filter.ProductoId,
                Cantidad = filter.Cantidad,
                FechaCaducidad = filter.FechaCaducidad
            });
            return Ok(result);
        }

        [Authorize(Roles = "Emprendedor, Vendedor")]
        [HttpGet("stockById")]
        public async Task<IActionResult> StockById([FromQuery] StockById request)
        {
            var result = await Mediator.Send(request);
            return Ok(result);
        }
    }
}
