using Application.DTOs.Ventas;
using Application.Feautures.VentaC.Commands;
using Application.Feautures.VentaC.Queries;
using Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class VentaController : BaseApiController
    {
        public VentaController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("crear")]
        public async Task<ActionResult<Response<long>>> CrearVenta(CrearVenta command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet("listar")]
        public async Task<ActionResult<PagedResponse<IEnumerable<GeneralVenta>>>> ListarVentas([FromQuery] ListarVentasParameters filter)
        {
            return await Mediator.Send(new ListarVentas
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                NegocioId = filter.NegocioId,
                IdentificacionCliente = filter.IdentificacionCliente,
                Fecha = filter.Fecha
            });
        }


    }
}
