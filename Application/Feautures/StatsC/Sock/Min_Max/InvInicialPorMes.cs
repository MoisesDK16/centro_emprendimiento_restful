using Application.DTOs.Stats.Stock.Min_Max;
using Application.Interfaces;
using Application.Specifications;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feautures.StatsC.Sock.Min_Max
{
    public class InvInicialPorMes : IRequest<Response<List<InvInicialPorMesDTO>>>
    {
        public long NegocioId { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
        public long CategoriaId { get; set; }
    }


    public class InvInicialPorMesHandler : IRequestHandler<InvInicialPorMes, Response<List<InvInicialPorMesDTO>>>
    {

        private readonly IReadOnlyRepositoryAsync<Historial_Stock> _historialRepository;

        public InvInicialPorMesHandler(IReadOnlyRepositoryAsync<Historial_Stock> historialRepository)
        {
            _historialRepository = historialRepository;
        }

        public async Task<Response<List<InvInicialPorMesDTO>>> Handle(InvInicialPorMes request, CancellationToken cancellationToken)
        {
            var historial_Stocks = new List<Historial_Stock>();
            var mesesConDatos = new Dictionary<int, int>();

            for (var fecha = request.FechaInicio; fecha <= request.FechaFin; fecha = fecha.AddMonths(1))
            {
                var inicioMes = new DateOnly(fecha.Year, fecha.Month, 1);

                var stockInicial = await _historialRepository.ListAsync(
                    new HistorialStockSpecification(request.NegocioId, inicioMes, request.CategoriaId));

                var cantidad = stockInicial.Sum(s => s.Existencias);
                mesesConDatos[fecha.Month] = cantidad;
            }


            var historialList = new List<InvInicialPorMesDTO>();

            for (var fecha = request.FechaInicio; fecha <= request.FechaFin; fecha = fecha.AddMonths(1))
            {
                var mes = fecha.Month;
                historialList.Add(new InvInicialPorMesDTO
                {
                    Mes = mes,
                    Cantidad = mesesConDatos.ContainsKey(mes) ? mesesConDatos[mes] : 0
                });
            }

            return new Response<List<InvInicialPorMesDTO>>(historialList);
        }

    }
}
