using Ardalis.Specification;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Specifications
{
    public class DetalleSpecification : Specification<Detalle>
    {
        public DetalleSpecification(long negocioId)
        {
            Query.Include(d => d.Stock)
                 .Include(d => d.Producto)
                 .Include(d => d.Venta)
                 .ThenInclude(v => v.Negocio)
                 .Where(d => d.Venta.NegocioId == negocioId);
        }

        public DetalleSpecification(long negocioId, DateOnly fechaInicio, DateOnly fechaFin)
        {
            Query.Include(d => d.Stock) 
                 .Include(d => d.Producto)
                 .Include(d => d.Venta)
                 .ThenInclude(v => v.Negocio)
                 .Where(d => d.Venta.NegocioId == negocioId)
                 .Where(d => DateOnly.FromDateTime(d.Venta.Fecha) >= fechaInicio
                          && DateOnly.FromDateTime(d.Venta.Fecha) <= fechaFin);
        }

        public DetalleSpecification(int pageNumber, int pageSize, long negocioId, DateOnly fechaInicio, DateOnly fechaFin, long categoriaId)
        {
            Query
                .Include(d => d.Producto)
                .Include(d => d.Stock)
                .Include(d => d.Venta) 
                .ThenInclude(v => v.Negocio) 
                .Where(d => d.Venta.NegocioId == negocioId); 

            if (fechaInicio != default && fechaFin != default)
            {
                Query.Where(d => d.Venta.Fecha >= fechaInicio.ToDateTime(TimeOnly.MinValue)
                              && d.Venta.Fecha <= fechaFin.ToDateTime(TimeOnly.MaxValue));
            }

           Query.Where(d => d.Producto.CategoriaId == categoriaId);

           Query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

    }
}
