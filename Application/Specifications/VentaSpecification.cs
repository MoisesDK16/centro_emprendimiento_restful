using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specifications
{
    public class VentaSpecification : Specification<Venta>
    {
        public VentaSpecification(int PageNumber, int PageSize, long NegocioId, string? IdentificacionCliente, DateOnly? Fecha)
        {
            Query
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .Include(x => x.Cliente)
                .Where(x => x.NegocioId == NegocioId);

            if (!string.IsNullOrEmpty(IdentificacionCliente)) { Query.Where(x => x.Cliente.Identificacion == IdentificacionCliente); }
            if (Fecha.HasValue)
            {
                Query.Where(x => DateOnly.FromDateTime(x.Fecha) == Fecha);
            }
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
