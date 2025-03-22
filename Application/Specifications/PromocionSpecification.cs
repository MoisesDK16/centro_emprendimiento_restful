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

        public PromocionSpecification(long productoId, long promocionId)
        {
            Query
                .Where(p => p.Id == promocionId && p.Productos.Any(prod => prod.Id == productoId))
                .Include(p => p.Productos);
        }

        public PromocionSpecification(long promocionId)
        {
            Query
                .Where(p => p.Id == promocionId)
                .Include(p => p.Productos);
        }

    }
}
