using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specifications
{
    public class VentaSpecification : Specification<Venta>
    {
        public VentaSpecification(long NegocioId, string? IdentificacionCliente, DateOnly FechaInicio, DateOnly FechaFin)
        {
            Query
                .Include(x => x.Cliente)
                .Where(x => x.NegocioId == NegocioId);

            Query.Where(x => DateOnly.FromDateTime(x.Fecha) >= FechaInicio && DateOnly.FromDateTime(x.Fecha) <= FechaFin);

            if (!string.IsNullOrEmpty(IdentificacionCliente)) { Query.Where(x => x.Cliente.Identificacion == IdentificacionCliente); }

           
        }

        public VentaSpecification(long ventaId)
        {
            Query
                .Include(x => x.Cliente)
                .Include(x => x.Detalles)
                    .ThenInclude(d => d.Producto) 
                        .ThenInclude(p => p.Categoria) 
                .Include(x => x.Detalles)
                    .ThenInclude(d => d.Promocion) 
                .Include(x => x.Negocio)
                .Where(x => x.Id == ventaId);
        }

    }
}
