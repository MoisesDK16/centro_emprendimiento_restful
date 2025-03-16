using Ardalis.Specification;
using Domain.Entities;
using Domain.Enums.Promocion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Specifications
{
    public class PromocionSpecification : Specification<Promocion>
    {
        public PromocionSpecification(int PageNumber,int PageSize, long negocioId)
        {
            Query.Where(p => p.NegocioId == negocioId);
            Query.Skip((PageNumber - 1) * PageSize);
            Query.Take(PageSize);
        }

        public PromocionSpecification(long productoId)
        {
            Query
                .Where(p => p.Productos.Any(x => x.Id == productoId));
        }

    }
}
