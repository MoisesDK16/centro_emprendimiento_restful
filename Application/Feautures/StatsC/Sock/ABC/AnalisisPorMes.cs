using Application.DTOs.Stats.Stock.ABC;
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

namespace Application.Feautures.StatsC.Sock.ABC
{
    public class AnalisisPorMes : IRequest<Response<List<AnalisisPorMesDTO>>>
    {
        public long NegocioId { get; set; }
        public long categoriaId { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
    }

    public class AnalisisPorMesHandler : IRequestHandler<AnalisisPorMes, Response<List<AnalisisPorMesDTO>>>
    {
        private readonly IReadOnlyRepositoryAsync<Historial_Stock> _repostoryHistorical;
        private readonly IReadOnlyRepositoryAsync<Detalle> _repositoryDetalle;

        public AnalisisPorMesHandler(
            IReadOnlyRepositoryAsync<Historial_Stock> repostoryHistorical,
            IReadOnlyRepositoryAsync<Detalle> repositoryDetalle)
        {
            _repostoryHistorical = repostoryHistorical;
            _repositoryDetalle = repositoryDetalle;
        }

        public async Task<Response<List<AnalisisPorMesDTO>>> Handle(AnalisisPorMes request, CancellationToken cancellationToken)
        {
            var detalles = await _repositoryDetalle.ListAsync(
                new DetalleSpecification(request.NegocioId, request.FechaInicio, request.FechaFin, request.categoriaId));

            var historial = await _repostoryHistorical.ListAsync(
                new HistorialStockSpecification(request.NegocioId));

            var fechaInicio = request.FechaInicio.ToDateTime(TimeOnly.MinValue);
            var fechaFin = request.FechaFin.ToDateTime(TimeOnly.MaxValue);

            var resultados = new List<AnalisisPorMesDTO>();

            for (var fecha = fechaInicio; fecha <= fechaFin; fecha = fecha.AddMonths(1))
            {
                int mes = fecha.Month;
                var inicioMes = new DateOnly(fecha.Year, mes, 1);
                var finMes = new DateOnly(fecha.Year, mes, DateTime.DaysInMonth(fecha.Year, mes));

                // Inventario inicial: registros del último día del mes anterior
                var fechaInicioInv = inicioMes.AddDays(-1);
                var invInicial = historial
                    .Where(h => h.FechaCorte == fechaInicioInv && h.NegocioId == request.NegocioId)
                    .Sum(h => h.CostoTotal);

                // Inventario final: registros del último día del mes actual
                var invFinal = historial
                    .Where(h => h.FechaCorte == finMes && h.NegocioId == request.NegocioId)
                    .Sum(h => h.CostoTotal);

                var invPromedio = Math.Round((invInicial + invFinal) / 2, 2);

                var ventasMes = detalles
                    .Where(d =>
                        DateOnly.FromDateTime(d.Venta.Fecha).Month == mes &&
                        DateOnly.FromDateTime(d.Venta.Fecha).Year == fecha.Year)
                    .Sum(d => d.Total);

                resultados.Add(new AnalisisPorMesDTO
                {
                    Mes = mes,
                    CostoInvInicial = Math.Round(invInicial, 2),
                    CostoInvFinal = Math.Round(invFinal, 2),
                    CostoInvPromedio = invPromedio,
                    TotalVentas = Math.Round(ventasMes, 2)
                });
            }

            return new Response<List<AnalisisPorMesDTO>>(resultados);
        }

    }
}
