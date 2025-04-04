using Application.Feautures.PromocionC.Commands;
using Application.Feautures.PromocionC.Queries;
using Domain.Settings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [ApiVersion("1.0")]
    public class PromocionController : BaseApiController
    {
        public PromocionController(IMediator mediator) : base(mediator)
        {
        }

        [Authorize(Roles = "Emprendedor")]
        [HttpPost("crear")]
        public async Task<IActionResult> CrearPromocion([FromBody] CrearPromocion command)
        {
            return Ok(await Mediator.Send(
                new CrearPromocion
                {
                    TipoPromocion = command.TipoPromocion,
                    FechaInicio = command.FechaInicio,
                    FechaFin = command.FechaFin,
                    Descuento = command.Descuento,
                    CantidadCompra = command.CantidadCompra,
                    CantidadGratis = command.CantidadGratis,
                    IdProductos = command.IdProductos,
                    NegocioId = command.NegocioId,
                    UserId = User.FindFirst("uid")?.Value
                }));
        }

        [Authorize(Roles = "Emprendedor")]
        [HttpPut("actualizar")]
        public async Task<IActionResult> EditarPromocion([FromBody] ActualizarPromocion command)
        {
            return Ok(await Mediator.Send(
                new ActualizarPromocion
                {
                    Id = command.Id,
                    FechaInicio = command.FechaInicio,
                    FechaFin = command.FechaFin,
                    Descuento = command.Descuento,
                    CantidadCompra = command.CantidadCompra,
                    CantidadGratis = command.CantidadGratis,
                    IdProductos = command.IdProductos,
                    NegocioId = command.NegocioId,
                    UserId = User.FindFirst("uid")?.Value
                }
                ));
        }

        [Authorize(Roles = "Emprendedor,Vendedor")]
        [HttpGet("listarPromociones")]
        public async Task<IActionResult> ListarPromociones([FromQuery] ListarPromocionesParameters filter)
        {
            return Ok(await Mediator.Send(
                new ListarPromociones{
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    NegocioId = filter.NegocioId,
                    UserId = User.FindFirst("uid")?.Value,
                }));
        }

        [Authorize(Roles = "Emprendedor,Vendedor")]
        [HttpGet("ById")]   
        public async Task<IActionResult> PromocionById([FromQuery] PromocionByIdParameters request)
        {
            return Ok(await Mediator.Send(
                new PromocionById
                {
                    Id = request.Id,
                    NegocioId = request.NegocioId,
                    UserId = User.FindFirst("uid")?.Value
                }));
        }
    }
}
