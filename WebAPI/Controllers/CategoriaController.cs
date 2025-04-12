using Application.Feautures.Categoria.Queries;
using Application.Feautures.CategoriaC.Commands;
using Application.Feautures.CategoriaC.Queries;
using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiVersion("1.0")]
    public class CategoriaController : BaseApiController
    {
        public CategoriaController(IMediator mediator) : base(mediator)
        {
        }

        [Authorize(Roles = "Admin,Emprendedor")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CrearCategoriaComando command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,Emprendedor,Vendedor")]
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] ListarCategoriasParameters filter)
        {
            var command = new ListarCategorias
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                Tipo = filter.Tipo,
                NegocioId = filter.NegocioId
            };
            var listaCategorias = await Mediator.Send(command);
            return Ok(listaCategorias);
        }

        [Authorize(Roles = "Admin,Emprendedor,Vendedor")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] long id)
        {
            var query = new CategoriaById { Id = id };
            var categoria = await Mediator.Send(query);
            return Ok(categoria);
        }

        [Authorize(Roles = "Admin,Emprendedor,Vendedor")]
        [HttpGet("select-categorias")]
        public async Task<IActionResult> SelectCategorias([FromQuery] SelectCategorias select)
        {
            var categorias = await Mediator.Send(select);
            return Ok(categorias);
        }
    }
}
