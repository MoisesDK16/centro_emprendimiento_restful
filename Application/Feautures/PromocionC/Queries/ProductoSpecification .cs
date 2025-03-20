using Ardalis.Specification;
using Domain.Entities;

namespace Application.Feautures.PromocionC.Queries
{
    public class ProductoSpecification : Specification<Producto>
    {
        public ProductoSpecification(long productId)
        {
            Query.Where(p => p.Id == productId);
        }

    }
}
