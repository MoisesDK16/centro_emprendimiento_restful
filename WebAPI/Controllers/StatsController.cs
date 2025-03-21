using Application.Feautures.StatsC;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiVersion("1.0")]
    public class StatsController : BaseApiController
    {
        public StatsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("stockPorProductos")]
        public async Task<IActionResult> StockPorProductos([FromQuery] StockByProductosParameters filter)
        {
            return Ok(await Mediator.Send(
                new StockByProductos 
                { PageSize = filter.PageSize,
                    PageNumber = filter.PageNumber,
                    NegocioId = filter.NegocioId,
                    CategoriaId = filter.CategoriaId }));
        }

        [HttpGet("gananciasPorNegocio")]
        public async Task<IActionResult> Ganancias([FromQuery] TotalGanancias totalGanancias)
        {
            return Ok(await Mediator.Send(totalGanancias));
        }

        [HttpGet("ventasPorNegocio")]
        public async Task<IActionResult> Ventas([FromQuery] TotalVentas totalVentas)
        {
            return Ok(await Mediator.Send(totalVentas));
        }
    }
}
