using Application.Feautures.StatsC.Cuadros_Mando;
using Application.Feautures.StatsC.Ventas;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiVersion("1.0")]
    public class VentasStatsController : BaseApiController
    {
        public VentasStatsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("ventasPorNegocio")]
        public async Task<IActionResult> Ventas([FromQuery] TotalVentas totalVentas)
        {
            return Ok(await Mediator.Send(totalVentas));
        }

        [HttpGet("VentasMensuales")]
        public async Task<IActionResult> VentasMensuales([FromQuery] VentasMensuales command)
        {
            return Ok(await Mediator.Send(command));
        }



    }
}
