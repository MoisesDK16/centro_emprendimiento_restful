using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BaseApiController : Controller
    {
        private readonly IMediator _mediator;

        public BaseApiController(IMediator mediator) // 🔹 Inyectar IMediator correctamente
        {
            _mediator = mediator;
        }

        protected IMediator Mediator => _mediator;
    }
}
