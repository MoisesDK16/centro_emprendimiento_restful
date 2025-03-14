using Ardalis.Specification;
using Domain.Entities;

namespace Application.Feautures.PromocionC.Queries
{
    public class ProductoSpecification : Specification<Producto>
    {
        public ProductoSpecification(List<long> idProductos)
        {
            Query.Where(p => idProductos.Contains(p.Id));
        }
    }
}
