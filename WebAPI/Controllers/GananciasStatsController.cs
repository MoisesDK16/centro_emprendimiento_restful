using Application.Feautures.StatsC.Cuadros_Mando;
using Application.Feautures.StatsC.Ganancias;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Application.Feautures.StatsC.Ganancias.GananciasPorCategoriaHandler;

namespace WebAPI.Controllers
{
    [ApiVersion("1.0")]
    public class GananciasStatsController : BaseApiController
    {
        public GananciasStatsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("gananciasPorNegocio")]
        public async Task<IActionResult> Ganancias([FromQuery] TotalGanancias totalGanancias)
        {
            return Ok(await Mediator.Send(totalGanancias));
        }

        [HttpGet("gananciasPorProductos")]
        public async Task<IActionResult> GananciasPorProductos([FromQuery] GananciasPorProductosParameters filter)
        {
            return Ok(await Mediator.Send(
                new GananciasPorProductos
                {
                    PageSize = filter.PageSize,
                    PageNumber = filter.PageNumber,
                    FechaInicio = filter.FechaInicio,
                    FechaFin = filter.FechaFin,
                    NegocioId = filter.NegocioId,
                    CategoriaId = filter.CategoriaId
                }));
        }

        [HttpGet("gananciasPorCategoria")]
        public async Task<IActionResult> GananciasPorCategoria([FromQuery] GananciasPorCategoriaParameters filter)
        {
            return Ok(await Mediator.Send(
                new GananciasPorCategoria
                {
                    PageSize = filter.PageSize,
                    PageNumber = filter.PageNumber,
                    NegocioId = filter.NegocioId,
                    FechaInicio = filter.FechaInicio,
                    FechaFin = filter.FechaFin,
                    CategoriaId = filter.CategoriaId
                }));
        }

        [HttpGet("MejoresClientes")]
        public async Task<IActionResult> MejoresClientes([FromQuery] MejoresClientes command)
        {
            return Ok(await Mediator.Send(
                new MejoresClientes
                {
                    NegocioId = command.NegocioId,
                    FechaInicio = command.FechaInicio,
                    FechaFin = command.FechaFin
                }));
        }

    }
}
