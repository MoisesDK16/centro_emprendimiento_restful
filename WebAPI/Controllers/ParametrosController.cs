using Application.Feautures.ParametrosC.Commands;
using Application.Feautures.ParametrosC.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiVersion("1.0")]
    public class ParametrosController : BaseApiController
    {
        public ParametrosController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarParametro([FromBody] ActualizarParametro command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet]
        public async Task<IActionResult> ListarParametros()
        {
            return Ok(await Mediator.Send(new ListarParametros()));
        }
    }
}
