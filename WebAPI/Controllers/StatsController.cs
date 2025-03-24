using Application.Feautures.StatsC.Cuadros_Mando;
using Application.Feautures.StatsC.Ganancias;
using Application.Feautures.StatsC.Sock.ABC;
using Application.Feautures.StatsC.Sock.Existencia;
using Application.Feautures.StatsC.Sock.Min_Max;
using Application.Feautures.StatsC.Ventas;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiVersion("1.0")]
    public class StatsController : BaseApiController
    {
        public StatsController(IMediator mediator) : base(mediator)
        {
        }

        //Cuadros de Mando

        //Ventas y Ganancias

        //Inventario
        
    }
}
