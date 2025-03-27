using Application.Feautures.StatsC.Sock.ABC;
using Application.Feautures.StatsC.Sock.Existencia;
using Application.Feautures.StatsC.Sock.Min_Max;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiVersion("1.0")]
    public class InventarioStatsController : BaseApiController
    {
        public InventarioStatsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("stockPorProductos")]
        public async Task<IActionResult> StockPorProductos([FromQuery] StockByProductosParameters filter)
        {
            return Ok(await Mediator.Send(
                new StockByProductos
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
                    FechaFin = filter.FechaFin,
                    CategoriaId = filter.CategoriaId
                }));
        }

        [HttpGet("IndicadoresExistencia")]
        public async Task<IActionResult> IndicadoresExistencia([FromQuery] Indicadores command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet("EntradaSalida")]
        public async Task<IActionResult> EntradaSalida([FromQuery] EntradaSalida command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet("RendimientoInventario")]
        public async Task<IActionResult> RendimientoInventario([FromQuery] RendimientoInventarioParameters command)
        {
            return Ok(await Mediator.Send(
                new RendimientoInventario
                {
                    PageNumber = command.PageNumber,
                    PageSize = command.PageSize,
                    NegocioId = command.NegocioId,
                    FechaInicio = command.FechaInicio,
                    FechaFin = command.FechaFin,
                    CategoriaId = command.CategoriaId
                }));
        }

        [HttpGet("Clasificacion")]
        public async Task<IActionResult> Clasificacion([FromQuery] ClasificacionParameters command)
        {
            return Ok(await Mediator.Send(
                new Clasificacion
                {
                    PageNumber = command.PageNumber,
                    PageSize = command.PageSize,
                    NegocioId = command.NegocioId,
                    FechaInicio = command.FechaInicio,
                    FechaFin = command.FechaFin,
                    categoriaId = command.categoriaId
                }));
        }

        [HttpGet("InvInicialPorMes")]
        public async Task<IActionResult> InvInicialPorMes([FromQuery] InvInicialPorMes command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet("CostoAlmacenadoPorMes")]
        public async Task<IActionResult> CostoAlmacenadoPorMes([FromQuery] CostoAlmacenadoPorMes command)
        {
            return Ok(await Mediator.Send(command));
        }


        [HttpGet("AnalisisPorMes")]
        public async Task<IActionResult> AnalisisPorMes([FromQuery] AnalisisPorMes command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
