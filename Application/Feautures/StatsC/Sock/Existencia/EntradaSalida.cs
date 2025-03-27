using Application.Interfaces;
using Application.Specifications;
using Domain.Entities;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feautures.StatsC.Sock.Existencia
{
    public class EntradaSalida : IRequest<IDictionary<int, int[]>>
    {
        public long NegocioId { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }

        public long CategoriaId { get; set; }
    }

    public class EntradaSalidaHandler : IRequestHandler<EntradaSalida, IDictionary<int, int[]>>
    {
        private readonly IReadOnlyRepositoryAsync<Detalle> _detalleRepository;
        private readonly IReadOnlyRepositoryAsync<Stock> _stockRepository;
        private readonly IReadOnlyRepositoryAsync<Producto> _productoRepository;

        public EntradaSalidaHandler(
            IReadOnlyRepositoryAsync<Detalle> detalleRepository,
            IReadOnlyRepositoryAsync<Stock> stockRepository,
            IReadOnlyRepositoryAsync<Producto> productoRepository)
        {
            _detalleRepository = detalleRepository;
            _stockRepository = stockRepository;
            _productoRepository = productoRepository;
        }

        public async Task<IDictionary<int, int[]>> Handle(EntradaSalida request, CancellationToken cancellationToken)
        {
            var result = new Dictionary<int, int[]>();

            var fechaInicio = request.FechaInicio.ToDateTime(TimeOnly.MinValue);
            var fechaFin = request.FechaFin.ToDateTime(TimeOnly.MaxValue);

            for (var fecha = fechaInicio; fecha <= fechaFin; fecha = fecha.AddMonths(1))
            {
                int mes = fecha.Month;
                var inicioMes = new DateOnly(fecha.Year, mes, 1);
                var finMes = new DateOnly(fecha.Year, mes, DateTime.DaysInMonth(fecha.Year, mes));

                // Cantidad de Salidas (Ventas)
                var detalles = await _detalleRepository.ListAsync(new DetalleSpecification(request.NegocioId, inicioMes, finMes, request.CategoriaId));
                int totalSalidas = detalles.Sum(d => d.Cantidad);

                // Cantidad de Entradas (Ingresos por compras al stock)
                var stocks = await _stockRepository.ListAsync(new StockSpecification(request.NegocioId, inicioMes, finMes, request.CategoriaId));
                int totalEntradas = stocks.Sum(s => s.Cantidad);

                result[mes] = new int[] { totalEntradas, totalSalidas };
            }

            return result;
        }
    }
}



