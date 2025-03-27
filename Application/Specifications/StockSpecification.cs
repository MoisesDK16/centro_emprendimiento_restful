using Application.Feautures.Negocio.Queries;
using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specifications
{
    public class StockSpecification : Specification<Stock>
    {
        public StockSpecification(long negocioId, long? productoId = null, int? cantidad = null, DateOnly? fechaCaducidad = null)
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
        }

        public StockSpecification(long productoId, long stockId)
        {
            Query.Where(stock => stock.ProductoId == productoId && stock.Id == stockId);
        }

        public StockSpecification(long negocioId, long categoriaId, bool byCategory)
        {
            Query.Include(s => s.Producto)
                .ThenInclude(p => p.Categoria)
                .Where(stock => stock.Producto.NegocioId == negocioId);

            if (categoriaId > 0)
                Query.Where(stock => stock.Producto.CategoriaId == categoriaId);
        }


        public StockSpecification(long NegocioId, long ProductoId, long stockId)
        {
            Query.Include(s => s.Producto)
                .Include(s => s.Producto.Promocion)
                .Where(
                stock => stock.ProductoId == ProductoId &&
                stock.Producto.NegocioId == NegocioId &&
                stock.Id == stockId);
        }

        public StockSpecification(long negocioId, DateOnly FechaInicio, DateOnly FechaFin, long? categoriaId)
        {
            Query.Include(s => s.Producto)
                .ThenInclude(p => p.Negocio)
                .Where(stock => stock.Producto.NegocioId == negocioId &&
                DateOnly.FromDateTime(stock.FechaIngreso) >= FechaInicio &&
                DateOnly.FromDateTime(stock.FechaIngreso) <= FechaFin);

            if (categoriaId > 0)
                Query.Where(stock => stock.Producto.CategoriaId == categoriaId);
        }

        public StockSpecification(long negocioId, List<long> productoIds, DateOnly FechaInicio, DateOnly FechaFin)
        {
            Query.Include(s => s.Producto)
                .ThenInclude(p => p.Negocio)
                .Where(stock => stock.Producto.NegocioId == negocioId &&
                productoIds.Contains(stock.ProductoId) &&
                DateOnly.FromDateTime(stock.FechaIngreso) >= FechaInicio &&
                DateOnly.FromDateTime(stock.FechaIngreso) <= FechaFin);
        }

        public StockSpecification(long stockId)
        {
            Query.Where(stock => stock.Id == stockId);
        }

        public StockSpecification (long productId, bool producto)
        {
            Query.Include(s => s.Producto)
                .Where(stock => stock.ProductoId == productId);
        }

    }
}
