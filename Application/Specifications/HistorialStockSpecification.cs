using Ardalis.Specification;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uno.Extensions.ValueType;

namespace Application.Specifications
{
    public class HistorialStockSpecification : Specification<Historial_Stock>
    {
        public HistorialStockSpecification(long negocioId, DateOnly fechaInicio, bool begin)
        {
            var ultimoDiaDelMes = new DateOnly(
                fechaInicio.Year,
                fechaInicio.Month,
                DateTime.DaysInMonth(fechaInicio.Year, fechaInicio.Month)
            );

            Query.Where(h =>
                h.NegocioId == negocioId &&
                h.FechaCorte >= fechaInicio &&
                h.FechaCorte <= ultimoDiaDelMes
            );
        }

        public HistorialStockSpecification(long negocioId, DateOnly fechaInicio, DateOnly fechaFin)
        {
            Query.Where(h =>
                h.NegocioId == negocioId &&
                h.FechaInicio >= fechaInicio && h.FechaCorte <= fechaFin
            );
        }


        public HistorialStockSpecification(long negocioId, DateOnly fechaFin)
        {
            Query.Where(h => h.NegocioId == negocioId && h.FechaCorte == fechaFin);
        }

        public HistorialStockSpecification(long negocioId)
        {
            Query.Where(h => h.NegocioId == negocioId);
        }

    }
}
