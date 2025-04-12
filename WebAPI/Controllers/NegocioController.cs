using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Feautures.NegocioC.Commands;
using Application.Feautures.NegocioC.Queries;
using Microsoft.AspNetCore.Authorization;
using Application.Services.NegocioS;
using System.Text;
using static Application.Feautures.NegocioC.Commands.CrearNegocio;
using static Application.Feautures.NegocioC.Commands.ActualizarNegocio;

namespace WebAPI.Controllers
{
    [ApiVersion("1.0")]
    public class NegocioController : BaseApiController
    {
        private readonly NegocioService _negocioService;
        public NegocioController(IMediator mediator, NegocioService negocioService) : base(mediator)
        {
            _negocioService = negocioService;
        }

        [Authorize(Roles = "Admin, Emprendedor")]
        [HttpPost("crear")]
        public async Task<IActionResult> CrearNegocio([FromBody] CrearNegocioParameters commando)
        {
            return Ok(await Mediator.Send(
                new CrearNegocio
                {
                    Nombre = commando.Nombre,
                    Telefono = commando.Telefono,
                    Direccion = commando.Direccion,
                    Descripcion = commando.Descripcion,
                    CategoriaId = commando.CategoriaId,
                    EmprendedorId = commando.EmprendedorId,
                    UserId = User.FindFirst("Id")?.Value
                }));
        }

        [Authorize(Roles = "Admin, Emprendedor")]
        [HttpPut("actualizar")]
        public async Task<IActionResult> ActualizarNegocio([FromBody] ActualizarNegocioParameters commando)
        {
            return Ok(await Mediator.Send(
                new ActualizarNegocio
                {
                    Id = commando.Id,
                    Nombre = commando.Nombre,
                    Telefono = commando.Telefono,
                    Direccion = commando.Direccion,
                    Descripcion = commando.Descripcion,
                    Estado = commando.Estado,
                    CategoriaId = commando.CategoriaId,
                    UserId = User.FindFirst("Id")?.Value
                }));
        }

        [Authorize(Roles = "Admin, Emprendedor")]
        [HttpGet("listar")]
        public async Task<IActionResult> ListarNegocios([FromQuery] ListarNegociosParameter filter)
        {
            return Ok(await Mediator.Send(
                new ListarNegocios
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Nombre = filter.Nombre,
                    Telefono = filter.Telefono,
                    Tipo = filter.Tipo,
                    Estado = filter.Estado,
                    EmprendedorId = filter.EmprendedorId
                }));
        }

        [Authorize(Roles = "Admin, Emprendedor")]
        [HttpGet("negocioById")]
        public async Task<IActionResult> NegocioById([FromQuery] long id)
        {
            return Ok(await Mediator.Send(new NegocioById { Id = id }));
        }

        [Authorize(Roles = "Admin, Emprendedor")]
        [HttpGet("negocioByTelefono")]
        public async Task<IActionResult> NegocioByTelefono([FromQuery] string telefono)
        {
            return Ok(await Mediator.Send(new NegocioByTelefono { telefono = telefono }));
        }

        [Authorize(Roles = "Emprendedor")]
        [HttpGet("selectNegocios")]
        public async Task<IActionResult> SelectNegocios([FromQuery] SelectNegocios selectNegocios)
        {
            return Ok(await Mediator.Send(selectNegocios));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("selectNegociosAdmin")]
        public async Task<IActionResult> SelectNegociosAdmin([FromQuery] SelectNegociosAdmin selectNegocios)
        {
            return Ok(await Mediator.Send(selectNegocios));
        }

        [HttpGet("aprobar")]
        [AllowAnonymous]
        public async Task<IActionResult> AprobarORechazarNegocio([FromQuery] long negocioId, [FromQuery] bool aprobado)
        {
            var mensaje = await _negocioService.DeterminarNegocio(negocioId, aprobado);
            return Content(mensaje, "text/html", Encoding.UTF8);
        }


    }
}
