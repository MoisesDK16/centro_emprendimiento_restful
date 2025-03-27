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

        public HistorialStockSpecification(long negocioId, DateOnly fechaInicio,DateOnly fechaFin, long categoriaId)
        {
            Query.Include(h => h.Producto);

            Query
                .Where(h =>
                h.NegocioId == negocioId &&
                h.FechaInicio == fechaInicio &&
                h.FechaCorte == fechaFin
            );

            if (categoriaId != 0)
            {
                Query.Where(h => h.Producto.CategoriaId == categoriaId);
            }
        }

        public HistorialStockSpecification(long negocioId, DateOnly fechaInicio, long categoriaId)
        {
            Query.Where(h =>
                h.NegocioId == negocioId &&
                h.FechaInicio == fechaInicio
            );

            if (categoriaId != 0)
            {
                Query.Where(h => h.Producto.CategoriaId == categoriaId);
            }
        }

        public HistorialStockSpecification(long negocioId, DateOnly fechaInicio, DateOnly fechaFin)
        {
            Query.Where(h =>
                h.NegocioId == negocioId &&
                h.FechaInicio >= fechaInicio && h.FechaCorte <= fechaFin
            );
        }


        public HistorialStockSpecification(long negocioId, DateOnly fechaFin, long categoriaId, bool isFin)
        {
            Query.Where(h =>
                h.NegocioId == negocioId &&
                h.FechaCorte == fechaFin
            );

            if (categoriaId != 0)
            {
                Query.Where(h => h.Producto.CategoriaId == categoriaId);
            }
        }

        public HistorialStockSpecification(long negocioId)
        {
            Query.Where(h => h.NegocioId == negocioId);
        }

    }
}
