using Application.Feautures.StatsC.Cuadros_Mando;
using Application.Feautures.StatsC.Ganancias;
using Application.Feautures.StatsC.Sock.Existencia;
using Application.Feautures.StatsC.Sock.Min_Max;
using Application.Feautures.StatsC.Ventas;
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

        [HttpGet("gananciasPorProductos")]
        public async Task<IActionResult> GananciasPorProductos([FromQuery] GananciasPorProductosParameters filter)
        {
            return Ok(await Mediator.Send(
                new GananciasPorProductos
                {
                    PageSize = filter.PageSize,
                    PageNumber = filter.PageNumber,
                    NegocioId = filter.NegocioId,
                    CategoriaId = filter.CategoriaId
                }));
        }

        [HttpGet("EstadoInventario")]
        public async Task<IActionResult> EstadoInventario([FromQuery] EstadoInventarioParameters filter)
        {
            return Ok(await Mediator.Send(
                new EstadoInventario
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    NegocioId = filter.NegocioId,
                    FechaInicio = filter.FechaInicio,
                    FechaFin = filter.FechaFin
                }));
        }

        [HttpGet("VentasMensuales")]
        public async Task<IActionResult> VentasMensuales([FromQuery] VentasMensuales command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
