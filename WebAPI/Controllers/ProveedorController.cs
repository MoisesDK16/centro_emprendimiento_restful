using Application.Features.Proveedores.Commands.DeleteProveedorCommand;
using Application.Features.Proveedores.Commands.UpdateProveedorCommand;
using Application.Feautures.Proveedores.Commands.CreateProveedorCommand;
using Application.Feautures.Proveedores.Queries.GetAll;
using Application.Feautures.Proveedores.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiVersion("1.0")]
    public class ProveedorController : BaseApiController
    {
        public ProveedorController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProveedorCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateProveedorCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            return Ok(await Mediator.Send(new DeleteProveedorCommand { Id = id }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] long id)
        {
            return Ok(await Mediator.Send(new GetProveedorById{ Id = id }));
        }

        [HttpGet]
        [Authorize(Roles = "Emprendedor")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllProveedorParameters filter)
        {
            return Ok(await Mediator.Send(
                new GetAllProveedor { 
                    PageSize = filter.PageSize,
                    PageNumber = filter.PageNumber,
                    ruc = filter.ruc 
                }));

        }
    }
}
