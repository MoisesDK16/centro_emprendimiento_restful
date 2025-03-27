using Application.DTOs.Stats.Stock.Min_Max;
using Application.Interfaces;
using Application.Specifications;
using Application.Wrappers;
using Domain.Entities;
using MediatR;

namespace Application.Feautures.StatsC.Sock.Min_Max
{
    public class CostoAlmacenadoPorMes : IRequest<Response<List<CostoAlmacenadoPorMesDTO>>>
    {
        public long NegocioId { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
        public long CategoriaId { get; set; }
    }

    public class CostoAlmacenadoPorMesHandler : IRequestHandler<CostoAlmacenadoPorMes, Response<List<CostoAlmacenadoPorMesDTO>>>
    {

        private readonly IReadOnlyRepositoryAsync<Historial_Stock> _historialRepository;

        public CostoAlmacenadoPorMesHandler(IReadOnlyRepositoryAsync<Historial_Stock> historialRepository)
        {
            _historialRepository = historialRepository;
        }

        public async Task<Response<List<CostoAlmacenadoPorMesDTO>>> Handle(CostoAlmacenadoPorMes request, CancellationToken cancellationToken)
        {
            var mesesConDatos = new Dictionary<int, decimal>();

            for (var fecha = request.FechaInicio; fecha <= request.FechaFin; fecha = fecha.AddMonths(1))
            {
                var finMes = new DateOnly(fecha.Year, fecha.Month, DateTime.DaysInMonth(fecha.Year, fecha.Month));

                var stock = await _historialRepository.ListAsync(
                    new HistorialStockSpecification(request.NegocioId, finMes, request.CategoriaId, true));

                mesesConDatos[fecha.Month] = stock.Sum(s => s.CostoTotal);
            }

            var resultado = new List<CostoAlmacenadoPorMesDTO>();

            for (var fecha = request.FechaInicio; fecha <= request.FechaFin; fecha = fecha.AddMonths(1))
            {
                var mes = fecha.Month;

                resultado.Add(new CostoAlmacenadoPorMesDTO
                {
                    Mes = mes,
                    Costo = mesesConDatos.ContainsKey(mes) ? mesesConDatos[mes] : 0m
                });
            }

            return new Response<List<CostoAlmacenadoPorMesDTO>>(resultado);
        }

    }
}
