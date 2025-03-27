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
            Console.WriteLine("FechaInicio: " + fechaInicio);
            Console.WriteLine("FechaFin: " + fechaFin);

            Query.Include(d => d.Stock) 
                 .Include(d => d.Venta)
                 .Include(d => d.Venta.Cliente)
                 .Include(d => d.Producto)
                 .Include(d => d.Producto.Categoria)
                 .ThenInclude(v => v.Negocio)
                 .Where(d => d.Venta.NegocioId == negocioId)
                 .Where(d => DateOnly.FromDateTime(d.Venta.Fecha) >= fechaInicio
                          && DateOnly.FromDateTime(d.Venta.Fecha) <= fechaFin);
        }

        public DetalleSpecification(long negocioId, DateOnly fechaInicio, DateOnly fechaFin, long? categoriaId)
        {
            Query.Include(d => d.Stock)
                 .Include(d => d.Producto)
                 .Include(d => d.Producto.Categoria)
                 .Include(d => d.Venta)
                 .ThenInclude(v => v.Negocio)
                 .Where(d => d.Venta.NegocioId == negocioId)
                 .Where(d => DateOnly.FromDateTime(d.Venta.Fecha) >= fechaInicio
                          && DateOnly.FromDateTime(d.Venta.Fecha) <= fechaFin);

            if (categoriaId > 0)
            {
                Query.Where(d => d.Producto.CategoriaId == categoriaId);
            }
        }

    }
}
