using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specifications
{
    public class StockSpecification : Specification<Stock>
    {
        public StockSpecification(int pageNumber, int pageSize, long negocioId, long? productoId = null, int? cantidad = null, DateOnly? fechaCaducidad = null)
        {
            Query.Include(s => s.Producto)
                 .ThenInclude(p => p.Negocio) 
                 .Where(stock => stock.Producto != null && stock.Producto.NegocioId == negocioId);

            if (productoId.HasValue)
                Query.Where(stock => stock.ProductoId == productoId.Value);

            if (cantidad.HasValue)
                Query.Where(stock => stock.Cantidad == cantidad.Value);

            if (fechaCaducidad.HasValue)
                Query.Where(stock => stock.FechaCaducidad == fechaCaducidad.Value);

            Query.Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize);
        }

        public StockSpecification(long productoId)
        {
            Query.Where(stock => stock.ProductoId == productoId);
        }
    }
}
